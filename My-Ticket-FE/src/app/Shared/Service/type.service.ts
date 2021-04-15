import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';

@Injectable({
  providedIn: 'root'
})
export class TypeService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  constructor(private http : HttpClient) { }

  GetTypeList(){
    return this.http.get(this.RootUrl + "api/Type", {headers : this.AuthJSON});
  }
}
