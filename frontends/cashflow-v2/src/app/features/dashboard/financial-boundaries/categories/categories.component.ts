import { Component, OnInit } from '@angular/core';
import { FinancialBoundariesService } from '../../../financial-boundaries.service';
import { CdkMenuModule } from '@angular/cdk/menu';
import { Category, CategoryListItem, CategoryTransactionsAggregate } from '../../../../models/financial-boundaries/category';
import { catchError, of, retry } from 'rxjs';
import { CommonModule, CurrencyPipe} from '@angular/common';
import { CacheService } from '../../../../services/cache.service';
import { Router, RouterLink } from '@angular/router';
import { ButtonComponent } from "../../../../components/button/button.component";
import { DateHelper } from '../../../../helpers/date.helper';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { matArrowBackIosRound, matArrowForwardIosRound } from '@ng-icons/material-icons/round';
import { FormFieldComponent } from "../../../../components/form-field/form-field.component";
import { SelectComponent } from "../../../../components/select/select.component";
import { SelectContainerComponent } from "../../../../components/select/select-container/select-container.component";
import { SelectOptionComponent } from '../../../../components/select/select-option/select-option.component';
import { Option } from '../../../../components/select/select.models';

@Component({
  selector: 'app-categories',
  imports: [CdkMenuModule, CommonModule, RouterLink, ButtonComponent, FormFieldComponent, SelectComponent, SelectContainerComponent, SelectOptionComponent],
  viewProviders: [provideIcons({ matArrowBackIosRound, matArrowForwardIosRound })],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  currentDate = new DateHelper();

  categoriesOptions: Option<CategoryListItem>[] = [];
  categories: CategoryListItem[] = [];

  isLoadingCurrentCategory: boolean = false;
  isLoadingTransactions: boolean = false;
  currentCategory: Category | null = null;

  constructor(private readonly _financialBoundariesService: FinancialBoundariesService, private readonly _cacheService: CacheService, private readonly _router: Router) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this._financialBoundariesService.listCategories().pipe(
      retry(3),
      catchError(httpErr => {
        console.error({ httpErr });
        return of([]);
      })
    ).subscribe(categories => {
      this.categories = categories;

      if (this.categories.length > 0) {
        this.categoriesOptions = this.categories.map(category => ({ label: category.name, value: category }));
        this.viewCategory(this.categories[0]);
      }
    })
  }

  view(data: any) {
    console.log({data})
  }

  viewCategory(selectCatgory: CategoryListItem) {
    this.isLoadingCurrentCategory = true;
    const { subject } = this._cacheService.getOrResolveTo(() => this._financialBoundariesService.getCategory(selectCatgory).pipe(retry(3), catchError(err => {
      console.error([err]);
      this.isLoadingCurrentCategory = false;
      return of(null);
    })), `category-${selectCatgory.id}`);

    subject.subscribe(result => {
      this.currentCategory = result;
      this.isLoadingCurrentCategory = false;
    })
  }


  editCurrentCategory() {
    this._router.navigate(['/categories', 'edit', this.currentCategory!.id]);
  }

  get hasContraint() {
    return this.currentCategory?.maximumBudgetInvestment || this.currentCategory?.maximumMoneyInvestment;
  }

  get constraintTitle() {
    if (this.currentCategory?.maximumBudgetInvestment) {
      return "Maximum Budget for Investment";
    }

    if (this.currentCategory?.maximumMoneyInvestment) {
      return "Maximum Money for Investment";
    }

    return "No Constraints for Investment"
  }

  get constraint() {
    if (this.currentCategory?.maximumBudgetInvestment) {
      return `${this.currentCategory.maximumBudgetInvestment * 100}%`;
    }

    if (this.currentCategory?.maximumMoneyInvestment) {
      return new CurrencyPipe("pt-br", "BRL").transform(this.currentCategory.maximumMoneyInvestment, 'BRL', 'symbol', '1.2-2');
    }

    return 0;
  }
}
