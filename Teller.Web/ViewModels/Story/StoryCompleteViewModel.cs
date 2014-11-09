namespace Teller.Web.ViewModels.Story
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Web.ViewModels.Series;

    public class StoryCompleteViewModel
    {
        public static Expression<Func<Teller.Models.Story, StoryCompleteViewModel>> FromStory
        {
            get
            {
                return story => new StoryCompleteViewModel()
                { 
                    Id = story.Id,
                    Title = story.Title,
                    Content = story.Content,
                    DatePublished = story.DatePublished,
                    PicturePath = story.PicturePath,
                    ViewsCount = story.ViewsCount,
                    Author = story.Author.UserName,
                    Genre = story.Genre.Name,
                    LikesCount = story.Likes.Count(l => l.Value == true),
                    DislikesCount = story.Likes.Count(l => l.Value == false),
                    FavouritedByCount = story.FavouritedBy.Count(),
                    IsFlagged = story.Flags.Any(f => !f.IsResolved),
                    Comments = story.Comments.Select(CommentViewModel.FromComment)
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

        public SeriesViewModel Series { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public int FavouritedByCount { get; set; }

        public bool IsFlagged { get; set; }
    }
}