import { Component } from '@angular/core';
import { ButtonComponent } from '../../../../components/button/button.component';
import { CdkMenuTrigger } from '@angular/cdk/menu';
import { BankAccount, BankAccountListItem } from '../../../../models/financial-distribution/bank-account';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-bank-accounts',
  imports: [ButtonComponent, CdkMenuTrigger, RouterLink],
  templateUrl: './bank-accounts.component.html',
  styleUrl: './bank-accounts.component.scss'
})
export class BankAccountsComponent {
  currentBankAccount: BankAccount | null = null;

  bankAccounts: BankAccountListItem[] = [];
  
  editCurrentBankAccount(): void {}
}
