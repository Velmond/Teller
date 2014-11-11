namespace Teller.Web.ViewModels.Story
{
    public class FormViewModel : StoryCreateViewModel
    {
        public SelectViewModel GenresList { get; set; }

        public SelectViewModel SeriesList { get; set; }
    }
}