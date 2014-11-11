namespace Teller.Web.ViewModels.Story
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StoryViewModel : StoryCreateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }
    }
}
