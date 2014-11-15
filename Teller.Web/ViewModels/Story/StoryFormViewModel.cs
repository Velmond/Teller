namespace Teller.Web.ViewModels.Story
{
    public class StoryFormViewModel
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public FormViewModel Model { get; set; }

        public string PageSubTitle { get; set; }
    }
}