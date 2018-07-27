import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AlertType } from './alert-base.component';

@Component({
  selector: 'alert-content',
  templateUrl: './alert.component.html',
  styles: []
})
export class AlertComponent {
    /** sample
    alerts: any = [
        {
            type: 'success',
            msg: `<strong>Well done!</strong> You successfully read this important alert message.`
        },
        {
            type: 'info',
            msg: `<strong>Heads up!</strong> This alert needs your attention, but it's not super important.`
        },
        {
            type: 'danger',
            msg: `<strong>Warning!</strong> Better check yourself, you're not looking too good.`
        }
    ];
    */
    @Input() type: AlertType = AlertType.Warning;
    @Input() message: string;
    @Output() onClose: EventEmitter<any> = new EventEmitter<any>();

    close() {
        this.onClose.emit();
    }
}
