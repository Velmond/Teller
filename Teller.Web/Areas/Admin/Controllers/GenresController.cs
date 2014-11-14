namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.Genre;

    public class GenresController : AdminController
    {
        public GenresController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<Genre, GenreViewModel>()
                .ReverseMap();

            var genres = this.Data.Genres.All()
                .Project<Genre>()
                .To<GenreViewModel>();

            return genres;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Genres.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, GenreViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = Mapper.Map<Genre>(model);
                this.ChangeEntityStateAndSave(dbModel, EntityState.Added);

                if (dbModel != null)
                {
                    model.Id = dbModel.Id;
                }
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, GenreViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = this.GetById<Genre>(model.Id);

                if (this.Data.Genres.All().Any(g => g.Name == model.Name))
                {
                    throw new HttpException(400, "Genre name must be unique.");
                }
                else
                {
                    dbModel.Name = model.Name;
                    base.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
                }
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, GenreViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var storiesIds = this.Data.Stories.All().Where(s => s.GenreId == model.Id).Select(s => s.Id);
                foreach (var storyId in storiesIds)
                {
                    this.Data.Stories.Delete(storyId);
                }

                var seriesIds = this.Data.Series.All().Where(s => s.GenreId == model.Id).Select(s => s.Id);
                foreach (var seriesId in seriesIds)
                {
                    var storiesInSeriesId = this.Data.Stories.All().Where(s => s.SeriesId == seriesId).Select(s => s.Id);

                    foreach (var storyId in storiesInSeriesId)
                    {
                        this.Data.Stories.GetById(storyId).SeriesId = null;
                    }

                    this.Data.Series.Delete(seriesId);
                }

                this.Data.Genres.Delete(model.Id);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }
    }
}