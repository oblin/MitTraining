import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LhcService } from '../shared/lhc.service';
import { RegFile } from '../shared/lhc.model';
import { AlertBaseComponent, AlertType } from '../core/alert-base.component';
import { Globals } from '../core/globals.service';
import { PagedList } from '../shared/shared.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'patient',
  templateUrl: './patient.component.html',
  styles: []
})
export class PatientComponent extends AlertBaseComponent implements OnInit {

  constructor(private data: LhcService, private globals: Globals, private router: Router) { super(); }

  busy: Subscription;
  patients: Array<RegFile>;
  ngOnInit() {
    if (this.globals.routeObject) {
      this.alertType = this.globals.routeObject.alertType;
      this.alertMessage = this.globals.routeObject.alertMessage;
      this.globals.routeObject = {};
    }
    //this.busy = this.data.getAllPatients().subscribe(data => this.patients = data);
    this.setPagedList(1);
  }

  add() {
    this.router.navigate(["/patient-details", "new"]);
  }

  pageSizeOptions = [10, 20, 50];
  pageSize: number = 20;
  pagedList: PagedList<RegFile> = new PagedList<RegFile>();
  private setPagedList(page: number) {
    let paged = this.pagedList;
    let pageCount = this.pagedList.pageCount;

    page = page || ((paged.currentPage - 1) * pageCount);

    this.busy = this.data.getPage(page, this.pageSize)
      .subscribe(
        result => {
          this.pagedList.list = result.list;
          this.pagedList.pageCount = result.pageCount;
          this.pagedList.count = Math.ceil(result.pageCount / this.pageSize);
        },
        error => super.showError(AlertType.Warning, error)
      );
  }

  prevPage() {
    if (this.pagedList.currentPage <= 1) {
      return
    }
    this.pagedList.currentPage--;
    this.setPagedList(this.pagedList.currentPage);
  }
  nextPage() {
    if (this.pagedList.currentPage == this.pagedList.count)
      return
    this.pagedList.currentPage++;
    this.setPagedList(this.pagedList.currentPage);
  }
  goPage() {
    if (this.pagedList.currentPage > this.pagedList.count) {
      this.pagedList.currentPage = this.pagedList.count;
    }
    if (this.pagedList.currentPage < 1) {
      this.pagedList.currentPage = 1;
    }
    this.setPagedList(this.pagedList.currentPage);
  }
}
