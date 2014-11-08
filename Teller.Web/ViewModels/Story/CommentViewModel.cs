namespace Teller.Web.ViewModels.Story
{
    using System;
    using System.Linq;

    using Teller.Models;

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
                    DislikesCount = comment.Likes.Count(l => l.Value == false)
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
    }
}
