namespace Teller.Web.Infrastructure.Sanitizers
{
    using Html;

    public class HtmlSanitizerAdapter : ISanitizer
    {
        public string Sanitize(string html)
        {
            var sanitizer = new HtmlSanitizer();
            var result = sanitizer.Sanitize(html);
            return result;
        }
    }
}
