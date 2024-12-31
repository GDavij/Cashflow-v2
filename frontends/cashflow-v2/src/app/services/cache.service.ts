import { Injectable } from '@angular/core';
import { firstValueFrom, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  readonly cacheLocalStorageKey = 'LOCAL_CACHE';
  cacheMap: Map<string, any> = new Map();

  constructor() {
    this.syncWithLocalStorageValues();
  }

  async getOrResolveTo<T>(resolve: () => Observable<T>, key: string): Promise<T> {
    if (this.cacheMap.has(key)) {
      return this.cacheMap.get(key);
    }

    const result = await firstValueFrom(resolve())
    this.cacheMap.set(key, result);

    return result;
  }

  invalidate(key: string): boolean {
    if (this.cacheMap.has(key)) {
      return this.cacheMap.delete(key);
    }

    return false;
  }

  clearAll(key: string): boolean {
    this.cacheMap.clear();
    return true;
  }

  persistToLocalStorage() {
    console.log({ ...this.cacheMap })
    localStorage.setItem(this.cacheLocalStorageKey, JSON.stringify({ ...this.cacheMap }));
  }

  private syncWithLocalStorageValues() {
    const cacheValue = localStorage.getItem(this.cacheLocalStorageKey);
    if (!cacheValue) {
      return;
    }

    const values = JSON.parse(cacheValue) as {};
    this.cacheMap = new Map<any, any>(Object.entries(values));
  }
}
