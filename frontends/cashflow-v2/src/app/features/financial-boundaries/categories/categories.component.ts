import { Component, OnInit } from '@angular/core';
import { FinancialBoundariesService } from '../../financial-boundaries.service';
import { CdkMenuModule } from '@angular/cdk/menu';
import { Category, CategoryListItem, CategoryTransactionsAggregate } from '../../../models/financial-boundaries/category';
import { catchError, of, retry } from 'rxjs';
import { CommonModule} from '@angular/common';
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
    })
  }


  editCurrentCategory() {
    this._router.navigate(['/categories', 'edit', this.currentCategory!.id]);
  }

}
