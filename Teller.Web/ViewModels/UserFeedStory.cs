namespace Teller.Web.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class UserFeedStory
    {
        public static Expression<Func<Teller.Models.Story, UserFeedStory>> FromStory
        {
            get
            {
                return story => new UserFeedStory()
                {
                    Id = story.Id,
                    Title = story.Title,
                    Content = story.Content.Length > 100 ? story.Content.Substring(0, 100) + "..." : story.Content,
                    DatePublished = story.DatePublished,
                    PicturePath = story.PicturePath,
                    ViewsCount = story.ViewsCount,
                    Author = story.Author.UserName,
                    Genre = story.Genre.Name,
                    Series = story.Series,
                    LikesCount = story.Likes.Count(l => l.Value == true),
                    DislikesCount = story.Likes.Count(l => l.Value == false),
                    CommentsCount = story.Comments.Count()
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DatePublished { get; set; }

        public string PicturePath { get; set; }

        public long ViewsCount { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public Teller.Models.Series Series { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}