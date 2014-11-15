namespace Teller.Web.Infrastructure.Sanitizers
{
    public interface ISanitizer
    {
        string Sanitize(string html);
    }
}
