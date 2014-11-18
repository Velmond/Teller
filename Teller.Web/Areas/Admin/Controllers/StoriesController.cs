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
    
    using Teller.Common;
    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.Story;
    using Teller.Web.Helpers;

    public class StoriesController : AdminController
    {
        public StoriesController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var genres = this.GetGenres();
            return this.View(genres);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, StoryViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = this.GetById<Story>(model.Id);
                if (string.IsNullOrEmpty(model.PicturePath.Trim()))
                {
                    model.PicturePath = GlobalConstants.DefaultStoryPicturePath;
                }

                dbModel.Title = model.Title;
                dbModel.Content = model.Content;
                dbModel.PicturePath = model.PicturePath;
                dbModel.GenreId = model.GenreId;

                if (!model.IsFlagged && dbModel.Flags.Any())
                {
                    var flagIds = this.Data.Flags.All()
                        .Where(f => f.StoryId == model.Id)
                        .Select(f => f.Id);

                    foreach (var flagId in flagIds)
                    {
                        this.Data.Flags.Delete(flagId);
                    }
                }

                this.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, StoryViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                this.Data.Stories.Delete(model.Id);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<Story, StoryViewModel>()
                .ForMember(u => u.Author, v => v.MapFrom(u => u.Author.UserName))
                .ForMember(u => u.IsFlagged, v => v.MapFrom(u => u.Flags.Any()))
                .ForMember(u => u.CommentsCount, v => v.MapFrom(u => u.Comments.Count()))
                .ForMember(u => u.LikesCount, v => v.MapFrom(u => u.Likes.Count()))
                .ForMember(u => u.Series, v => v.MapFrom(u => u.Series.Title ?? string.Empty))
                .ReverseMap();

            var stories = this.Data.Stories.All()
                .Project<Story>()
                .To<StoryViewModel>();

            return stories;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Stories.GetById(id) as T;
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
