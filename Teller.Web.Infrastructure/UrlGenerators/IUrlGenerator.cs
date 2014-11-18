namespace Teller.Web.Infrastructure.UrlGenerators
{
    public interface IUrlGenerator
    {
        string GenerateUrlId(int id, string title);
    }
}
