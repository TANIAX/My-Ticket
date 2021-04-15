import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';

@Injectable({
  providedIn: 'root'
})
export class PriorityService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  GetPriorityList(){
    return this.http.get(this.RootUrl + "api/Priority", {headers : this.AuthJSON});
  }
}
