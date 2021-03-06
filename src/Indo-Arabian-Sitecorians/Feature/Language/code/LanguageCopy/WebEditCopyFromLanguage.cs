﻿using System;
using System.Globalization;
using System.Linq;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace IAS.Feature.Language.LanguageCopy
{
    [Serializable]
    class WebEditCopyFromLanguage : WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length == 1)
            {
                Item currentItem = context.Items[0];
                LanguageCollection languages = LanguageManager.GetLanguages(currentItem.Database);
                
                SheerResponse.DisableOutput();
                Menu menu = new Menu();
                foreach (Sitecore.Globalization.Language language in languages)
                {
                    Item item = currentItem.Database.GetItem(currentItem.ID, language);
                    if (item != null && item.Versions.GetVersionNumbers(false).Length == 0)
                    {
                        continue;
                    }

                    string id = "L" + ShortID.NewId();
                    string languageName = GetLanguageName(language.CultureInfo);
                    string icon = LanguageService.GetIcon(language, currentItem.Database);
                    string click = string.Format("item:addversionrecursive(id={0},sourceLang={1},targetLang={2},includeSubitems={3})",
                            currentItem.ID, language, currentItem.Language, ToggleSubitemCopying.Checked);
                    menu.Add(id, languageName, icon, string.Empty, click, false, string.Empty, MenuItemType.Normal);
                }
                SheerResponse.EnableOutput();
                SheerResponse.ShowPopup("CopyFromLanguageButton", "below", menu);
            }
        }

        private static string GetLanguageName(CultureInfo info)
        {
            Assert.ArgumentNotNull(info, "info");
            if (info.IsNeutralCulture)
            {
                info = Sitecore.Globalization.Language.CreateSpecificCulture(info.Name);
            }
            string englishName = info.EnglishName;
            if (englishName.IndexOf("(") > 0)
            {
                englishName = StringUtil.Left(englishName, englishName.IndexOf("(")).Trim();
            }
            return englishName;
        }


        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }
            Item item = context.Items[0];
            if (new[] { TemplateIDs.Template, TemplateIDs.TemplateSection, TemplateIDs.TemplateField }.Contains(item.TemplateID))
            {
                return CommandState.Hidden;
            }

            if (item.Versions.Count > 0 && !(item.Paths.IsContentItem || item.Paths.IsMediaItem))
            {
                return CommandState.Disabled;
            }
            if (item.Appearance.ReadOnly)
            {
                return CommandState.Disabled;
            }
            if (Context.IsAdministrator)
            {
                return CommandState.Enabled;
            }

            if (!item.Access.CanWrite())
            {
                return CommandState.Disabled;
            }
            if (!item.Locking.CanLock() && !item.Locking.HasLock())
            {
                return CommandState.Disabled;
            }
            if (!item.Access.CanWriteLanguage())
            {
                return CommandState.Disabled;
            }

            return base.QueryState(context);
        }
    }
}

