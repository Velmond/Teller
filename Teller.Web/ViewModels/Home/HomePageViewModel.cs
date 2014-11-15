namespace Teller.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Teller.Web.ViewModels.Search;

    public class HomePageViewModel
    {
        public IEnumerable<SearchStoryViewModel> MostVotedStoriesEver { get; set; }

        public IEnumerable<SearchStoryViewModel> MostVotedStoriesLastWeek { get; set; }

        public IEnumerable<SearchStoryViewModel> MostCommentedStoriesEver { get; set; }

        public IEnumerable<SearchStoryViewModel> MostCommentedStoriesLastWeek { get; set; }
    }
}