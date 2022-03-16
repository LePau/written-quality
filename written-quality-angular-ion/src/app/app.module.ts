import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { DocumentEditorComponent } from './document-editor/document-editor.component';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { DocumentAnalysisComponent } from './document-analysis/document-analysis.component';
import { DocumentNavigatorComponent } from './document-navigator/document-navigator.component';

import './extensions/array.extensions';
import { WrittenDocumentQualityComponent } from './written-document-quality/written-document-quality.component';
import { FormsModule } from '@angular/forms';
import { WrittenEnvironment } from './written-environment.model';
import { environment } from '../environments/environment';
import { AboutComponent } from './about/about.component';

@NgModule({
  declarations: [
    AppComponent,
    DocumentEditorComponent,
    DocumentAnalysisComponent,
    DocumentNavigatorComponent,
    WrittenDocumentQualityComponent,
    MainLayoutComponent,
    AboutComponent
  ],
  entryComponents: []
  ,
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    {provide: WrittenEnvironment, useValue: environment}
  ],
  bootstrap: [
    AppComponent
  ],
})
export class AppModule { }
