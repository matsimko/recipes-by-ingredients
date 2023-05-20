using System.Data;

namespace RbiData.Transactions;
public interface IManagedTransaction : IDisposable
{
    IDbTransaction Transaction { get; }
    IDbConnection Connection { get; }

    void Commit();
}