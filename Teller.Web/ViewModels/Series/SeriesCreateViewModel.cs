namespace Teller.Web.ViewModels.Series
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class SeriesCreateViewModel
    {
        [DisplayName("Series Title")]
        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string SeriesTitle { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [DisplayName("Series Genre")]
        [Required]
        public int SeriesGenreId { get; set; }

        public SelectViewModel GenresList { get; set; }
    }
}