namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;

    public class ProfileController : BaseController
    {
        public ProfileController(ITellerData data)
            : base(data)
        {
        }

        [NonAction]
        private UserViewModel GetUser(string username)
        {
            var user = this.Data.Users.All()
                .Where(u => u.UserName == username)
                .Select(UserViewModel.FromUser)
                .FirstOrDefault();

            return user;
        }

        public ActionResult Stories(string username)
        {
            ViewBag.Username = username;
            //UserViewModel user = GetUser(username);
            // Get user's stories page
            return View();
        }

        public ActionResult Series(string username)
        {
            ViewBag.Username = username;
            // Get user's series page
            return View();
        }

        public ActionResult Info(string username)
        {
            ViewBag.Username = username;
            // Get user's info page
            return View();
        }

        //[Authorize]
        //public ActionResult Edit()
        //{
        //    var model = new EditUserProfileViewModel()
        //    {
        //        Username = this.User.UserName,
        //        AvatarPath = this.User.UserInfo.AvatarPath,
        //        Description = this.User.UserInfo.Description,
        //        Motto = this.User.UserInfo.Motto,
        //        Facebook = this.User.UserInfo.LinkedProfiles.Facebook,
        //        GooglePlus = this.User.UserInfo.LinkedProfiles.GooglePlus,
        //        LinkedIn = this.User.UserInfo.LinkedProfiles.LinkedIn,
        //        Twitter = this.User.UserInfo.LinkedProfiles.Twitter,
        //        YouTube = this.User.UserInfo.LinkedProfiles.YouTube,
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(EditUserProfileViewModel user)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return RedirectToAction("Edit", user);
        //    }

        //    var currentUser = this.Data.Users.Find(this.User.Id);

        //    this.User.UserInfo.AvatarPath = user.AvatarPath;
        //    this.User.UserInfo.Description = user.Description;
        //    this.User.UserInfo.Motto = user.Motto;
        //    this.User.UserInfo.LinkedProfiles.Facebook = user.Facebook;
        //    this.User.UserInfo.LinkedProfiles.GooglePlus = user.GooglePlus;
        //    this.User.UserInfo.LinkedProfiles.LinkedIn = user.LinkedIn;
        //    this.User.UserInfo.LinkedProfiles.Twitter = user.Twitter;
        //    this.User.UserInfo.LinkedProfiles.YouTube = user.YouTube;

        //    this.Data.Users.Update(this.User);
        //    return RedirectToAction("Info", new { username = user.Username });
        //}

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string username)
        {
            if(this.User.UserName != username && this.User.Roles.FirstOrDefault() != null)
            {
                return RedirectToAction("Info", new { username = username });
            }

            if(this.User.UserInfo == null)
            {
                this.User.UserInfo = new UserInfo();
                this.User.UserInfo.LinkedProfiles = new LinkedProfiles();
                this.Data.Users.Update(this.User);
                this.Data.SaveChanges();
            }

            var profile = new EditUserProfileViewModel()
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

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserProfileViewModel profile)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Edit", profile);
            }

            //var currentUser = this.Data.Users.Find(this.User.Id);

            this.User.UserInfo.AvatarPath = profile.AvatarPath;
            this.User.UserInfo.Description = profile.Description;
            this.User.UserInfo.Motto = profile.Motto;
            this.User.UserInfo.LinkedProfiles.Facebook = profile.Facebook;
            this.User.UserInfo.LinkedProfiles.GooglePlus = profile.GooglePlus;
            this.User.UserInfo.LinkedProfiles.LinkedIn = profile.LinkedIn;
            this.User.UserInfo.LinkedProfiles.Twitter = profile.Twitter;
            this.User.UserInfo.LinkedProfiles.YouTube = profile.YouTube;

            this.Data.Users.Update(this.User);
            return RedirectToAction("Info", new { username = profile.Username });
        }
    }
}