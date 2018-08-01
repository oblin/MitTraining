import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { ActivatedRoute } from '@angular/router';
import { RegFile } from '../shared/lhc.models';
import { NgForm } from '@angular/forms';
import { BaseComponent } from '../core/base.component';
import { Globals } from '../core/globals.service';
import { CodeService, CodeFile } from '../core/code.service';
import { AlertType, AlertBaseComponent } from '../core/alert-base.component';
import { Subscription } from 'rxjs';
import { ConfirmComponent } from '../core/confirm.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'patient-detail',
  templateUrl: './patient-detail.component.html',
  styles: []
})
export class PatientDetailComponent extends BaseComponent implements OnInit {

  constructor(private data: LhcService, private route: ActivatedRoute,
    private modalService: BsModalService, globals: Globals, codeService: CodeService) {
    super(globals, codeService);
  }

  id: string;
  model: RegFile;
  ngOnInit() {
    // 初始化設定
    let codes = new Array<CodeFile>();
    codes.push(CodeFile.initType('Sex', null, null));
    this.codeService.setCodeSelections(codes);

    this.route.params.subscribe(p => {
      this.id = p["id"];
      if (this.id === "new")
        this.model = new RegFile();
      else
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
    if (this.id === "new")
      this.busy = this.data.insertPatientDetail(this.model)
        .subscribe(
          data => {
            this.model.RegNo = data.RegNo;
            super.showSaveSuccess();
            this.cancel();
          },
          error => super.showError(AlertType.Danger, error)
        );
    else
      this.busy = this.data.updatePatientDetail(this.model)
        .subscribe(
          data => {
            super.showSaveSuccess();
            this.cancel();
          },
          error => super.showError(AlertType.Danger, error)
        );
  }

  bsModalRef: BsModalRef;
  confirmDelete() {
    const initialState = {
      title: "確認刪除",
      description: "請確認是否刪除",
      action: this.deleting, // callback method
      param: this
    };
    this.bsModalRef = this.modalService.show(ConfirmComponent, { initialState });
  }

  cancel() {
    this.globals.routeObject = { alertType: this.alertType, alertMessage: this.alertMessage };
    window.history.back();
  }

  deleting(param: PatientDetailComponent) {
    param.busy = param.data.deletePatientDetail(param.id)
      .subscribe(
        success => {
          param.globals.routeObject = { alertType: AlertType.Success, alertMessage: "刪除病歷號：" + param.id + "成功" };
          window.history.back();

        },
        error => param.showError(AlertType.Danger, error)
      );
  }
}
