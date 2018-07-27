import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import * as _ from 'lodash';

@Injectable()
export class CodeService {
    private baseUrl = '/api/codes/';

    constructor(private http: HttpClient) { }

    // calls the [GET] /api/codes/{id} Web API method to retrieve the item with the given id.
    get(key: string, parentCode: string = null, needEmpty: boolean = false, codeFile: CodeFile): void {
        if (key == null) { throw new Error('id is required.'); }

        let params = new HttpParams();
        // HttpParams is an immutable object，因此需要利用 assign 
        params = params.append('code', key);
        if (parentCode)
            params = params.append("parentCode", parentCode);

        this.http.get<Code[]>(this.baseUrl, { params: params })
            .subscribe(response => {
                let items: Code[] = <Code[]>response;
                if (needEmpty) {
                    items.unshift(new Code('', "---請選擇---"));
                }
                codeFile.Details = items;
                this.selections.push(codeFile);
                //return items;
            });
    }

    getAllChildren(selections: CodeFile[]): void {
        for (var i = 0; i < selections.length; i++) {
            var item = selections[i];
            this.get(item.ItemType, item.ParentCode, item.NeedEmpty, item);
        }
    }

    private selections: CodeFile[] = new Array<CodeFile>();
    setCodeSelections(codes: CodeFile[]): void {
        for (var i = 0; i < codes.length; i++) {
            var code = this.selections.find(p => p.ItemType == codes[i].ItemType);
            if (!code) {
                var item = codes[i];
                this.get(item.ItemType, item.ParentCode, item.NeedEmpty, item);
            }
        }
    }

    getValue(code, value): string {
        var list = this.selections.find(p => p.ItemType == code);

        if (!list) {
            console.log(code);
            throw "沒有找到對應的代碼檔，需要再 constructor 中呼叫 setCodeSelections 建立"
        }
        else {
            let result = list.Details.find(p => p.ItemCode == value);
            if (!result) {
                console.log(code);
                throw "沒有找到對應的代碼，請檢察 CodeDetail 是否有對應的代碼"
            }
            return result.Description;
        }
    }

    get ExistSelections(): CodeFile[] { return this.selections; }
}

export class Code {
    constructor(
        public ItemCode: string,
        public Description: string) { }
}

export class CodeFile {
    constructor(
        public ItemType: string,
        public ParentType: string,
        public ParentCode: string,
        public TypeName: string,
        public Details: Code[],
        public NeedEmpty: boolean = false,
        public Id: number = 0
    ) {
        if (Details == null) {
            this.Details = new Array<Code>();
        }
    }

    static defaultType(): CodeFile {
        return new CodeFile("", "", "", "", new Array<Code>());
    }

    static initType(itemType: string, parentType: string, parentCode: string, needEmpty: boolean = false) {
        return new CodeFile(itemType, parentType, parentCode, null, null, needEmpty);
    }
}
