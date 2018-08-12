import { Component, OnInit, AfterViewInit, Compiler, Injector, NgModuleRef, NgModule, ViewChild, ViewContainerRef, OnDestroy } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { NgModel, FormsModule } from '@angular/forms';
import { CompileReflector } from '@angular/compiler';
import { RegFile } from '../shared/lhc.model';
import { AppModule } from '../app.module';
import { BrowserModule } from '@angular/platform-browser';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { ComponentRef } from '@angular/core/src/render3';
import { BsDatepickerModule, BsDaterangepickerConfig } from 'ngx-bootstrap';

declare var module: {
  id: string;
}

@Component({
  selector: 'dynamic-template',
  template: '<ng-container #dynamicTemplate></ng-container>',
  styles: []
})
export class DynamicTemplateComponent implements AfterViewInit, OnInit, OnDestroy {
  @ViewChild('dynamicTemplate', { read: ViewContainerRef }) dynamicTemplate: ViewContainerRef;

  constructor(private compiler: Compiler, private injector: Injector, private ngModuleRef: NgModuleRef<any>,
    private route: ActivatedRoute, private data: LhcService) { }

  id: number;
  ngOnInit() {
    console.log(module.id);
    this.route.params.subscribe(p => {
      this.id = p["id"];
    })
  }

  componentsReferences= [];
  /**
   *會在 Init 之後呼叫
   * */
  ngAfterViewInit(): void {
    this.data.getTemplate(this.id).subscribe(data => {
      let model: any = {};
      if (this.id === 2) {
        model.username = "";
        model.password = "";
      }
      const tmpComponent = Component({ moduleId: module.id, template: data })
        (class implements OnInit {
          ngOnInit(): void {
            console.log("in ngOnInit");
          }

          private data: LhcService;
          patients: Array<RegFile>;
          model: any

          getPatient() {
            this.data.getAllPatients().subscribe(data => this.patients = data);
          }

          callback: Function;
          clickPatient(patient: RegFile) {
            this.callback(patient).subscribe(m => console.log("easy way from patient: " + m));
          }

          submitForm() {
            console.log('submit');
          }

          cancel() {
            window.history.back();
          }
        });

      const tmpModule = NgModule({
        declarations: [tmpComponent],
        imports: [BrowserModule, FormsModule]
      })(class { });

      this.compiler.compileModuleAndAllComponentsAsync(tmpModule)
        .then((factories) => {
          const factory = factories.componentFactories[0];
          const cmpRef = factory.create(this.injector, [], null, this.ngModuleRef);
          cmpRef.instance.name = 'dynamic';
          cmpRef.instance.data = this.data;
          cmpRef.instance.model = model;
          cmpRef.instance.callback = this.interactive;
          //this.componentsReferences.push(cmpRef);
          this.dynamicTemplate.insert(cmpRef.hostView);
        })
    });
  }
 
  interactive(patient: RegFile): Observable<string> {
    console.log("dynamic compoent callback, show reg_no: " + patient.RegNo);
   
    return new Observable((observe) => {
      observe.next(patient.Name);
      observe.complete();
    })
  }

  ngOnDestroy(): void {
    for (var i = 0; i < this.componentsReferences.length; i++) {
      this.componentsReferences[i].destroy();
    }
    console.log("destroy");
  }
}
