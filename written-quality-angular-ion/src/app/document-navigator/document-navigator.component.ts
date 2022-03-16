import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DocumentViewService } from '../document-view.service';
import { WrittenDocument } from '../document.model';

@Component({
  selector: 'app-document-navigator',
  templateUrl: './document-navigator.component.html',
  styleUrls: ['./document-navigator.component.scss'],
})
export class DocumentNavigatorComponent implements OnInit, OnDestroy {
  public documentViewSubscription: Subscription;
  public documents: WrittenDocument[] = [];

  constructor(
    public documentViewService: DocumentViewService
  ) { }

  ngOnInit() {
    this.documentViewSubscription = this.documentViewService.listenForUpdates((document: WrittenDocument) => {
      this.documents = this.documents
        .filter(d => d.id != document.id)
        .concat(document)
        .sortByDesc(d => d.createdDate);
    });
  }

  ngOnDestroy(): void {
    this.documentViewSubscription?.unsubscribe();
  }

}
