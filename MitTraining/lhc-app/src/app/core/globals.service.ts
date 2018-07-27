import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { Dictionary } from '../shared/shared.models';

@Injectable()
export class Globals {
    private model: any;

    /**
     * 用來接收外部傳來的參數，可以透過 Json 方式指定多重參數。
     * 外部傳入範例：
     * 設定： var @params = new { Default = ViewBag.Code, Clinics = ViewBag.Clinics, Date = DateTime.Now };
     * 在 app-root 中設定 <app-root defaultRoute="query" model='@Json.Serialize(@params)'></app-root>
     */ 
    get razorModel(): any {
        return this.model;
    }

    set razorModel(data) {
        if (this.isJSON(data))
            this.model = JSON.parse(data);
        else
            this.model = data;
    }

    public isJSON(MyTestStr) {
        try {
            var MyJSON = JSON.stringify(MyTestStr);
            var json = JSON.parse(MyJSON);
            if (typeof (MyTestStr) == 'string')
                if (MyTestStr.length == 0)
                    return false;
        }
        catch (e) {
            return false;
        }
        return true;
    }

    public getAge(birthday: Date): number {
        return moment().diff(birthday, 'years');
    }

    /**
     * 將 Json object map to dictionary
     * @param dict
     */
    public mapToIterable(dict: Object): Dictionary[] {
        let result: Dictionary[] = new Array<Dictionary>();
        for (var key in dict) {
            if (dict.hasOwnProperty(key)) {
                result.push({ key: key, value: dict[key] });
            }
        }

        return result;
    }

    private dateFormat = "YYYY/MM/DD";

    public dateToString(d: Date): string {
        return moment(d).format(this.dateFormat);
    }

    public dateFormatter(s: string): string {
        if (s == null) return "";

        if (s.length == 0 || s.length < 6) return "";

        if (s.length == 6) {
            var result = this.dealSlashDate(s);
            if (result.length == 10)
                return result;
            // 處理 990101
            s = '0' + s;
        }
        if (s.length == 7) {
            var result = this.dealSlashDate(s);
            if (result.length == 10)
                return result;
            // 處理 0990101 or 1031201
            var y = parseInt(s.substr(0, 3), 10) + 1911;
            var m = parseInt(s.substr(3, 2), 10);
            var d = parseInt(s.substr(5, 2), 10);
            var date = y + "/" + m + "/" + d;
            //if (moment(date).isValid()) {
            //    var returnDate = moment(date).format(dateFormat);
            //    var stringDate = returnDate.toString();
            //    return stringDate;
            //}
            //else
            //    return "";
            return this.getDateString(date);
        }
        if (s.length == 8 || s.length == 9) {
            var result = this.dealSlashDate(s);
            if (result.length == 10)
                return result;
        }

        return this.getDateString(s);
    }

    private dealSlashDate(s): string {
        if (s.indexOf('/') > -1) {
            var array = s.split('/');
            if (array.length == 3) {
                var y = parseInt(array[0]) + 1911;
                var m = parseInt(array[1], 10);
                var d = parseInt(array[2], 10);
                var date = y + "/" + m + "/" + d;
                return this.getDateString(date);
            }
        }
        return s;
    }

    private getDateString(s): string {
        if (moment(s).isValid()) {
            var returnDate = moment(s).format(this.dateFormat);
            var stringDate = returnDate.toString();
            return stringDate;
        } else {
            return "";
        }
    }
}
