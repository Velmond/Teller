namespace Teller.Web.Areas.Admin.ViewModels.Genre
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;

    public class GenreViewModel : IMapFrom<Genre>
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [StringLength(25, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
