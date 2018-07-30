import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { ActivatedRoute } from '@angular/router';
import { RegFile } from '../shared/lhc.models';
import { NgForm } from '@angular/forms';
import { BaseComponent } from '../core/base.component';
import { Globals } from '../core/globals.service';
import { CodeService } from '../core/code.service';
import { AlertType, AlertBaseComponent } from '../core/alert-base.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'patient-detail',
  templateUrl: './patient-detail.component.html',
  styles: []
})
export class PatientDetailComponent extends BaseComponent implements OnInit {

  constructor(private data: LhcService, private route: ActivatedRoute, globals: Globals, codeService: CodeService) {
    super(globals, codeService);
  }

  id: string;
  model: RegFile;
  ngOnInit() {
    this.route.params.subscribe(p => {
      this.id = p["id"];
      this.data.getPatient(this.id).subscribe(data => this.model = data);
    })
  }

  busy: Subscription;
  submitForm(form: NgForm) {
    if (form && !form.valid) {
      this.alertType = AlertType.Danger;
      this.alertMessage = "欄位檢查錯誤";
      return;
    }
    if (this.model.RegNo)
      this.busy = this.data.updatePatientDetail(this.model)
        .subscribe(
          data => {
            super.showSaveSuccess();
            this.cancel();
          },
        error => super.showError(AlertType.Danger, error)
        );
    else
      this.busy = this.data.insertPatientDetail(this.model)
        .subscribe(
          data => {
            this.model.RegNo = data.RegNo;
            super.showSaveSuccess();
            this.cancel();
          },
        error => super.showError(AlertType.Danger, error)
        );
  }

  cancel() {
    window.history.back();
  }
}
