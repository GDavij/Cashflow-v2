import { Component, OnInit } from '@angular/core';
import { ButtonComponent } from '../../../../components/button/button.component';
import { CdkMenuTrigger } from '@angular/cdk/menu';
import {
  BankAccount,
  BankAccountListItem,
} from '../../../../models/financial-distribution/bank-account';
import { Router, RouterLink } from '@angular/router';
import { SelectComponent } from '../../../../components/select/select.component';
import { SelectContainerComponent } from '../../../../components/select/select-container/select-container.component';
import { Option } from '../../../../components/select/select.models';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { FinancialDistributionService } from '../../../../services/financial-distribution.service';
import { CacheService } from '../../../../services/cache.service';
import { catchError, EMPTY, finalize, retry } from 'rxjs';
import { SelectOptionComponent } from '../../../../components/select/select-option/select-option.component';
import { LoaderComponent } from "../../../../components/loader/loader.component";
import { DividerComponent } from '../../../../components/divider/divider.component';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-bank-accounts',
  imports: [
    ButtonComponent,
    RouterLink,
    SelectComponent,
    SelectContainerComponent,
    SelectOptionComponent,
    ReactiveFormsModule,
    LoaderComponent,
    DividerComponent,
    CurrencyPipe
],
  templateUrl: './bank-accounts.component.html',
  styleUrl: './bank-accounts.component.scss',
})
export class BankAccountsComponent implements OnInit {
  isLoadingCurrentBankAccount: boolean = false;
  currentBankAccount: BankAccount | null = null;

  selectedOption = new FormControl<number>(0);
  bankAccounts: BankAccountListItem[] = [];
  bankAccountOptions: Option<number>[] = [];

  constructor(
    private readonly _financialDistributionService: FinancialDistributionService,
    private readonly _cacheService: CacheService,
    private readonly _router: Router
  ) {}

  ngOnInit(): void {
    this.loadBankAccounts();
    this.selectedOption.valueChanges.subscribe((v) => {
      const selectedBankAccount = this.bankAccounts.find((ba) => ba.id === v);
      this.viewBankAccount(selectedBankAccount!);
    });
  }

  editCurrentBankAccount(): void {
    this._router.navigate([
      '/bank-accounts',
      'edit',
      this.currentBankAccount?.id,
    ])

  }

  private viewBankAccount(bankAccount: BankAccountListItem): void {
    this.isLoadingCurrentBankAccount = true;
    const { subject } = this._cacheService.getOrResolveTo(
      () =>
        this._financialDistributionService.getBankAccount(bankAccount).pipe(
          retry(3),
          catchError((httpError) => {
            console.error({ httpError });
            return EMPTY;
          }),
        ),
      `bank-account-${bankAccount.id}`,
    );

    subject.subscribe((bankAccount) => {
      this.currentBankAccount = bankAccount;

      setTimeout(() => {
        this.isLoadingCurrentBankAccount = false;
      }, 200)
    });
  }

  private loadBankAccounts() {
    this._financialDistributionService
      .listBankAccounts()
      .pipe(
        retry(3),
        catchError((httpError) => {
          console.error({ httpError });
          return EMPTY;
        }),
      )
      .subscribe((bankAccounts) => {
        this.bankAccounts = bankAccounts;
        this.bankAccountOptions = bankAccounts.map((bankAccount) => ({
          label: bankAccount.name,
          value: bankAccount.id,
        }));
        if (bankAccounts.length > 0) {
          this.selectedOption.setValue(this.bankAccountOptions[0].value);
        }
      });
  }
}
