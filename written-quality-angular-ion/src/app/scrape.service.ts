import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WrittenEnvironment } from './written-environment.model';

@Injectable({
  providedIn: 'root'
})
export class ScrapeService {

  constructor(
    private _httpClient: HttpClient,
    private _environment: WrittenEnvironment
  ) { }

  public async scrapeUrlToMarkdown(url: string): Promise<string> {
    if (url && !url?.includes('://')) {
      url = 'https://' + url;
    }

    let params = new HttpParams().append("url", url);

    try {
      let metadata = await this._httpClient.get(this._environment.baseUrl + "/Scrape/ScrapeUrl", { params }).toPromise();

      return ((metadata as any).content as string);

    } catch (e) {
      window.alert("There was an error scraping.  Please check the URL or try a different one");
    }

  }

}
