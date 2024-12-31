import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule, FormGroup, FormBuilder, ReactiveFormsModule, Validators, FormControl } from '@angular/forms';
import { CdkMenuModule } from '@angular/cdk/menu';
import { FINANCIAL_BOUNDARIES } from '../../../../enums/FINANCIAL_BOUNDARIES';
import { CommonModule, NgIf } from '@angular/common';
import { tap } from 'rxjs';

@Component({
  selector: 'app-edit',
  imports: [FormsModule, ReactiveFormsModule, CdkMenuModule, CommonModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditComponent implements OnInit {
  readonly financialBoundaries: FINANCIAL_BOUNDARIES[] = [FINANCIAL_BOUNDARIES.NONE, FINANCIAL_BOUNDARIES.MONEY, FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT];

  isLoadingCurrentCategory: boolean = false;
  hasBoundary: FINANCIAL_BOUNDARIES = FINANCIAL_BOUNDARIES.NONE;
  form!: FormGroup;

  constructor(private readonly activatedRoute: ActivatedRoute, private readonly _fb: FormBuilder) { }

  get id(): string | null {
    return this.activatedRoute.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.form = this._fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(60)]],
    })

    this.changeBoundaryTo(FINANCIAL_BOUNDARIES.MONEY)
  }

  changeBoundaryTo(boundaryValue: FINANCIAL_BOUNDARIES) {
    switch (boundaryValue) {
      case FINANCIAL_BOUNDARIES.MONEY:
        this.form.removeControl('maximumBudgetInvestment');
        this.form.addControl('maximumMoneyInvestment', new FormControl<number | null>(null, [Validators.required, Validators.min(0)]))
        break;

      case FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT:
        this.form.removeControl('maximumMoneyInvestment');
        this.form.addControl('maximumBudgetInvestment', new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]));
        break

      default:
        this.form.removeControl('maximumBudgetInvestment');
        this.form.removeControl('maximumMoneyInvestment');

    }

    this.hasBoundary = boundaryValue;
    this.form.updateValueAndValidity();

  }
}
