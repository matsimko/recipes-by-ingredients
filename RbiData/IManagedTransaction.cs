using System.Data;

namespace RbiData;
public interface IManagedTransaction
{
    IDbTransaction Transaction { get; }
    IDbConnection Connection { get; }

    void Commit();
    void Dispose();
}