namespace Teller.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Data;
    using Teller.Models;
    using Teller.Web.ViewModels.Like;

    public class CommentLikeController : BaseController
    {
        public CommentLikeController(ITellerData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        public ActionResult Like(int id, bool like)
        {
            var comment = this.Data.Comments.Find(id);

            if(comment == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }

            this.Data.CommentLikes.Add(new CommentLike()
            {
                Value = like,
                AuthorId = this.User.Id,
                CommentId = comment.Id
            });

            this.Data.SaveChanges();

            var likesCount = comment.Likes.Count(l => l.Value == true);
            var dislikesCount = comment.Likes.Count(l => l.Value == false);

            var likesModel = new LikeViewModel()
            {
                LikesCount = likesCount,
                DislikesCount = dislikesCount
            };

            return PartialView("_CommentLikes", likesModel);
        }
    }
}
