import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';


import {AppComponent} from './app.component';
import {DocumentsComponent} from './documents/documents.component';
import {BackendService} from "./services/backend.service";

@NgModule({
  declarations: [
    AppComponent,
    DocumentsComponent
  ],
  imports: [
    BrowserModule,
    NgSelectModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: DocumentsComponent, pathMatch: 'full'}
    ]),
    ReactiveFormsModule
  ],
  providers: [BackendService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
