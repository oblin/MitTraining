import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AgePipe } from './core/age.pipe';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValueComponent } from './test/value.component';
import { LhcService } from './shared/lhc.service';
import { HttpClientModule } from '@angular/common/http';
import { PatientComponent } from './lhc/patient.component';
import { PatientDetailComponent } from './lhc/patient-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    ValueComponent,
    PatientComponent,
    AgePipe,
    PatientDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [LhcService],
  bootstrap: [AppComponent]
})
export class AppModule { }
