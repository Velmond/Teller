namespace Teller.Web.Controllers.Story
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Teller.Data.UnitsOfWork;
    using Teller.Models;
    using Teller.Web.Controllers.Base;
    using Teller.Web.ViewModels.Like;

    public class CommentLikeController : BaseController
    {
        private const string CommentLikesPertialName = "_CommentLikes";

        public CommentLikeController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        public ActionResult Like(int id, bool likeValue)
        {
            var comment = this.Data.Comments.GetById(id);

            if (comment == null)
            {
                throw new HttpException(400, "Story not foud in database.");
                ////return this.RedirectToAction("NotFound", "Error", new { Area = string.Empty });
            }

            comment.Likes.Add(new CommentLike
            {
                Value = likeValue,
                AuthorId = this.UserProfile.Id
            });

            //this.Data.CommentLikes.Add(new CommentLike()
            //{
            //    Value = likeValue,
            //    AuthorId = this.UserProfile.Id,
            //    CommentId = comment.Id
            //});

            this.Data.SaveChanges();

            var likesModel = new LikeViewModel()
            {
                LikesCount = comment.Likes.Count(l => l.Value == true),
                DislikesCount = comment.Likes.Count(l => l.Value == false)
            };

            return this.PartialView(CommentLikesPertialName, likesModel);
        }
    }
}
