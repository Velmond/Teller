namespace Teller.Web.Areas.Admin.ViewModels.Flag
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;

    public class FlagViewModel : IMapFrom<Flag>
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Is Resolved")]
        [Required]
        public bool IsResolved { get; set; }

        [Display(Name = "Flagged On")]
        [Required]
        public DateTime DateFlagged { get; set; }
        
        [Display(Name = "Author")]
        [Required]
        public string Author { get; set; }

        [Required]
        public int StoryId { get; set; }

        [Display(Name = "Story")]
        [Required]
        public string Story { get; set; }
    }
}
