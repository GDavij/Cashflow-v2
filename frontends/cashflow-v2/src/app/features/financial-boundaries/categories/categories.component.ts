import { Component, OnInit } from '@angular/core';
import { FinancialBoundariesService } from '../../financial-boundaries.service';
import { CdkMenuModule } from '@angular/cdk/menu';
import { Category, CategoryListItem, CategoryTransactionsAggregate } from '../../../models/financial-boundaries/category';
import { catchError, of, retry } from 'rxjs';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { CacheService } from '../../../services/cache.service';
import { Router, RouterLink } from '@angular/router';
import { ButtonComponent } from "../../../components/button/button.component";
import { DateHelper } from '../../../helpers/date.helper';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { matArrowBackIosRound, matArrowForwardIosRound } from '@ng-icons/material-icons/round';

@Component({
  selector: 'app-categories',
  imports: [CdkMenuModule, CommonModule, RouterLink, ButtonComponent, NgIcon],
  viewProviders: [provideIcons({ matArrowBackIosRound, matArrowForwardIosRound })],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  currentDate = new DateHelper();

  categories: CategoryListItem[] = [];

  isLoadingCurrentCategory: boolean = false;
  isLoadingTransactions: boolean = false;
  currentCategory: Category | null = null;
  private _currentcategoryTransactionsAggregate: CategoryTransactionsAggregate | null = null;

  get totalTransactionsMadeInMonth() {
    return this._currentcategoryTransactionsAggregate!.transactionsUsageAggregate.find(t => t.month == this.currentDate.getMonth())!.totalTransactions;
  }

  get totalDepositInMonth() {
    return this._currentcategoryTransactionsAggregate!.transactionsUsageAggregate.find(t => t.month == this.currentDate.getMonth())!.totalDeposit;
  }

  get totalWithdrawlInMonth() {
    return this._currentcategoryTransactionsAggregate!.transactionsUsageAggregate.find(t => t.month == this.currentDate.getMonth())!.totalWithdrawl;
  }

  get alerts(): string[] {
    const alerts: string[] = [];
    for (let transaction of this._currentcategoryTransactionsAggregate!.transactionsUsageAggregate) {
      if (transaction.hasReachedLimit) {
        this.alerts.push(`You cross the boundary for this category in ${DateHelper.getMonthName(transaction.month)} of ${this._currentcategoryTransactionsAggregate!.year}`)
      }
    }

    return alerts;
  }

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
        this.viewCategory(this.categories[0]);
      }
    })
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
      this.viewTransactions(this.currentCategory!);
    })

  }

  viewTransactions(category: Category) {
    this.isLoadingTransactions = true;

    const { subject } = this._cacheService.getOrResolveTo(() => this._financialBoundariesService.getCategoryTransactionsAggregateForYear(category, this.currentDate.getYear()).pipe(retry(3), catchError(err => {
      console.error({ err });
      this.isLoadingTransactions = false;
      return of(null)
    })), `category-${category.id}-transactions-${this.currentDate.getYear()}`);

    subject.subscribe(result => {
      this._currentcategoryTransactionsAggregate = result;

      setTimeout(() => {

        this.isLoadingTransactions = false;
      }, 100)
    })
  }

  editCurrentCategory() {
    this._router.navigate(['/categories', 'edit', this.currentCategory!.id]);
  }

  nextMonth() {
    this.currentDate.goToNextMonth();
    this.viewTransactions(this.currentCategory!);
  }

  previousMonth() {
    this.currentDate.goToPreviousMonth();
    this.viewTransactions(this.currentCategory!);
  }

  nextYear() {
    this.currentDate.goToNextYear();
    this.viewTransactions(this.currentCategory!);
  }

  previousYear() {
    this.currentDate.goToPreviousYear();
    this.viewTransactions(this.currentCategory!);
  }

  asPercentage(value: number) {
    return value * 100;
  }

  // transformTransactionsAggregate(currentDate: DateHelper, currentCategoryTransactionsAggregate: TransactionAggregate): ChartData<'bar'> {
  //   console.log({ currentCategoryTransactionsAggregate })
  //   return {
  //     labels: [currentDate.getMonthName()],
  //     datasets: [
  //       { data: [currentCategoryTransactionsAggregate.totalDeposit], label: 'Deposit' },
  //       { data: [currentCategoryTransactionsAggregate.totalWithdrawl], label: 'Withdrawl' }
  //     ]
  //   };
  // }
}
