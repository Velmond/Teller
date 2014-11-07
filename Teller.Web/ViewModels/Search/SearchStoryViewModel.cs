namespace Teller.Web.ViewModels.Search
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;

    public class SearchStoryViewModel
    {
        public static Expression<Func<Story, SearchStoryViewModel>> FromStory
        {
            get
            {
                return story => new SearchStoryViewModel
                {
                    Id = story.Id,
                    Title = story.Title,
                    Content = story.Content.Length > 50 ? story.Content.Substring(0, 50) + "..." : story.Content,
                    DatePublished = story.DatePublished,
                    Author = story.Author.UserName,
                    Genre = story.Genre.Name,
                    PicturePath = story.PicturePath,
                    ViewsCount = story.ViewsCount
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DatePublished { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string PicturePath { get; set; }

        public long ViewsCount { get; set; }
    }
}
