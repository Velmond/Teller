namespace Teller.Web.ViewModels.Story
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StoryViewModel : StoryCreateViewModel
    {
        [Required]
        public DateTime DatePublished { get; set; }
    }
}
