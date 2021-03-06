import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Dictionary, PagedList } from './shared.model';
import { RegFile } from './lhc.model';

@Injectable()
export class LhcService {
    private token: string = "";
    private tokenExpiration: Date;
    private noCacheHeader: HttpHeaders = new HttpHeaders().set('Cache-Control', 'no-cache').set('Pragma', 'no-cache');

  constructor(private http: HttpClient) { }

  private base: string = "http://localhost:59898/";
  //public getValues(token: string): Observable<string[]> {
  //  let header = new HttpHeaders().set("Authorization", "Bearer " + token);
  //  return this.http.get<string[]>(this.base + "api/values", { headers: header });
  //}

  public getValues(): Observable<string[]> {
    return this.http.get<string[]>(this.base + "api/values");
  }

  getAllPatients(): Observable<RegFile[]> {
    return this.http.get<RegFile[]>(this.base + "api/regs/inpatient");
  }

  getPage(page: number, size: number): Observable<PagedList<RegFile>> {
    var params = new HttpParams().set('pageNumber', page.toString()).set('pageSize', size.toString());
    return this.http.get<PagedList<RegFile>>(this.base + 'api/regs/GetPaged', { params: params });
  }

  getPatient(id: string): Observable<RegFile> {
    return this.http.get<RegFile>(this.base + "api/regs/patient/" + id);
  }

  insertPatientDetail(model: RegFile): Observable<RegFile> {
    return this.http.post<RegFile>(this.base + "api/regs/addpatient", model);
  }

  updatePatientDetail(model: RegFile): Observable<boolean> {
    return this.http.put<boolean>(this.base + "api/regs/updatepatient/" + model.RegNo, model);
  }

  deletePatientDetail(id: string): Observable<boolean> {
    return this.http.delete<boolean>(this.base + "api/regs/deletepatient/" + id);
  }

  getToken(): Observable<any> {
    return this.http.post<any>(this.base + "api/account/GetToken", { username: "Tester", email: "tester@example.com", password: "1234" });
  }

  getTemplate(id: number) {
    return this.http.get(this.base + "api/tests/" + id, { responseType: 'text' });
  }
}
