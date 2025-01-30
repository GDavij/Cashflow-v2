import { Component, OnInit } from '@angular/core';
import { ButtonComponent } from '../../../../../components/button/button.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BankAccount } from '../../../../../models/financial-distribution/bank-account';

@Component({
  selector: 'app-edit',
  imports: [ButtonComponent, RouterLink, FormsModule, ReactiveFormsModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditComponent implements OnInit {
  fetchedBankAccount: BankAccount | null = null;
  isLoadingCurrentBankAccount: boolean = false;
  
  isDeletingBankAccount: boolean = false;
  isSavingBankAccount: boolean = false;

  form!: FormGroup;

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
    this.form = this._fb.group({});
  }

}
