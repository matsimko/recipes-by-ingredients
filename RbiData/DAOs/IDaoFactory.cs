using RbiData.Transactions;

namespace RbiData.DAOs;

public interface IDaoFactory<T>
{
    T Create(IManagedTransaction transaction);
}