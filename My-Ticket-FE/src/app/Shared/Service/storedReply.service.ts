import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';

@Injectable({
  providedIn: 'root'
})
export class StoredReplyService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  constructor(private http : HttpClient) { }

  Create(body:any){
    return this.http.post(this.RootUrl + "api/StoredReplies/Create",body, {headers : this.AuthJSON});
  }

  Update(body:any){
    return this.http.post(this.RootUrl + "api/StoredReplies/Update",body, {headers : this.AuthJSON});
  }
  Delete(body:any){
    return this.http.post(this.RootUrl + "api/StoredReplies/Delete",body, {headers : this.AuthJSON});
  }
}
