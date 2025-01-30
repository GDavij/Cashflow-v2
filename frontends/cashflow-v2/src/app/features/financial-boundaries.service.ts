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

  public getCategoryById(id: string): Observable<Category> {
    return super.get<Category>(id);
  }

  public saveCategory(category: SaveCategoryPayload): Observable<EntitySavedResponse> {
    if (category.id) {
      return super.put(category.id.toString(), category);
    }

    return super.post('', category);
  }

  public deleteCategory(category: Category): Observable<void> {
    return super.delete(category.id.toString());
  }
}
