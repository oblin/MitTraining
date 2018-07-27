import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'age'
})
export class AgePipe implements PipeTransform {

  transform(value: Date): number {
    let today = moment();
    let birthdate = moment(value);
    return today.diff(birthdate, 'years');
    //let result: string = years + " yr ";

    //result += today.subtract(years, 'years').diff(birthdate, 'months') + " mo";

    //return result;
  }

}
