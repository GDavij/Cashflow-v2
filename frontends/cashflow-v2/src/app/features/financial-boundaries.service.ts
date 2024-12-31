import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../abstractions/baseHttp.service';
import { HttpClient } from '@angular/common/http';
import { Category, CategoryListItem, CategoryTransactionsAggregate } from '../models/financial-boundaries/category';

@Injectable({
  providedIn: 'root'
})
export class FinancialBoundariesService extends BaseHttpService {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'categories')
  }

  public listCategories(): Observable<CategoryListItem[]> {
    return super.get<CategoryListItem[]>('');
  }

  public getCategory(category: CategoryListItem): Observable<Category> {
    return super.get<Category>(category.id.toString());
  }

  public getCategoryTransactionsAggregateForYear(category: CategoryListItem, year: number): Observable<CategoryTransactionsAggregate> {
    return super.get<any>(`${category.id}/transactions/aggregate`, {
      year
    });
  }
}
