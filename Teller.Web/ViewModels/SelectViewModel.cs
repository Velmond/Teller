namespace Teller.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SelectViewModel
    {
        public string Id { get; set; }

        public IEnumerable<SelectListItem> List { get; set; }
    }
}