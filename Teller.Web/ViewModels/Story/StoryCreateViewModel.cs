namespace Teller.Web.ViewModels.Story
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class StoryCreateViewModel
    {
        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string Title { get; set; }

        [AllowHtml]
        [MinLength(10)]
        [Required]
        public string Content { get; set; }

        [DisplayName("Genre")]
        [Required]
        public int GenreId { get; set; }

        [DisplayName("Thumbnail picture")]
        public string PicturePath { get; set; }

        [DisplayName("Series")]
        public int? SeriesId { get; set; }

        [DisplayName("Series")]
        public string SeriesName { get; set; }

        [DisplayName("Series Genre")]
        public int? SeriesGenreId { get; set; }
    }
}