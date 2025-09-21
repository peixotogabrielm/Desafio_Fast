import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AtasListComponent } from './components/atas-list/atas-list.component';
import { WorkshopDetailsComponent } from './components/workshop-details/workshop-details.component';

const routes: Routes = [
  { path: 'atas', component: AtasListComponent },
  { path: 'workshop/:id', component: WorkshopDetailsComponent },
  { path: '', redirectTo: '/atas', pathMatch: 'full' },
  { path: '**', redirectTo: '/atas' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
