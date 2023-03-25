using System.Data;

namespace RbiData;
public interface IDbAccess
{
    IDbTransaction Transaction { get; }
    IDbConnection Connection { get; }

    void Commit();
    void Dispose();
}