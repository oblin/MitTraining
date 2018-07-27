import { Directive, Input, ElementRef } from '@angular/core';

@Directive({
    selector: '[faIcon]'
})
export class FaIconDirective {
    @Input('faIcon') faIcon: string;

    constructor(private el: ElementRef) {
        el.nativeElement.innerHtml = `<i class='fa ${this.faIcon}'></i>`;
        
    }

    ngOnInit() {
        this.el.nativeElement.innerHTML += `<i class='fa ${this.faIcon}'></i>`;
    }
}