import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WrittenDocument, WrittenDocumentAnalysis, WrittenDocumentQuality } from './document.model';
import { WrittenEnvironment } from './written-environment.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(
    private _httpClient: HttpClient,
    private _environment: WrittenEnvironment
  ) { }

  async analyzeDocument(document: WrittenDocument): Promise<WrittenDocumentAnalysis> {

    try {
      let analysis = await this._httpClient.post<WrittenDocumentAnalysis>(this._environment.baseUrl + "/Document/Analyze", document).toPromise();

      return new WrittenDocumentAnalysis(analysis);

    } catch (e) {
      window.alert("There was an error analyzing the document.  Make sure you provided at least 20 words in one or more full sentences, with puncuation.");
    }

  }
}
