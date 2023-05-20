using RbiData.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Transactions;
public class TransactionExecutor<TArg>
{
    private readonly IManagedTransactionFactory _transactionFactory;
    private readonly Func<IManagedTransaction, TArg> _preparationFunc;

    public TransactionExecutor(IManagedTransactionFactory transactionFactory, IDaoFactory<TArg> daoFactory)
    {
        _transactionFactory = transactionFactory;
        _preparationFunc = (transaction) => daoFactory.Create(transaction);
    }

    public TransactionExecutor(IManagedTransactionFactory transactionFactory, Func<IManagedTransaction, TArg> preparationFunc)
    {
        _transactionFactory = transactionFactory;
        _preparationFunc = preparationFunc;
    }


    public async Task Execute(Func<TArg, Task> action)
    {
        using var transaction = _transactionFactory.Create();
        TArg arg = _preparationFunc(transaction);

        await action(arg);

        transaction.Commit();
    }

    public async Task<T> Execute<T>(Func<TArg, Task<T>> func)
    {
        using var transaction = _transactionFactory.Create();
        TArg arg = _preparationFunc(transaction);

        T result = await func(arg);

        transaction.Commit();
        return result;
    }
}
