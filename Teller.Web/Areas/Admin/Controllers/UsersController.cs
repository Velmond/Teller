﻿namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    using Teller.Common;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.User;

    public class UsersController : AdminController
    {
        private const string UserRolesCacheKey = "admin-panel-user-roles";

        public UsersController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var roles = this.GetRoles();
            return this.View(roles);
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<AppUser, UserViewModel>()
                .ForMember(u => u.AvatarPath,
                           v => v.MapFrom(u => u.UserInfo.AvatarPath))
                .ForMember(u => u.RoleId,
                           v => v.MapFrom(u => u.Roles.FirstOrDefault().RoleId))
                .ForMember(u => u.CommentFlags,
                           v => v.MapFrom(u => u.Comments.Count(c => c.IsFlagged)))
                .ForMember(u => u.StoryFlags,
                           v => v.MapFrom(u => u.Stories.Count(s => s.Flags.Any())))
                .ReverseMap();

            var users = this.Data.Users.All()
                .Project<AppUser>()
                .To<UserViewModel>();

            return users;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = this.GetById<AppUser>(model.Id);
                if (dbModel.UserInfo == null)
                {
                    dbModel.UserInfo = new UserInfo();
                    dbModel.UserInfo.LinkedProfiles = new LinkedProfiles();
                }
                
                if (string.IsNullOrEmpty(model.AvatarPath.Trim()))
                {
                    model.AvatarPath = GlobalConstants.DefaultUserAvatarPicturePath;
                }

                dbModel.UserInfo.AvatarPath = model.AvatarPath;

                var roles = dbModel.Roles;
                roles.Clear();

                dbModel.Roles.Add(new IdentityUserRole() { RoleId = model.RoleId });

                base.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                this.Data.Users.Delete(model.Id);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        [ChildActionOnly]
        private IQueryable<SelectListItem> GetRoles()
        {
            var roles = this.HttpContext.Cache[UserRolesCacheKey];

            if (roles == null)
            {
                roles = this.Data.Roles.All()
                    .Select(r => new SelectListItem() { Text = r.Name, Value = r.Id });

                this.HttpContext.Cache.Add(
                    UserRolesCacheKey,
                    roles,
                    null,
                    DateTime.Now.AddDays(1),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }

            return roles as IQueryable<SelectListItem>;
        }
    }
}
