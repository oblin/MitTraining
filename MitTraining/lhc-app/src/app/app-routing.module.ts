import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PatientComponent } from './lhc/patient.component';
import { PatientDetailComponent } from './lhc/patient-detail.component';

const routes: Routes = [
  {
    path: "patient-list", component: PatientComponent
  },
  {
    path: "patient-details/:id", component: PatientDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
