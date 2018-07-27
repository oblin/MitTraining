import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class LhcService {
    private token: string = "";
    private tokenExpiration: Date;
    private noCacheHeader: HttpHeaders = new HttpHeaders().set('Cache-Control', 'no-cache').set('Pragma', 'no-cache');

    constructor(private http: HttpClient) { }
}
