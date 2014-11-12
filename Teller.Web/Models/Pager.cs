namespace Teller.Web.Models
{
    public class Pager
    {
        public string Area { get; set; }
        
        public string Action { get; set; }
        
        public string Controller { get; set; }
        
        public int Page { get; set; }

        public int PagesCount { get; set; }
    }
}