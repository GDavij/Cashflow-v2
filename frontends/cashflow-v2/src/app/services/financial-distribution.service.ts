import { Injectable } from '@angular/core';
import { BaseHttpService } from '../abstractions/baseHttp.service';
import { HttpClient } from '@angular/common/http';
import { BankAccount, BankAccountListItem } from '../models/financial-distribution/bank-account';
import { Observable } from 'rxjs';
import { AccountType } from '../models/financial-distribution/account-type';
import { CacheService } from './cache.service';

@Injectable({
  providedIn: 'root'
})
export class FinancialDistributionService extends BaseHttpService {

  private readonly _cacheKey: string = "financial-distribution";

  // Use Cache Service for Application wide cache
  constructor(httpClient: HttpClient, 
              private readonly _cacheService: CacheService) {
    super(httpClient, 'bankAccounts');
  }

  public listBankAccounts() {
    return super.get<BankAccountListItem[]>('');
  }

  public getBankAccount(bankAccount: BankAccountListItem): Observable<BankAccount> {
    return super.get<BankAccount>(bankAccount.id.toString());
  }

  public listTypes() {
    return this._cacheService.getOrResolveTo(() => super.get<AccountType>('types'), `${this._cacheKey}-account-types`);
  }
}
