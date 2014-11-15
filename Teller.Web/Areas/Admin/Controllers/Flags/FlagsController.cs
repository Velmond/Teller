namespace Teller.Web.Areas.Admin.Controllers.Flags
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;

    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.Flag;

    public class FlagsController : AdminController
    {
        public FlagsController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, FlagViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (model.IsResolved)
                {
                    this.Data.Flags.Delete(model.Id);
                    this.Data.SaveChanges();
                }
            }

            return this.GridOperation(model, request);
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<Flag, FlagViewModel>()
                .ForMember(v => v.Author, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(v => v.Story, opt => opt.MapFrom(c => c.Story.Title))
                .ReverseMap();

            var users = this.Data.Flags.All()
                .Project<Flag>()
                .To<FlagViewModel>();

            return users;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Flags.GetById(id) as T;
        }
    }
}
