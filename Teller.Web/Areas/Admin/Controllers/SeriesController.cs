namespace Teller.Web.Areas.Admin.Controllers
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

    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.Series;
    using Teller.Web.Helpers;

    public class SeriesController : AdminController
    {
        public SeriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var genres = this.GetGenres();
            return this.View(genres);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, SeriesViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = this.GetById<Series>(model.Id);

                dbModel.Title = model.Title;
                dbModel.GenreId = model.GenreId;

                this.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, SeriesViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var storiesIds = this.Data.Stories.All().Where(s => s.SeriesId == model.Id).Select(s => s.Id);
                foreach (var storyId in storiesIds)
                {
                    this.Data.Stories.GetById(storyId).SeriesId = null;
                }

                this.Data.Series.Delete(model.Id);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<Series, SeriesViewModel>()
                .ForMember(u => u.Author, v => v.MapFrom(u => u.Author.UserName))
                .ForMember(u => u.StoriesCount, v => v.MapFrom(u => u.Stories.Any() ? u.Stories.Count() : 0))
                .ReverseMap();

            var series = this.Data.Series.All()
                .Project<Series>()
                .To<SeriesViewModel>();

            return series;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Series.GetById(id) as T;
        }

        [ChildActionOnly]
        private IQueryable<SelectListItem> GetGenres()
        {
            var genres = this.HttpContext.Cache[Constants.GenresCacheKey];

            if (genres == null)
            {
                genres = this.Data.Genres.All()
                    .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() });

                this.HttpContext.Cache.Add(
                    Constants.GenresCacheKey,
                    genres,
                    null,
                    DateTime.Now.AddDays(1),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    null);
            }

            return genres as IQueryable<SelectListItem>;
        }
    }
}
