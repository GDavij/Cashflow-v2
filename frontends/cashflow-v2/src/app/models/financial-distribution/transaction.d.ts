export interface Transaction {
  id: number;
  description: string;
  doneAt: string;
  transactionMethod: string;
  category: Category;
}