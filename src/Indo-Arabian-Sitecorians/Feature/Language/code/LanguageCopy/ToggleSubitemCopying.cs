using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Shell;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace IAS.Feature.Language.LanguageCopy
{
    [Serializable]
    class ToggleSubitemCopying : Command
    {
        public static bool Checked
        {
            get
            {
                return Registry.GetBool("/Current_User/UserOptions.CopyLanguage.IncludeSubitems", false);
            }
            set
            {
                Registry.SetBool("/Current_User/UserOptions.CopyLanguage.IncludeSubitems", value);
            }
        }

        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                Item item = context.Items[0];

                Checked = !Checked;

                // reload the page 
                string reload = string.Format("item:load(id={0})", item.ID);
                Sitecore.Context.ClientPage.ClientResponse.Timer(reload, 1);
            }
        }

        //public override string GetClick(CommandContext context, string click)
        //{
        //    return base.GetClick(context, click);
        //    //return "javascript:scContent.toggleFolders()";
        //}

        public override CommandState QueryState(CommandContext context)
        {
            if (Checked)
            {
                return CommandState.Down;
            }
            return CommandState.Enabled;
        }
    }
}
