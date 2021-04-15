import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';


@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  GetGroupList(){
    return this.http.get(this.RootUrl + "api/Groups", {headers : this.AuthJSON});
  }
  GetGroupListWithNumberOfMember(){
    return this.http.get(this.RootUrl + "api/Groups/List", {headers : this.AuthJSON});
  }
}
