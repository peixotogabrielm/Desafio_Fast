import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AtasListComponent } from './components/atas-list/atas-list.component';
import { WorkshopDetailsComponent } from './components/workshop-details/workshop-details.component';

@NgModule({
  declarations: [
    AppComponent,
    AtasListComponent,
    WorkshopDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
