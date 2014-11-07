namespace Teller.Web.ViewModels.Story
{
    using System.ComponentModel.DataAnnotations;

    public class StoryFormViewModel
    {
        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string Title { get; set; }

        [MinLength(10)]
        [Required]
        public string Content { get; set; }

        [Required]
        public int GenreId { get; set; }

        public SelectViewModel GenresList { get; set; }

        public string PicturePath { get; set; }

        public int? SeriesId { get; set; }

        public SelectViewModel SeriesList { get; set; }
    }
}