using System.Data;

namespace RbiData;
public interface IManagedTransaction : IDisposable
{
    IDbTransaction Transaction { get; }
    IDbConnection Connection { get; }

    void Commit();
}