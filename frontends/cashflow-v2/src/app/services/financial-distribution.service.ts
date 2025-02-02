import { Injectable } from '@angular/core';
import { BaseHttpService } from '../abstractions/baseHttp.service';
import { HttpClient } from '@angular/common/http';
import { BankAccount, BankAccountListItem } from '../models/financial-distribution/bank-account';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinancialDistributionService extends BaseHttpService {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'bankAccounts');
  }

  public listBankAccounts() {
    return super.get<BankAccountListItem[]>('');
  }

  public getBankAccount(bankAccount: BankAccountListItem): Observable<BankAccount> {
    return super.get<BankAccount>(bankAccount.id.toString());
  }
}
