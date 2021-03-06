using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.HtmlControls;
using System;

namespace IAS.Feature.Language.LanguageCopy
{
    /// <summary>
    /// Enables "Deep copy" eg.,
    /// </summary>
    [Serializable]
    class ToggleDeepCopyCommand : Command
    {
        private const string TOGGLE_DEEP_COPY_KEY = "/Current_User/UserOptions.CopyLanguage.DeepCopy";

        /// <summary>
        /// Note the static access.
        /// </summary>
        public static bool Checked
        {
            get { return Registry.GetBool(TOGGLE_DEEP_COPY_KEY, false); }
            set { Registry.SetBool(TOGGLE_DEEP_COPY_KEY, value); }
        }

        /// <summary>
        /// Executes the command in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                Item item = context.Items[0];

                Checked = !Checked;

                string reload = $"item:load(id={item.ID})";
                Sitecore.Context.ClientPage.ClientResponse.Timer(reload, 1);
            }
        }

        public override CommandState QueryState(CommandContext context) {
            return Checked ? CommandState.Down : CommandState.Enabled;
        }
    }
}