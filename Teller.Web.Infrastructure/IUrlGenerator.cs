namespace Teller.Web.Infrastructure
{
    public interface IUrlGenerator
    {
        string GenerateUrlId(int id, string title);
    }
}
