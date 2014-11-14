namespace Teller.Web.Areas.Admin.ViewModels.Series
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Web.Infrastructure.Mapping;
    using Teller.Models;

    public class SeriesViewModel : IMapFrom<Series>
    {
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int Id { get; set; }

        [DisplayName("Title")]
        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [DisplayName("Author")]
        [Required]
        public string Author { get; set; }

        [DisplayName("Genre")]
        [Required]
        public int GenreId { get; set; }

        [DisplayName("Stories")]
        public int? StoriesCount { get; set; }
    }
}
