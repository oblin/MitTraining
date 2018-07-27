import { Component, Input, OnInit, OnChanges, SimpleChanges, DoCheck } from '@angular/core';
import { AbstractControlDirective } from '@angular/forms';

@Component({
  selector: 'form-group',
  templateUrl: './form-group.component.html',
  styles: []
})
export class FormGroupComponent implements OnInit, OnChanges {
    @Input() width: number;
    @Input() required: boolean;
    @Input() controlVariable: AbstractControlDirective;

    private defaultMessage: string = "輸入資料有錯誤，請重新輸入";
    errorMessage: string;

    ngOnChanges(changes: SimpleChanges): void {
        if (this.controlVariable.invalid) {
            //if (this.controlVariable.invalid && this.controlVariable.touched) {
            if (this.controlVariable.errors.required)
                this.errorMessage = "This field is required";
            if (!this.errorMessage)
                this.errorMessage = this.defaultMessage;
        }
    }

    // 每一次滑鼠移動到區域都會被呼叫，因此會有速度慢的問題
    ngDoCheck(): void {
        //if (this.controlVariable.invalid) {
        //    if (this.controlVariable.errors.required)
        //        this.errorMessage = "This field is required";
        //    if (!this.errorMessage)
        //        this.errorMessage = this.defaultMessage;
        //}
    }

    ngOnInit(): void {
        this.controlVariable.valueChanges.subscribe(value => {
            if (this.controlVariable.invalid) {
                if (this.controlVariable.errors.required)
                    this.errorMessage = "此欄位必須要輸入";
                if (!this.errorMessage)
                    this.errorMessage = this.defaultMessage;
            }
        });
    }
}
