namespace Teller.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    
    using Teller.Data;
    using Teller.Models;
    using Teller.Web.Areas.Admin.Controllers.Base;
    using Teller.Web.Areas.Admin.ViewModels.Comment;

    public class CommentsController : AdminController
    {
        public CommentsController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(v => v.Author,
                           opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember(v => v.StoryTitle,
                           opt => opt.MapFrom(c => c.Story.Title))
                .ForMember(v => v.LikesCount,
                           opt => opt.MapFrom(c => c.Likes.Any() ? c.Likes.Count() : 0))
                .ReverseMap();

            var users = this.Data.Comments.All()
                .Project<Comment>()
                .To<CommentViewModel>();

            return users;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Comments.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CommentViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dbModel = this.GetById<Comment>(model.Id);
                dbModel.IsFlagged = model.IsFlagged;
                dbModel.Content = model.Content;
                base.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, CommentViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                this.Data.Comments.Delete(model.Id);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }
    }
}