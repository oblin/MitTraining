import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { RegFile } from '../shared/lhc.models';

@Component({
  selector: 'patient',
  templateUrl: './patient.component.html',
  styles: []
})
export class PatientComponent implements OnInit {

  constructor(private data: LhcService) { }

  patients: Array<RegFile>;
  ngOnInit() {
    this.data.getAllPatients().subscribe(data => this.patients = data);
  }
}
