namespace Teller.Web.ViewModels.Search
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public string Pattern { get; set; }

        public IEnumerable<SearchStoryViewModel> Stories { get; set; }

        public IEnumerable<SearchUserViewModel> Users { get; set; }
        
        public IEnumerable<SearchSeriesViewModel> Series { get; set; }
    }
}