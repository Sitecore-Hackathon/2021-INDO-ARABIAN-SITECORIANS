using System;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell;
using Sitecore.Shell.Applications.ContentManager.Galleries;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XmlControls;
using Version = Sitecore.Data.Version;
using System.Globalization;

namespace IAS.Feature.Language.LanguageCopy
{
    public class GalleryLanguagesForm : GalleryForm
    {
        protected Scrollbox Languages;
        protected GalleryMenu Options;

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            if (message.Name != "event:click")
            {
                Invoke(message, true);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                Item currentItem = GetCurrentItem();
                if (currentItem != null)
                {
                    foreach (Sitecore.Globalization.Language language in currentItem.Languages)
                    {
                        if (language == currentItem.Language) continue;

                        ID languageItemId = LanguageManager.GetLanguageItemId(language, currentItem.Database);
                        if (!languageItemId.IsNull)
                        {
                            Item languageItem = currentItem.Database.GetItem(languageItemId);
                            if (languageItem == null || !languageItem.Access.CanRead()
                                    || languageItem.Appearance.Hidden && !UserOptions.View.ShowHiddenItems)
                            {
                                continue;
                            }
                        }

                        Item item = currentItem.Database.GetItem(currentItem.ID, language);
                        if (item != null)
                        {
                            int length = item.Versions.GetVersionNumbers(false).Length;
                            if (length == 0 || item.IsFallback)
                            {
                                continue;
                            }

                            XmlControl control = ControlFactory.GetControl("Gallery.Languages.Option") as XmlControl;
                            if (control != null)
                            {
                                control["Icon"] = LanguageService.GetIcon(language, currentItem.Database);
                                control["Header"] = Sitecore.Globalization.Language.GetDisplayName(language.CultureInfo);
                                control["Description"] = (length == 1)
                                        ? Translate.Text("1 version.")
                                        : Translate.Text("{0} versions.", length);
                                control["Click"] = 
                                    $"item:addversionrecursive(id={currentItem.ID},sourceLang={language},targetLang={currentItem.Language},includeSubitems={ToggleSubitemCopying.Checked},deepCopy={ToggleDeepCopyCommand.Checked})";
//                                control["Click"] = string.Format("item:addversionrecursive(id={0},sourceLang={1},targetLang={2},includeSubitems={3})",
//                                        currentItem.ID, language, currentItem.Language, ToggleSubitemCopying.Checked);
                                control["ClassName"] = language.Name.Equals(WebUtil.GetQueryString("la"), StringComparison.OrdinalIgnoreCase)
                                        ? "scMenuPanelItemSelected"
                                        : "scMenuPanelItem";
                                Context.ClientPage.AddControl(Languages, control);
                            }
                        }
                    }
                    Context.ClientPage.AddControl(Options, new GalleryMenuLine());
                }
            }
        }

        private static Item GetCurrentItem()
        {
            string queryString = WebUtil.GetQueryString("db");
            string path = WebUtil.GetQueryString("id");

            Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(WebUtil.GetQueryString("la"));
            Version version = Version.Parse(WebUtil.GetQueryString("vs"));
            Database database = Factory.GetDatabase(queryString);
            
            Assert.IsNotNull(database, queryString);
            return database.GetItem(path, language, version);
        }
    }
}
