import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { BaseHttpService } from '../abstractions/baseHttp.service';
import { HttpClient } from '@angular/common/http';
import { Category, CategoryListItem, CategoryTransactionsAggregate, SaveCategoryPayload } from '../models/financial-boundaries/category';
import { EntitySavedResponse } from '../models/common';

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

  public saveCategory(category: SaveCategoryPayload): Observable<EntitySavedResponse> {
    if (category.id) {
      return super.post(category.id.toString(), category);
    }

    return super.post('', category);
  }
}