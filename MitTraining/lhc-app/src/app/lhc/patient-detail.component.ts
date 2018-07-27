import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'patient-detail',
  templateUrl: './patient-detail.component.html',
  styles: []
})
export class PatientDetailComponent implements OnInit {

  constructor(private data: LhcService, private route: ActivatedRoute) { }

  id: string;
  ngOnInit() {
    this.route.params.subscribe(p => {
      this.id = p["id"];
    })
  }

}
