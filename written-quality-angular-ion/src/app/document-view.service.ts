import { Injectable } from '@angular/core';
import { asyncScheduler, BehaviorSubject, Observable, Subscription } from 'rxjs';
import { filter, throttleTime } from 'rxjs/operators';
import { WrittenDocument } from './document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentViewService {

  private documentUpdated$: BehaviorSubject<WrittenDocument> = new BehaviorSubject(null);
  private documentUpdatedObservable$: Observable<WrittenDocument> = new Observable();

  private documentNavigated$: BehaviorSubject<WrittenDocument> = new BehaviorSubject(null);
  private documentNavigatedObservable$: Observable<WrittenDocument> = new Observable();

  constructor() {
    this.documentUpdatedObservable$ = this.documentUpdated$
      .pipe(
        throttleTime(100, asyncScheduler, { leading: false, trailing: true }),
        filter(document => document != null)
      )

    this.documentNavigatedObservable$ = this.documentNavigated$
      .pipe(
        filter(document => document != null)
      )
  }

  navigate(document: WrittenDocument) {
    this.documentNavigated$.next(document);
  }

  update(document: WrittenDocument) {
    this.documentUpdated$.next(document);
  }

  listenForUpdates(next: (document: WrittenDocument) => void): Subscription {
    return this.documentUpdatedObservable$.subscribe(next);
  }

  listenForNavigates(next: (document: WrittenDocument) => void): Subscription {
    return this.documentNavigatedObservable$.subscribe(next);
  }

}
