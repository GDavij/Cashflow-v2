﻿using Cashflow.Core;
using Cashflow.Domain.Events.FinancialDistribution;

namespace Cashflow.Domain.Entities;

public class BankAccount : OwnableEntity<BankAccount>
{
    public short AccountTypeId { get; init; }
    public AccountType AccountType { get; init; }
    public decimal CurrentValue { get; init; }
    public string Name { get; private set; }

    public ICollection<Transaction> Transactions { get; init; } = new List<Transaction>();

    public BankAccount(short accountTypeId, string name)
        : base()
    {
        AccountTypeId = accountTypeId;
        Name = name;
        CurrentValue = 0.0M;
    }



    public void RenameTo(string name)
    {
        if (name == Name)
        {
            return;
        }

        RaiseEvent(new BankAccountRenamedEvent(this, Name));
        Name = name;
    }
}