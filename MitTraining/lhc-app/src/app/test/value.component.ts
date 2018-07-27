import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';

@Component({
  selector: 'value',
  templateUrl: './value.component.html',
  styles: []
})
export class ValueComponent implements OnInit {

  constructor(private data: LhcService) { }

  values: string[];
  ngOnInit() {
    this.data.getValues().subscribe(data => this.values = data);
  }
}
