import { Component, OnInit } from '@angular/core';
import { LhcService } from '../shared/lhc.service';

@Component({
  selector: 'value',
  templateUrl: './value.component.html',
  styles: []
})
export class ValueComponent {
  constructor(private data: LhcService) { }

  values: string[];

  getValues() {
    this.data.getValues().subscribe(data => this.values = data);
  }
}
