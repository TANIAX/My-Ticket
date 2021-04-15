import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GlobalVariable } from './../Globals/global-variables';


@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  
  constructor(private http : HttpClient) { }

  GenerateTicketHeader(){
    return this.http.get(this.RootUrl + "api/Ticket/Create");
  }
  CreateStaff(body : any){
    return this.http.post(this.RootUrl + "api/Ticket/CreateStaff",body, {headers:this.AuthJSON});
  }
  CreateCustomer(body : any){
    return this.http.post(this.RootUrl + "api/Ticket/CreateCustomer",body, {headers:this.AuthJSON});
  }
  GetTicketList(body : any){
    return this.http.post(this.RootUrl + 'api/Ticket/List',body,{headers:this.AuthJSON});
  }
  GetMyTickets(){
    return this.http.get(this.RootUrl + 'api/Ticket/GetMyTickets');
  }
  GetTicket(id : number){
    return this.http.get(this.RootUrl + 'api/Ticket/'+ id);
  }
  Reply(body: any) {
    return this.http.post(this.RootUrl + 'api/Ticket/Reply', body ,{headers :this.AuthJSON})
  }
  Close(body:any){
    return this.http.post(this.RootUrl + 'api/Ticket/Close', body , {headers:this.AuthJSON})
  }
  Delete(body:any){
    return this.http.post(this.RootUrl + 'api/Ticket/Delete', body , {headers:this.AuthJSON})
  }
  UpdateTickets(body:any){
    return this.http.post(this.RootUrl + 'api/Ticket/Update',body,{headers:this.AuthJSON})
  }
  setSatisfaction(body:any){
    return this.http.post(this.RootUrl + 'api/Ticket/SetSatisfaction',body,{headers:this.AuthJSON})
  }
}
