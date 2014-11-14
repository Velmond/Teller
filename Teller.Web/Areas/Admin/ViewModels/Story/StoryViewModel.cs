namespace Teller.Web.Areas.Admin.ViewModels.Story
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using Teller.Web.Infrastructure.Mapping;
    using Teller.Models;

    public class StoryViewModel : IMapFrom<Story>
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [AllowHtml]
        [DataType("TextArea")]
        [MinLength(10)]
        [Required]
        [UIHint("TextArea")]
        public string Content { get; set; }

        [Display(Name = "Published")]
        [Required]
        public DateTime DatePublished { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Required]
        public string Author { get; set; }

        [DisplayName("Genre")]
        [Required]
        public int GenreId { get; set; }

        [DisplayName("Series")]
        public string Series { get; set; }

        [DisplayName("Flagged")]
        public bool IsFlagged { get; set; }

        [DisplayName("Views")]
        [Required]
        public long ViewsCount { get; set; }

        [DisplayName("Likes")]
        public int LikesCount { get; set; }

        [DisplayName("Comments")]
        public int CommentsCount { get; set; }
    }
}
