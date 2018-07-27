import { NgForm, NgModel } from '@angular/forms';
import { CodeFile, CodeService, Code } from './code.service';
import { Globals } from './globals.service';
import * as _ from 'lodash';
import { OnInit } from '@angular/core';
import { AlertBaseComponent } from './alert-base.component';

/**
 * 提供共用的 BaseComponent 主要的服務：
 * 1. 提供 codes： 可以寄放所有的下拉單選項
 * 2. onDropdownChange()：當 parent code changed 時候，可以變更 cascade dropdownlist
 * 3. getCode()：取出 codes 所對應的代碼資料
 * 4. setCode()：設定代碼資料
 * 5. dateFormat()：將任意的日期格式（包含民國年）轉成 yyyy/MM/dd
 */
export class BaseComponent extends AlertBaseComponent implements OnInit {
    private origin: any;
    // dropdown selections 改為使用 codeService.ExistSelections
    //protected codes: CodeFile[] = new Array<CodeFile>();

    constructor(protected globals: Globals, protected codeService: CodeService) {
        super();
    }

    ngOnInit(): void {
        //this.codes = this.codeService.ExistSelections;        
    }

    /**
     * 當父項目變更時候，呼叫此函數處理子項目的選項變更
     * @param parentValue 對應 parentCode
     * @param code 對應子項目的 ItemType
     */
    getCodeByParent(code: string, parentCode: NgModel): Code[] {
        if (code && parentCode.model) {
            var result = this.codeService.ExistSelections.filter(item => item.ItemType == code && item.ParentCode == parentCode.model);
            return result.length > 0 ? result[0].Details : [];
        }

        return [];
    }

    onInit(backup: any) {
        // 這個會重複讀取，只有當 cascade dropdown 才有意義，因此先省略
        //this.codeService.getAllChildren(this.codes);
        this.keepOrigin(backup);
    }

    /**
     * 使用字串取出子類別
     * @param code
     * @param parentCode
     */
    getParentCode(code: string, parentCode: string): Code[] {
        if (code && parentCode) {
            var result = this.codeService.ExistSelections.filter(item => item.ItemType == code && item.ParentCode == parentCode);
            return result.length > 0 ? result[0].Details : [];
        }

        return [];
    }

    /**
     * 取出 codes 陣列中，針對 ItemType 的選項清單
     * @param code
     */
    getCode(code: string): Code[] {
        if (code) {
            var result = this.codeService.ExistSelections.filter(item => item.ItemType == code);
            return result.length > 0 ? result[0].Details : [];
        }
    }

    /**
     * 變更 codes 陣列的項目內容，主要提供給 dropdown cascade，變更子項目的選項
     * @param code: string 要變更的 ItemType 值
     * @param codes: Code[] 提供的選項
     */
    setCode(code: string, selections: Code[]): CodeFile {
        let result: CodeFile;
        if (code) {
            this.codeService.ExistSelections.filter(item => item.ItemType == code)[0].Details = selections;
            result = CodeFile[0];
            result.Details = selections;
        }
        return result;
    }

    /**
     * 將任意的日期格式（包含民國年）轉成 yyyy/MM/dd
     * @param date 可以是民國年： 85/01/23 
     */
    dateFormat(date: string): string {
        return this.globals.dateFormatter(date);
    }

    /**
     * 將 model 的原始資料儲存
     * @param backup
     */
    keepOrigin(backup: any) {
        this.origin = _.cloneDeep(backup);
    }

    /**
     * 將原始資料回傳
     */
    restoreOrigin(): any {
        return this.origin;
    }

    /**
     * 回傳原始資料，並且將 form 所有 controls 的狀態清空
     * @param form: NgForm
     */
    resetAndRestore(form: NgForm): any {
        this.reset(form);
        return this.origin;
    }

    reset(form: NgForm, model: any = null): void {
        for (var name in form.controls) {
            form.controls[name].setErrors(null);
            form.controls[name].markAsPristine();
        }
        if (model) {
            this.onInit(model);
        }
    }

    trimField(field: string, length: number) {
        return field
                ? field.substr(0, Math.min(length, field.length)) + "..."
                : '';
    }
}
