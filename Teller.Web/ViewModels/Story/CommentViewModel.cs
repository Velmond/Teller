namespace Teller.Web.ViewModels.Story
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Teller.Models;
    using Teller.Web.ViewModels.Like;

    public class CommentViewModel
    {
        public static Func<Comment, CommentViewModel> FromComment
        {
            get
            {
                return comment => new CommentViewModel()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    Published = comment.Published,
                    IsFlagged = comment.IsFlagged,
                    Author = comment.Author.UserName,
                    LikesCount = comment.Likes.Count(l => l.Value == true),
                    DislikesCount = comment.Likes.Count(l => l.Value == false),
                    AllLikes = comment.Likes.Select(CommentLikeViewModel.FromComment)
                };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Published { get; set; }

        public bool IsFlagged { get; set; }

        public string Author { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public IEnumerable<CommentLikeViewModel> AllLikes { get; set; }
    }
}
