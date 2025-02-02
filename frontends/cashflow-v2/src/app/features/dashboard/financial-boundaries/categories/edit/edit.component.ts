import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, FormGroup, FormBuilder, ReactiveFormsModule, Validators, FormControl } from '@angular/forms';
import { CdkMenuModule } from '@angular/cdk/menu';
import { FINANCIAL_BOUNDARIES } from '../../../../../enums/FINANCIAL_BOUNDARIES';
import { CommonModule, NgIf } from '@angular/common';
import { catchError, of, retry, tap } from 'rxjs';
import { ButtonComponent } from "../../../../../components/button/button.component";
import { FinancialBoundariesService } from '../../../../../services/financial-boundaries.service';
import { Category, SaveCategoryPayload } from '../../../../../models/financial-boundaries/category';
import { CacheService } from '../../../../../services/cache.service';
import { Dialog } from '@angular/cdk/dialog';
import { DeleteComponent } from '../../../../../components/dialogs/delete/delete.component';
import { DividerComponent } from '../../../../../components/divider/divider.component';

@Component({
  selector: 'app-edit',
  imports: [FormsModule, ReactiveFormsModule, CdkMenuModule, CommonModule, ButtonComponent, RouterModule, DividerComponent],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditComponent implements OnInit {
  readonly financialBoundaries: FINANCIAL_BOUNDARIES[] = [FINANCIAL_BOUNDARIES.NONE, FINANCIAL_BOUNDARIES.MONEY, FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT];
  fetchedCategory: Category | null = null;
  
  isLoadingCurrentCategory: boolean = false;
  isSavingCategory: boolean = false;
  isDeletingCategory: boolean = false;

  hasBoundary: FINANCIAL_BOUNDARIES = FINANCIAL_BOUNDARIES.NONE;
  form!: FormGroup;

  constructor(
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _router: Router,
    private readonly _fb: FormBuilder,
    private readonly _financialBoundariesService: FinancialBoundariesService,
    private readonly _cacheService: CacheService,
    private readonly _dialog: Dialog) { }

  get id(): string | null {
    return this._activatedRoute.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.createForm();

    if (this.id) {
      this.loadCategory();
    }
  }

  createForm() {
    this.form = this._fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(60)]],
    })

    this.changeBoundaryTo(FINANCIAL_BOUNDARIES.MONEY)
  }

  resetForm() {
    if (this.id) {
      this.loadExistingCategoryToForm(this.fetchedCategory!)
      return
    }

    this.createForm()
    return
  }

  private loadExistingCategoryToForm(category: Category) {
    this.form.patchValue({
      name: category.name,
    })
    this.form.addControl('active', new FormControl<boolean>(category.active, [Validators.required]));

    if (category.maximumBudgetInvestment) {
      this.changeBoundaryTo(FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT);
      this.form.patchValue({
        maximumBudgetInvestment: this.asPercentage(category.maximumBudgetInvestment)
      });
    } else if (category.maximumMoneyInvestment) {
      this.changeBoundaryTo(FINANCIAL_BOUNDARIES.MONEY);
      this.form.patchValue({
        maximumMoneyInvestment: category.maximumMoneyInvestment
      })
    } else {
      this.changeBoundaryTo(FINANCIAL_BOUNDARIES.NONE);
    }
  }

  changeBoundaryTo(boundaryValue: FINANCIAL_BOUNDARIES) {
    switch (boundaryValue) {
      case FINANCIAL_BOUNDARIES.MONEY:
        this.form.removeControl('maximumBudgetInvestment');
        this.form.addControl('maximumMoneyInvestment', new FormControl<number | null>(null, [Validators.required, Validators.min(0)]))
        break;

      case FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT:
        this.form.removeControl('maximumMoneyInvestment');
        this.form.addControl('maximumBudgetInvestment', new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(100)]));
        break

      default:
        this.form.removeControl('maximumBudgetInvestment');
        this.form.removeControl('maximumMoneyInvestment');

    }

    this.hasBoundary = boundaryValue;
    this.form.updateValueAndValidity();
  }

  activate() {
    this.form.patchValue({
      active: true
    })
  }

  deactivate() {
    this.form.patchValue({
      active: false
    })
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
      maximumBudgetInvestment: this.asPercentualOrUndefined(maximumBudgetInvestment),
      maximumMoneyInvestment,
      active
    }

    this._financialBoundariesService.saveCategory(payload).pipe(retry(3), catchError(httpError => {
      console.error({ httpError });
      return of(null)
    })).subscribe(savedResult => {
      this.isSavingCategory = false;
      this._cacheService.invalidate(`category-${this.id}`)
      if (savedResult) {
        this._router.navigate(['/categories'])
      }
    })
  }

  loadCategory() {
    this._cacheService.getOrResolveTo(() => this._financialBoundariesService.getCategoryById(this.id!), `category-${this.id}`).subject.subscribe(category => {
      this.fetchedCategory = category;
      this.loadExistingCategoryToForm(this.fetchedCategory);
    })
  }

  asPercentage(value: number) {
    return value * 100;
  }


  openDeleteDialog() {
    const dialogRef = this._dialog.open<boolean>(DeleteComponent, {
      data: `Are you sure about deleting category ${this.form.get('name')?.value}`
    });

    dialogRef.closed.subscribe((shouldDelete) => {
      if (shouldDelete) {
        this.isDeletingCategory = true;
        this.form.disable();
        this._financialBoundariesService.deleteCategory(this.fetchedCategory!).pipe(retry(3), catchError(httpError => {
          console.error({ httpError });
          return of(null)
        })).subscribe(() => {
          this._router.navigate(['/categories']);
        });
      }

      return;      
    })
  }

  private asPercentualOrUndefined(value: number | undefined) {
    if (!value) {
      return undefined;
    }

    return value / 100
  }


}
