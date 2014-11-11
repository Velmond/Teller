namespace Teller.Web.ViewModels.Story
{
    public class StoryFormViewModel : StoryCreateViewModel
    {
        public SelectViewModel GenresList { get; set; }

        public SelectViewModel SeriesList { get; set; }
    }
}