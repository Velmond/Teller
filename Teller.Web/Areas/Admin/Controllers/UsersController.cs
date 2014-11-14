namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels;
    using Teller.Web.Areas.User.ViewModels;
    using Model = Teller.Models.AppUser;
    using ViewModel = Teller.Web.Areas.Admin.ViewModels.UserViewModel;

    public class UsersController : KendoGridAdminController
    {
        public UsersController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<AppUser, UserViewModel>();
            //ViewData["roles"] = new List<SelectListItem>();
            //foreach (var role in this.Data.Context.Set<Role>())
            {

            }
            return this.Data.Users.All().Project<AppUser>().To<UserViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            var dbModel = base.Create<Model>(model);
            if (dbModel != null)
            {
                model.Id = dbModel.Id;
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            base.Update<Model, ViewModel>(model, model.Id);
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                this.Data.Users.Delete(model.Id/*.Value*/);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }
    }
}
