namespace Teller.Web.ViewModels.Story
{
    using System.Web;

    public class StoryFormViewModel : StoryCreateViewModel
    {
        public SelectViewModel GenresList { get; set; }

        public SelectViewModel SeriesList { get; set; }

        public HttpPostedFileBase Picture { get; set; }
    }
}