namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Common;
    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Areas.User.ViewModels.Info;
    using Teller.Web.Controllers.Base;
    using Teller.Web.Infrastructure.Sanitizers;
    using Teller.Web.Infrastructure.UrlGenerators;

    public class InfoController : BaseController
    {
        private const string SubscribeBtnPartialName = "_SubscribeBtn";
        private readonly ISanitizer sanitizer;

        public InfoController(ITellerData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult Index(string id)
        {
            var user = this.Data.Users.All()
                .Select(UserInfoViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            if (this.UserProfile != null)
            {
                ViewBag.IsSubscribedTo = this.UserProfile.SubscribedTo.Any(u => u.UserName == id);
            }

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            return this.View(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (this.UserProfile.UserName != id && this.UserProfile.Roles.FirstOrDefault() != null)
            {
                return this.RedirectToAction("Info", new { username = id });
            }

            if (this.UserProfile.UserInfo == null)
            {
                this.UserProfile.UserInfo = new UserInfo();
                this.UserProfile.UserInfo.LinkedProfiles = new LinkedProfiles();
                this.Data.Users.Update(this.UserProfile);
                this.Data.SaveChanges();
            }

            var profile = new EditInfoViewModel()
            {
                Username = this.UserProfile.UserName,
                AvatarPath = this.UserProfile.UserInfo.AvatarPath == null ? string.Empty : this.UserProfile.UserInfo.AvatarPath,
                Motto = this.UserProfile.UserInfo.Motto == null ? string.Empty : this.UserProfile.UserInfo.Motto,
                Description = this.UserProfile.UserInfo.Description == null ? string.Empty : this.UserProfile.UserInfo.Description,
                Facebook = this.UserProfile.UserInfo.LinkedProfiles.Facebook == null ? string.Empty : this.UserProfile.UserInfo.LinkedProfiles.Facebook,
                GooglePlus = this.UserProfile.UserInfo.LinkedProfiles.GooglePlus == null ? string.Empty : this.UserProfile.UserInfo.LinkedProfiles.GooglePlus,
                LinkedIn = this.UserProfile.UserInfo.LinkedProfiles.LinkedIn == null ? string.Empty : this.UserProfile.UserInfo.LinkedProfiles.LinkedIn,
                Twitter = this.UserProfile.UserInfo.LinkedProfiles.Twitter == null ? string.Empty : this.UserProfile.UserInfo.LinkedProfiles.Twitter,
                YouTube = this.UserProfile.UserInfo.LinkedProfiles.YouTube == null ? string.Empty : this.UserProfile.UserInfo.LinkedProfiles.YouTube,
            };

            return this.View(profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(string id, EditInfoViewModel profile)
        {
            if (this.UserProfile.UserName != id)
            {
                return this.RedirectToAction("Info", new { username = id });
            }

            if (profile.Description.Length < 2 || profile.Description.Length > 1000)
            {
                ModelState.AddModelError(profile.Description, "Content must be between 2 and 1000 characters long");
                return this.RedirectToAction("Edit", profile);
            }

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", profile);
            }

            if (this.UserProfile.UserInfo == null)
            {
                this.UserProfile.UserInfo = new UserInfo();
                this.UserProfile.UserInfo.LinkedProfiles = new LinkedProfiles();
            }
            else if (this.UserProfile.UserInfo.LinkedProfiles == null)
            {
                this.UserProfile.UserInfo.LinkedProfiles = new LinkedProfiles();
            }

            var newPicturePath = this.GetAvatarPath(profile.Picture, this.UserProfile.UserName);

            if (profile.Picture != null && newPicturePath != GlobalConstants.DefaultUserAvatarPicturePath)
            {
                this.UserProfile.UserInfo.AvatarPath = newPicturePath;
            }

            this.UserProfile.UserInfo.Description = this.sanitizer.Sanitize(profile.Description);
            this.UserProfile.UserInfo.Motto = profile.Motto;
            this.UserProfile.UserInfo.LinkedProfiles.Facebook = profile.Facebook;
            this.UserProfile.UserInfo.LinkedProfiles.GooglePlus = profile.GooglePlus;
            this.UserProfile.UserInfo.LinkedProfiles.LinkedIn = profile.LinkedIn;
            this.UserProfile.UserInfo.LinkedProfiles.Twitter = profile.Twitter;
            this.UserProfile.UserInfo.LinkedProfiles.YouTube = profile.YouTube;

            this.Data.Users.Update(this.UserProfile);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Info", new { id = profile.Username });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Subscribe(string id)
        {
            var user = this.Data.Users.All()
                .FirstOrDefault(u => u.UserName == id);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            this.UserProfile.SubscribedTo.Add(user);
            this.Data.SaveChanges();

            return this.PartialView(SubscribeBtnPartialName, new SubscribeButtonViewModel { Username = user.UserName, IsSubscribed = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Unsubscribe(string id)
        {
            var user = this.Data.Users.All()
                .FirstOrDefault(u => u.UserName == id);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            this.UserProfile.SubscribedTo.Remove(user);
            this.Data.SaveChanges();

            return this.PartialView(SubscribeBtnPartialName, new SubscribeButtonViewModel { Username = user.UserName, IsSubscribed = false });
        }

        private string GetAvatarPath(HttpPostedFileBase httpPostedFileBase, string username)
        {
            if (httpPostedFileBase != null && httpPostedFileBase.ContentType.StartsWith(GlobalConstants.ImageTypeSubstring))
            {
                var url = new UrlGenerator();

                string folderPath = string.Format(GlobalConstants.UserAvatarPicturePathTemplate, url.GenerateUrlId((new Random()).Next(1, 1001), username));
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
                return GlobalConstants.DefaultUserAvatarPicturePath;
            }
        }
    }
}