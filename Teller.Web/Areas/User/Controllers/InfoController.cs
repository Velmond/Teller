namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;
    using Teller.Web.Infrastructure;

    public class InfoController : BaseController
    {
        private const string DefaultProfileImage = "/Images/UserPictures/default/user.png";

        public InfoController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            var user = this.Data.Users.All()
                .Select(UserInfoViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            return this.View(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (this.User.UserName != id && this.User.Roles.FirstOrDefault() != null)
            {
                return this.RedirectToAction("Info", new { username = id });
            }

            if (this.User.UserInfo == null)
            {
                this.User.UserInfo = new UserInfo();
                this.User.UserInfo.LinkedProfiles = new LinkedProfiles();
                this.Data.Users.Update(this.User);
                this.Data.SaveChanges();
            }

            var profile = new EditUserInfoViewModel()
            {
                Username = this.User.UserName,
                AvatarPath = this.User.UserInfo.AvatarPath == null ? string.Empty : this.User.UserInfo.AvatarPath,
                Motto = this.User.UserInfo.Motto == null ? string.Empty : this.User.UserInfo.Motto,
                Description = this.User.UserInfo.Description == null ? string.Empty : this.User.UserInfo.Description,
                Facebook = this.User.UserInfo.LinkedProfiles.Facebook == null ? string.Empty : this.User.UserInfo.LinkedProfiles.Facebook,
                GooglePlus = this.User.UserInfo.LinkedProfiles.GooglePlus == null ? string.Empty : this.User.UserInfo.LinkedProfiles.GooglePlus,
                LinkedIn = this.User.UserInfo.LinkedProfiles.LinkedIn == null ? string.Empty : this.User.UserInfo.LinkedProfiles.LinkedIn,
                Twitter = this.User.UserInfo.LinkedProfiles.Twitter == null ? string.Empty : this.User.UserInfo.LinkedProfiles.Twitter,
                YouTube = this.User.UserInfo.LinkedProfiles.YouTube == null ? string.Empty : this.User.UserInfo.LinkedProfiles.YouTube,
            };

            return this.View(profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(string id, EditUserInfoViewModel profile)
        {
            if (this.User.UserName != id && this.User.Roles.FirstOrDefault() != null)
            {
                return this.RedirectToAction("Info", new { username = id });
            }

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", profile);
            }

            if (this.User.UserInfo == null)
            {
                this.User.UserInfo = new UserInfo();
                this.User.UserInfo.LinkedProfiles = new LinkedProfiles();
            }
            else if (this.User.UserInfo.LinkedProfiles == null)
            {
                this.User.UserInfo.LinkedProfiles = new LinkedProfiles();
            }

            var newPicturePath = this.GetAvatarPath(profile.Picture, this.User.UserName);

            if (profile.Picture != null && newPicturePath != DefaultProfileImage)
            {
                this.User.UserInfo.AvatarPath = newPicturePath;
            }

            this.User.UserInfo.Description = profile.Description;
            this.User.UserInfo.Motto = profile.Motto;
            this.User.UserInfo.LinkedProfiles.Facebook = profile.Facebook;
            this.User.UserInfo.LinkedProfiles.GooglePlus = profile.GooglePlus;
            this.User.UserInfo.LinkedProfiles.LinkedIn = profile.LinkedIn;
            this.User.UserInfo.LinkedProfiles.Twitter = profile.Twitter;
            this.User.UserInfo.LinkedProfiles.YouTube = profile.YouTube;

            this.Data.Users.Update(this.User);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Info", new { id = profile.Username });
        }

        private string GetAvatarPath(HttpPostedFileBase httpPostedFileBase, string username)
        {
            if (httpPostedFileBase != null && httpPostedFileBase.ContentType.StartsWith("image/"))
            {
                var url = new UrlGenerator();

                string folderPath = string.Format("/Images/UserPictures/{0}", url.GenerateUrlId((new Random()).Next(1, 1001), username));
                string fullFolderPath = Server.MapPath(folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                string filePath = string.Format("{0}/{1}", folderPath, httpPostedFileBase.FileName);
                string fullFilePath = string.Format("{0}/{1}", fullFolderPath, httpPostedFileBase.FileName);
                httpPostedFileBase.SaveAs(fullFilePath);
                return filePath;
            }
            else
            {
                return DefaultProfileImage;
            }
        }

        ////private UserViewModel GetUser(string id)
        ////{
        ////    var user = this.Data.Users.All()
        ////        .Where(u => u.UserName == id)
        ////        .Select(UserViewModel.FromUser)
        ////        .FirstOrDefault();
        ////    return user;
        ////}
    }
}