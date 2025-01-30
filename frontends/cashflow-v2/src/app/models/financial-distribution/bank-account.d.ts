import { AccountType } from "./account-type"

export type BankAccountListItem = {
  id: number,
  name: string,
  currentValue: number,
  accountType: AccountType 
  active: boolean
}

interface BankAccount {
  id: number;
  type: AccountType;
  currentValue: number;
  name: string;
  totalTransactionsRegistered: number;
  lastTransactions: Transaction[];
  active: boolean;
}