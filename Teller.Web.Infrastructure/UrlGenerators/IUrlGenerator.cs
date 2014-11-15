namespace Teller.Web.Infrastructure.UrlGeneratotrs
{
    public interface IUrlGenerator
    {
        string GenerateUrlId(int id, string title);
    }
}
