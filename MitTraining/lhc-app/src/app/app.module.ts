import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Compiler, COMPILER_OPTIONS, CompilerFactory, enableProdMode  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

export function createJitCompiler(compilerFactory: CompilerFactory) {
  return compilerFactory.createCompiler();
}

enableProdMode();

import { NgBusyModule } from 'ng-busy';

// ngx components
import { AlertModule } from 'ngx-bootstrap/alert';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import  {  zhCnLocale  }  from  'ngx-bootstrap/locale';
defineLocale('zh-cn',  zhCnLocale); 
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { AccordionModule } from 'ngx-bootstrap/accordion';

// Angular Core Components
import { AgePipe } from './core/age.pipe';
import { FormGroupComponent } from './core/form-group.component';
import { ConfirmComponent } from './core/confirm.component';
import { AlertComponent } from './core/alert.component';
import { Globals } from './core/globals.service';
import { CodeService } from './core/code.service';
import { CodeOptionsComponent } from './core/code-options.component';
import { JwtInterceptor } from './core/jwt.interceptor';
import { ErrorInterceptor } from './core/error.interceptor';
import { AuthGuard } from './core/auth.guard';

// app components
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValueComponent } from './test/value.component';
import { LhcService } from './shared/lhc.service';
import { PatientComponent } from './lhc/patient.component';
import { PatientDetailComponent } from './lhc/patient-detail.component';
import { LoginComponent } from './login/login.component';
import { AuthenticationService } from './shared/authentication.service';
import { HomeComponent } from './test/home.component';
import { DynamicTemplateComponent } from './test/dynamic-template.component'
import { CodePipe } from './core/code.pipe';
import { FaIconDirective } from './core/faicon.directive';
import { ValidateSpanComponent } from './core/validate-span.component';
import { JitCompilerFactory } from '@angular/platform-browser-dynamic';

@NgModule({
  declarations: [
    AppComponent,
    ValueComponent,
    PatientComponent,
    AgePipe,
    PatientDetailComponent,
    AlertComponent,
    CodeOptionsComponent,
    LoginComponent,
    HomeComponent,
    CodePipe, FaIconDirective, ValidateSpanComponent,
    DynamicTemplateComponent,

    FormGroupComponent, ConfirmComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule, ReactiveFormsModule,

    BrowserAnimationsModule,
    NgBusyModule,

    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TooltipModule.forRoot(),
    AlertModule.forRoot(),
    AccordionModule.forRoot()
  ],
  providers: [LhcService, Globals, CodeService, AuthGuard, AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: COMPILER_OPTIONS, useValue: {}, multi: true },
    { provide: CompilerFactory, useClass: JitCompilerFactory, deps: [COMPILER_OPTIONS] },
    { provide: Compiler, useFactory: createJitCompiler, deps: [CompilerFactory] }
  ],
  entryComponents: [ConfirmComponent, DynamicTemplateComponent],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private localeService: BsLocaleService) {
    localeService.use("zh-cn");
  }
}
