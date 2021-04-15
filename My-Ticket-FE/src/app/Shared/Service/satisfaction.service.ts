import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';

@Injectable({
  providedIn: 'root'
})
export class SatisfactionService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  GetSatisfactionList(){
    return this.http.get(this.RootUrl + "api/Satisfaction", {headers : this.AuthJSON});
  }
}
