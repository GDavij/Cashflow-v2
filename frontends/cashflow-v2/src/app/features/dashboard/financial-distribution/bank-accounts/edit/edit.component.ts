import { Component, OnInit } from '@angular/core';
import { ButtonComponent } from '../../../../../components/button/button.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BankAccount } from '../../../../../models/financial-distribution/bank-account';
import { Option } from '../../../../../components/select/select.models';
import { DividerComponent } from '../../../../../components/divider/divider.component';

@Component({
  selector: 'app-edit',
  imports: [ButtonComponent, RouterLink, FormsModule, ReactiveFormsModule, DividerComponent],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditComponent implements OnInit {
  fetchedBankAccount: BankAccount | null = null;
  isLoadingCurrentBankAccount: boolean = false;
  
  isDeletingBankAccount: boolean = false;
  isSavingBankAccount: boolean = false;

  form!: FormGroup;

  options: Option<any>[] = [
    {
      label: 'Conta Corrente',
      value: 'current'
    },
    {
      label: 'Conta Poupança',
      value: 'savings'
    }
  ]

  constructor(private readonly _activatedRoute: ActivatedRoute, private readonly _fb: FormBuilder) {}

  ngOnInit(): void {
    this.createForm();
  }

  get id(): string | null {
    return this._activatedRoute.snapshot.paramMap.get('id');
  }

  handleSubmit() {
    
  }

  openDeleteDialog() {

  }

  resetForm() {
    if (this.id) {
      this.loadExistingBankAccountToForm(this.fetchedBankAccount!);
      return;
    }

    this.createForm();
  }

  private loadExistingBankAccountToForm(bankAccount: BankAccount) {
    this.form.patchValue({});
  }

  private createForm() {
    this.form = this._fb.group({
      accountType: [this.options[0].value, [Validators.required]]
    });
  }

}
