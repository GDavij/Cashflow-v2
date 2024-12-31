import { Component, OnInit, resource, signal, ViewChild } from '@angular/core';
import { FinancialBoundariesService } from '../../financial-boundaries.service';
import { CdkMenuModule } from '@angular/cdk/menu';
import { Category, CategoryListItem, CategoryTransactionsAggregate } from '../../../models/financial-boundaries/category';
import { catchError, firstValueFrom, of, retry, tap } from 'rxjs';
import { BaseChartDirective, provideCharts } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { monthFromIndex } from '../../../enums/MONTHS';
import { CacheService } from '../../../services/cache.service';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-categories',
  imports: [CdkMenuModule, CommonModule, RouterLink],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  currentYear = 2024;

  categories: CategoryListItem[] = [];

  isLoadingCurrentCategory: boolean = false;
  currentCategory: Category | null = null;
  currentcategoryTransactionsAggregate: CategoryTransactionsAggregate | null = null;

  constructor(private readonly _financialBoundariesService: FinancialBoundariesService, private readonly _cacheService: CacheService) { }

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

  async viewCategory(selectedCategory: CategoryListItem) {
    this.isLoadingCurrentCategory = true
    const categoryPromise = this._cacheService.getOrResolveTo(() => this._financialBoundariesService.getCategory(selectedCategory).pipe(
      retry(3),
      catchError(httpErr => {
        console.error({ httpErr });

        return of(null);
      })), `category-${selectedCategory.id}`)

    const categoryTransactionsPromise = this._cacheService.getOrResolveTo(() => this._financialBoundariesService.getCategoryTransactionsAggregateForYear(selectedCategory, this.currentYear).pipe(
      retry(3),
      catchError(httpError => {
        console.error({ httpError })
        return of(null);
      })), `category-${selectedCategory.id}-transactions-${this.currentYear}`);

    const [category, transactions] = await Promise.all([categoryPromise, categoryTransactionsPromise]);

    this.currentCategory = category;
    this.currentcategoryTransactionsAggregate = transactions;
    setTimeout(() => {
      this.isLoadingCurrentCategory = false;
    }, 200)
  }

  parseChart(currentCategoryTransactionsAggregate: CategoryTransactionsAggregate): ChartData<'bar'> {
    const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

    return {
      labels: months,
      datasets: [
        // { data: this.currentCategoryTransactionsAggregate!.transactionsUsageAggregate.map(agg => agg.totalDeposit), label: 'Deposit' },
        // { data: this.currentCategoryTransactionsAggregate!.transactionsUsageAggregate.map(agg => agg.totalWithdrawl), label: 'Withdrawl' }
      ]
    };
  }
}
