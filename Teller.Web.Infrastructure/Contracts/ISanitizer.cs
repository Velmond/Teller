namespace Teller.Web.Infrastructure.Contracts
{
    public interface ISanitizer
    {
        string Sanitize(string html);
    }
}
