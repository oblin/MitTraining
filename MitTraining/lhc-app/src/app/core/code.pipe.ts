import { Pipe, PipeTransform } from '@angular/core';
import { CodeService } from './code.service';

@Pipe({
    name: 'code'
})
export class CodePipe implements PipeTransform {
    constructor(private codeService: CodeService) { }

    transform(value: string, name: string): string {
        if (name === 'boolean') {
            return value ? '是' : '否';
        }

        if (value && name) {
            return this.codeService.getValue(name, value);
        }
        else
            return value;
    }

}
