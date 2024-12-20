using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class Transaction : OwnableEntity<Transaction>
{
    public long? BankAccountId { get; init; }
    public BankAccount? BankAccount { get; init; }
    public long? CategoryId { get; init; }
    public Category? Category { get; init; }
    public string Description { get; init; }
    public DateTime DoneAt { get; init; }
    public short Month { get; init; }
    public short TransactionMethodId { get; init; }
    public TransactionMethod TransactionMethod { get; init; }
    public decimal Value { get; init; }
    public int Year { get; init; }
}