import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { ActivatedRoute } from '@angular/router';
import { RegFile } from '../shared/lhc.models';

@Component({
  selector: 'patient-detail',
  templateUrl: './patient-detail.component.html',
  styles: []
})
export class PatientDetailComponent implements OnInit {

  constructor(private data: LhcService, private route: ActivatedRoute) { }

  id: string;
  model: RegFile;
  ngOnInit() {
    this.route.params.subscribe(p => {
      this.id = p["id"];
      this.data.getPatient(this.id).subscribe(data => this.model = data);
    })
  }

}
