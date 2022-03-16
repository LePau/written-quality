import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DocumentViewService } from '../document-view.service';
import { WrittenDocument, WrittenDocumentQuality } from '../document.model';

@Component({
  selector: 'app-document-analysis',
  templateUrl: './document-analysis.component.html',
  styleUrls: ['./document-analysis.component.scss'],
})
export class DocumentAnalysisComponent implements OnInit, OnDestroy {

  private documentViewSubscription: Subscription;
  private documentNavigateSubscription: Subscription;
  public document: WrittenDocument = null;
  public overallQuality: WrittenDocumentQuality = null;
  public paragraphs: string[] = [];

  constructor(
    public documentViewService: DocumentViewService
  ) { }

  ngOnInit() {
    this.documentViewSubscription = this.documentViewService.listenForUpdates((document: WrittenDocument) => {
      this.selectDocument(document);
    });

    this.documentNavigateSubscription = this.documentViewService.listenForNavigates((document: WrittenDocument) => {
      this.selectDocument(document);
    });
  }

  ngOnDestroy(): void {
    this.documentViewSubscription?.unsubscribe();
    this.documentNavigateSubscription.unsubscribe();
  }

  selectDocument (document: WrittenDocument)
  {
    this.document = document;
    this.overallQuality = this.document?.analysis?.overall;
    this.paragraphs = this.document?.analysis?.summary?.split("\n") || [];
  }

}
