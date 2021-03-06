using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore;
using Sitecore.Mvc;
using Sitecore.Mvc.Controllers;
using Sitecore.Data;
using Sitecore.Data.Items;
using IAS.Feature.SocialShare;

namespace IAS.Feature.SocialShare.Controllers
{
    public class SocialShareController : SitecoreController
    {
        // GET: SocialShare
        public ActionResult SocialShare()
        {
            Database db = Sitecore.Context.Database;
            Item addThisSettingItem = db.GetItem("{2AAF3F07-E266-414D-8441-B2012BD896D9}");
            string addThisID = addThisSettingItem.Fields["AddThis ID"].Value;
            return View((object)addThisID);
        }
    }
}