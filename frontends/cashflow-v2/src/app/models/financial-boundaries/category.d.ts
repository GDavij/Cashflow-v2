export interface CategoryListItem {
  id: number;
  name: string;
  active: boolean;
}

export interface Category {
  id: number;
  name: string;
  maximumBudgetInvestment?: number;
  maximumMoneyInvestment?: number;
  totalTransactionsRegistered: number;
  active: boolean;
}

export interface CategoryTransactionsAggregate {
  year: number;
  transactionsUsageAggregate: {
    month: number,
    totalTransactions: number,
    totalDeposit: number,
    totalWithdrawl: number,
    hasReachedLimit: boolean
  }[];
}
