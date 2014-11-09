namespace Teller.Web.ViewModels
{
    using System;
    using System.Linq;

    using Teller.Models;

    public class CommentLikeViewModel
    {
        public static Func<CommentLike, CommentLikeViewModel> FromComment
        {
            get
            {
                return like => new CommentLikeViewModel()
                {
                    Author = like.Author.UserName,
                    CommentId = like.CommentId,
                    Value = like.Value.GetValueOrDefault(true)
                };
            }
        }

        public string Author { get; set; }

        public int CommentId { get; set; }

        public bool Value { get; set; }
    }
}