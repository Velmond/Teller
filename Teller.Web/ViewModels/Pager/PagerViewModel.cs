namespace Teller.Web.ViewModels.Pager
{
    public class PagerViewModel
    {
        public string Area { get; set; }
        
        public string Action { get; set; }
        
        public string Controller { get; set; }
        
        public int Page { get; set; }

        public int PagesCount { get; set; }
    }
}