import { Component, OnInit, Input } from '@angular/core';
import { Code } from './code.service';

@Component({
  selector: '[code-options]',
  templateUrl: './code-options.component.html',
  styles: []
})
export class CodeOptionsComponent {
    @Input() codes: Code[];
    @Input() ngModel: string;
}
