namespace RbiData.Transactions;

public interface IManagedTransactionFactory
{
    IManagedTransaction Create();
}