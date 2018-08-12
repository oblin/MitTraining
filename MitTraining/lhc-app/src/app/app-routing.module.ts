import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PatientComponent } from './lhc/patient.component';
import { PatientDetailComponent } from './lhc/patient-detail.component';
import { componentFactoryName } from '@angular/compiler';
import { ValueComponent } from './test/value.component';
import { LoginComponent } from './login';
import { AuthGuard } from './core/auth.guard';
import { HomeComponent } from './test/home.component';
import { DynamicTemplateComponent } from './test/dynamic-template.component';

const routes: Routes = [
  { path: "patient-list", component: PatientComponent },
  { path: "patient-details/:id", component: PatientDetailComponent },
  { path: "values", component: ValueComponent, canActivate: [ AuthGuard ] },
  { path: "login", component: LoginComponent },
  { path: "template/:id", component: DynamicTemplateComponent },
  { path: "", component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
