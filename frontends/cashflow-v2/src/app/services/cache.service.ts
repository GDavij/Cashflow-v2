import { Injectable } from '@angular/core';
import { firstValueFrom, Observable, of, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  readonly cacheLocalStorageKey = 'LOCAL_CACHE';
  cacheMap: Map<string, any> = new Map();

  constructor() { }

  getOrResolveTo<T>(resolve: () => Observable<T>, key: string): { subject: Subject<T>, cancelSubscription: () => void } {
    const subject = new Subject<T>;

    if (this.cacheMap.has(key)) {
      console.log('Cache hit for key:', key)
      setTimeout(() => subject.next(this.cacheMap.get(key)), 100)
      return { subject, cancelSubscription: () => { } }
    }

    const subscription = resolve().subscribe(result => {
      this.cacheMap.set(key, result);
      console.log("Cache miss for key:", key)
      subject.next(result);
    });

    return { subject, cancelSubscription: () => subscription.unsubscribe() };
  }

  invalidate(key: string): boolean {
    if (this.cacheMap.has(key)) {
      return this.cacheMap.delete(key);
    }

    return false;
  }
}
