using System.Data;

namespace ecom.DBcontexts
{
    public interface IDbContext
    {
        IDbConnection GetConnection();
    }
}
