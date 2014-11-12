namespace Teller.Web.ViewModels.Story
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class StoryCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters long")]
        public string Title { get; set; }

        [AllowHtml]
        [DataType("tinymce_full")]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters long")]
        [Required(ErrorMessage = "Content is required")]
        [UIHint("tinymce_full")]
        public string Content { get; set; }

        [DisplayName("Genre")]
        [Required(ErrorMessage = "Genre is required")]
        public int GenreId { get; set; }

        public HttpPostedFileBase Picture { get; set; }

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