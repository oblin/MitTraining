import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Dictionary } from './shared.models';
import { RegFile } from './lhc.models';

@Injectable()
export class LhcService {
    private token: string = "";
    private tokenExpiration: Date;
    private noCacheHeader: HttpHeaders = new HttpHeaders().set('Cache-Control', 'no-cache').set('Pragma', 'no-cache');

  constructor(private http: HttpClient) { }

  private base: string = "http://localhost:59898/";
  public getValues(): Observable<string[]> {
    return this.http.get<string[]>(this.base + "api/values");
  }

  getAllPatients(): Observable<RegFile[]> {
    return this.http.get<RegFile[]>(this.base + "api/regs/inpatient");
  }
}
