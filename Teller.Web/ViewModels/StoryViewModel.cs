namespace Teller.Web.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StoryViewModel
    {
        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string Title { get; set; }

        [MinLength(10)]
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }

        [Required]
        public string PicturePath { get; set; }

        public int GenreId { get; set; }

        public int? SeriesId { get; set; }
    }
}
