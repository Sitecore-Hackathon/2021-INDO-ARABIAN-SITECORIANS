using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Version = Sitecore.Data.Version;

namespace IAS.Feature.Language.LanguageCopy
{
    [Serializable]
    class AddVersionRecursiveCommand : Command
    {
        private HashSet<ID> LinkSet;

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items != null && context.Items.Length == 1)
            {
                Context.ClientPage.Start(this, "Run", context.Parameters);
            }
        }

        protected void Run(ClientPipelineArgs args)
        {
            Log.Info("CopyFromLanguage beginning.", this);
            Assert.ArgumentNotNull(args, "args");
            string id = args.Parameters["id"];
            Sitecore.Globalization.Language sourceLang = Sitecore.Globalization.Language.Parse(args.Parameters["sourceLang"]);
            Sitecore.Globalization.Language targetLang = Sitecore.Globalization.Language.Parse(args.Parameters["targetLang"]);

            bool includeSubitems = false;
            bool.TryParse(args.Parameters["includeSubitems"], out includeSubitems);

            bool deepCopy = true;
            bool.TryParse(args.Parameters["deepCopy"], out deepCopy);

            LinkSet = new HashSet<ID>();

            Item item = Context.ContentDatabase.GetItem(id, targetLang);
            if (item == null)
            {
                return;
            }

            if (Context.IsAdministrator || (item.Access.CanWrite() && (item.Locking.CanLock() || item.Locking.HasLock())))
            {
                if (SheerResponse.CheckModified())
                {
                    // pause indexing for recursive operations
                    IndexCustodian.PauseIndexing();
                    try
                    {
                        // Add the current item itself.
                        AddAllReferences(item);

                        // Add the standard values items (if any) of the item's template and all base templates.
                        RecursiveAddTemplateStandardValues(item.Template);
                    
                        // Add the data source items, if any.
                        LayoutField layoutField = item.Fields[FieldIDs.LayoutField];
                        if (!string.IsNullOrEmpty(layoutField.Value))
                        {
                            LayoutDefinition layout = LayoutDefinition.Parse(layoutField.Value);

                            foreach (DeviceDefinition device in layout.Devices)
                            {
                                foreach (RenderingDefinition rendering in device.Renderings)
                                {
                                    Item datasourceItem = Context.ContentDatabase.GetItem(rendering.Datasource ?? string.Empty, targetLang);
                                    if (datasourceItem == null)
                                    {
                                        continue;
                                    }

                                    AddAllReferences(datasourceItem);

                                    if (ChildrenGroupingTemplates.Contains(datasourceItem.TemplateName))
                                    {
                                        foreach (Item childItem in datasourceItem.Children)
                                        {
                                            AddAllReferences(childItem);
                                        }
                                    }
                                }
                            }
                        }

                        // Add the child items of the current item, if selected.
                        if (includeSubitems)
                        {
                            RecursiveAddSubitemReferences(item);
                        }

                        // Do the copies.
                        Log.Info(string.Format("CopyFromLanguage: Found {0} items to copy from {1} to {2}.", LinkSet.Count, sourceLang, targetLang), this);
                        foreach (ID linkId in LinkSet)
                        {
                            CopyVersion(linkId, sourceLang, targetLang, deepCopy);
                            Log.Info(string.Format("CopyFromLanguage: Copied {0}", linkId), this);
                        }
                    }
                    catch (Exception x)
                    {
                        Log.Error(x.Message, x, this);
                    }
                    finally
                    {
                        IndexCustodian.ResumeIndexing();
                    }
                }
            }
            Log.Info("CopyFromLanguage finished.", this);
        }

        private void RecursiveAddTemplateStandardValues(TemplateItem template)
        {
            if (template.StandardValues != null)
                AddAllReferences(template.StandardValues);

            foreach (var baseTemplate in template.BaseTemplates)
            {
                RecursiveAddTemplateStandardValues(baseTemplate);
            }
        }

        private void RecursiveAddSubitemReferences(Item item)
        {
            foreach (Item child in item.Children)
            {
                AddAllReferences(child);
                RecursiveAddSubitemReferences(child);
            }
        }

        private void AddAllReferences(Item item)
        {
            if (LinkSet.Add(item.ID))
            {
                foreach (Item reference in item.Links.GetValidLinks(false).Select(il => il.GetTargetItem()))
                {
                    if (reference.Paths.IsContentItem || reference.Paths.IsMediaItem)
                    {
                        LinkSet.Add(reference.ID);
                    }
                }
            }
        }

        /// <summary>
        /// Copy item version to new language. If deepCopy is true, the field values will be changed on the new item. If deepCopy is false, then
        /// the field values will fallback to standard values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sourceLang"></param>
        /// <param name="targetLang"></param>
        /// <param name="deepCopy"></param>
        private void CopyVersion(ID id, Sitecore.Globalization.Language sourceLang, Sitecore.Globalization.Language targetLang, bool deepCopy)
        {
            Item source = Context.ContentDatabase.GetItem(id, sourceLang, Version.Latest);
            Item target = Context.ContentDatabase.GetItem(id, targetLang);

            Log.Debug($"CopyFromLanguage: Start processing {source.Paths.FullPath}");

            if (source == null || target == null)
            {
                return;
            }
            if (target.Versions.Count > 0 && !(target.Paths.IsContentItem || target.Paths.IsMediaItem))
                return;

            bool needNewVersion = false;
            foreach (Field field in source.Fields)
            {
                if ((!field.Shared && 
                    field.Name != string.Empty &&
                    field.Name != "__Revision" && field.Name != "__Updated" && field.Name != "__Updated by" && field.Name != "__Created" &&
                    source[field.Name] != target[field.Name])
                    || source.Fields.Count(f => !f.Shared && !string.IsNullOrWhiteSpace(f.Name)) <= 4)
                {
                    needNewVersion = true;
                    break;
                }
            }
            if (needNewVersion || target.IsFallback)
            {
                var newTarget = target.Versions.AddVersion();
                if (deepCopy)
                {
                    newTarget.Editing.BeginEdit();
                    foreach (Field field in source.Fields)
                    {
                        if (!field.Shared && field.Name != string.Empty)
                        {
                            newTarget[field.Name] = source[field.Name];
                        }
                    }
                    newTarget.Editing.EndEdit();
                }
            }
        }

        protected static IEnumerable<string> ChildrenGroupingTemplates
        {
            get
            {
                string rawSetting = Settings.GetSetting("VersionFromLanguage.ChildrenGroupingTemplates", string.Empty);
                return rawSetting.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
            }
        }
    }
}
