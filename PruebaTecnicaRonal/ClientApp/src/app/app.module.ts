import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import { FormsModule } from '@angular/forms'
import { ReactiveFormsModule } from '@angular/forms'
import { CommonModule } from '@angular/common';
//import { TableModule } from 'primeng/table';
//import { SliderModule } from 'primeng/slider';
//import { MultiSelectModule } from 'primeng/multiselect';
//import { ContextMenuModule } from 'primeng/contextmenu';
//import { ToastModule } from 'primeng/toast';
//import { ButtonModule } from 'primeng/button';
//import { CalendarModule } from 'primeng/calendar';
//import { DialogModule } from 'primeng/dialog';
//import { DropdownModule } from 'primeng/dropdown';
//import { InputTextModule } from 'primeng/inputtext';
//import { ProgressBarModule } from 'primeng/progressbar';
//import { TabViewModule } from 'primeng/tabview';
//import { CodeHighlighterModule } from 'primeng/codehighlighter';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,//Add if needed 
    CommonModule,
    HttpClientModule,
    BrowserModule,
    FormsModule,
    //TableModule,
    //CalendarModule,
    //SliderModule,
    //DialogModule,
    //MultiSelectModule,
    //ContextMenuModule,
    //DropdownModule,
    //ButtonModule,
    //ToastModule,
    //InputTextModule,
    //ProgressBarModule,
    //TabViewModule,
    //CodeHighlighterModule,
    RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent },
], { relativeLinkResolution: 'legacy' })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
