import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, FormGroup, FormBuilder, ReactiveFormsModule, Validators, FormControl } from '@angular/forms';
import { CdkMenuModule } from '@angular/cdk/menu';
import { FINANCIAL_BOUNDARIES } from '../../../../enums/FINANCIAL_BOUNDARIES';
import { CommonModule, NgIf } from '@angular/common';
import { catchError, of, retry, tap } from 'rxjs';
import { ButtonComponent } from "../../../../components/button/button.component";
import { FinancialBoundariesService } from '../../../financial-boundaries.service';
import { SaveCategoryPayload } from '../../../../models/financial-boundaries/category';

@Component({
  selector: 'app-edit',
  imports: [FormsModule, ReactiveFormsModule, CdkMenuModule, CommonModule, ButtonComponent, RouterModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditComponent implements OnInit {
  readonly financialBoundaries: FINANCIAL_BOUNDARIES[] = [FINANCIAL_BOUNDARIES.NONE, FINANCIAL_BOUNDARIES.MONEY, FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT];

  isLoadingCurrentCategory: boolean = false;
  isSavingCategory: boolean = false;
  hasBoundary: FINANCIAL_BOUNDARIES = FINANCIAL_BOUNDARIES.NONE;
  form!: FormGroup;

  constructor(
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _router: Router,
    private readonly _fb: FormBuilder,
    private readonly _financialBoundariesService: FinancialBoundariesService) { }

  get id(): string | null {
    return this._activatedRoute.snapshot.paramMap.get('id');
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

  handleSubmit() {
    if (this.form.invalid || this.isSavingCategory) {
      return;
    }

    this.isSavingCategory = true;

    const { name, maximumBudgetInvestment, maximumMoneyInvestment, active } = this.form.value;
    const payload: SaveCategoryPayload = {
      id: Number(this.id),
      name,
      maximumBudgetInvestment,
      maximumMoneyInvestment,
      active
    }

    this._financialBoundariesService.saveCategory(payload).pipe(retry(3), catchError(httpError => {
      console.error({ httpError });
      return of(null)
    })).subscribe(savedResult => {
      this.isSavingCategory = false;
      if (savedResult) {
        this._router.navigate(['/categories'])
      }
    })
  }
}
