namespace Teller.Web.Infrastructure.Contracts
{
    public interface IUrlGenerator
    {
        string GenerateUrlId(int id, string title);
    }
}
