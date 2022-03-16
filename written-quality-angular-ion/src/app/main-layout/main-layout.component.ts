import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuController } from '@ionic/angular';
import { DocumentEditorComponent } from '../document-editor/document-editor.component';
import { DocumentViewService } from '../document-view.service';
import { WrittenDocument } from '../document.model';
import { DocumentService } from '../document.service';
import { ScrapeService } from '../scrape.service';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent implements OnInit {

  @ViewChild("editor") editor: DocumentEditorComponent;

  public document: WrittenDocument = null;
  public urlToScrape: string = "";
  public analyzing: boolean;
  scraping: boolean;

  constructor(
    public documentViewService: DocumentViewService,
    public documentService: DocumentService,
    public scrapeService: ScrapeService,
    public menuController: MenuController
  ) { }

  ngOnInit() { }

  async analyze() {
    let document = this.editor.getDocument();
    this.analyzing = true;

    try {
      let analysis = await this.documentService.analyzeDocument(document);
      // avoid mutating existing objects, so create new one
      this.document = new WrittenDocument(document, analysis);
      this.editor.setDocument(this.document);
      this.menuController.toggle("end");

    }
    finally {
      this.analyzing = false;
    }

  }

  async scrapeUrl() {
    this.scraping = true;

    try {
      let markdown = await this.scrapeService.scrapeUrlToMarkdown(this.urlToScrape);
      this.editor.setMarkdown(markdown);
    }
    finally {
      this.scraping = false;
    }
  }

  async scrapeAndAnalyze(e) {
    e.preventDefault();
    e.stopPropagation();
    this.urlToScrape = e.target.href as string;
    await this.scrapeUrl();
    await this.analyze();
  }
}
