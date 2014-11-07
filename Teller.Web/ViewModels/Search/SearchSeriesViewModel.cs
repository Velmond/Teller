namespace Teller.Web.ViewModels.Search
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;

    public class SearchSeriesViewModel
    {
        public static Expression<Func<Series, SearchSeriesViewModel>> FromSeries
        {
            get
            {
                return series => new SearchSeriesViewModel
                {
                    Id = series.Id,
                    Title = series.Title,
                    Author = series.Author.UserName,
                    Genre = series.Genre.Name
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }
    }
}
