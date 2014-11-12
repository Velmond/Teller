namespace Teller.Web.ViewModels.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;
    using Teller.Web.ViewModels.Story;
    
    public class SeriesViewModel
    {
        public static Expression<Func<Series, SeriesViewModel>> FromSeries
        {
            get
            {
                return series => new SeriesViewModel()
                {
                    Id = series.Id,
                    Title = series.Title,
                    Author = series.Author.UserName,
                    Genre = series.Genre.Name,
                    TotalViewsCount = series.Stories.Sum(s => s.ViewsCount),
                    TotalLikesCount = series.Stories.Sum(s => s.Likes.Count()),
                    TotalFavoritesCount = series.Stories.Sum(s => s.FavouritedBy.Count())
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public int TotalLikesCount { get; set; }

        public int TotalFavoritesCount { get; set; }

        public long TotalViewsCount { get; set; }

        public ICollection<StoryViewModel> Stories { get; set; }
    }
}
