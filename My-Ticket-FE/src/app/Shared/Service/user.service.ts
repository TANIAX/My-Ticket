import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { User } from '../model/user.model';
import { GlobalVariable } from './../Globals/global-variables';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly RootUrl: string = GlobalVariable.BASE_API_URL;
  private readonly noAuthreqHeader = new HttpHeaders({ 'No-Auth': 'True' });
  private readonly AuthJSON = new HttpHeaders({ 'Content-Type': 'application/json' });
  private isLogged: BehaviorSubject<Boolean> = new BehaviorSubject<Boolean>(null);
  public isLogged$: Observable<Boolean> = this.isLogged.asObservable();

  constructor(private http: HttpClient) { }
  /*GET REQUEST*/
  // getUserClaims(){
  //   return  this.http.get(this.RootUrl+'/api/GetUserClaims');
  //  }
  // getAllRoles() {
  //   return this.http.get(this.RootUrl + '/api/GetAllRoles', { headers: this.noAuthreqHeader });
  // }

  getEditMyProfile() {
    return this.http.get(this.RootUrl + 'api/users/EditMyProfile');
  }

  getMyAccount() {
    return this.http.get(this.RootUrl + 'api/users/MyAccount');
  }

  getCurrentUser() {
    return this.http.get(this.RootUrl + 'api/users/GetCurrentUser');
  }
  getMemberList() {
    return this.http.get(this.RootUrl + 'api/users/GetMemberList');
  }

  GetMyEmployees() {
    return this.http.get(this.RootUrl + 'api/users/Employees/List');
  }
  /*POST REQUEST*/
  Register(body: User) {
    return this.http.post(this.RootUrl + 'api/users/Register', body, { headers: this.noAuthreqHeader });
  }

  EmailExist(email: string) {
    return this.http.post(this.RootUrl + 'api/users/EmailExist', { Email: email }, { headers: this.noAuthreqHeader });
  }

  forgotPassword(email: string) {
    return this.http.post(this.RootUrl + 'api/users/ForgotPassword', { Email: email }, { headers: this.noAuthreqHeader });
  }

  resetPassword(email: string, password: string, code: string) {
    return this.http.post(this.RootUrl + 'api/users/ResetPassword', { Email: email, Password: password, Code: code }, { headers: this.noAuthreqHeader });
  }

  userAuthentication(userName, password) {
    var data = "username=" + userName + "password=";
    return this.http.post(this.RootUrl + 'api/Auth/Login', { UserName: userName, Password: password }, { headers: this.noAuthreqHeader });
  }

  getCaptchaResponse(token) {
    return this.http.post(this.RootUrl + 'api/users/CheckCaptcha', { Token: token }, { headers: this.noAuthreqHeader });
  }

  GenerateNewCode(email: string) {
    return this.http.post(this.RootUrl + 'api/users/GenerateRandomCode', { Email: email }, { headers: this.noAuthreqHeader });
  }
  updateMyAccount(body) {
    return this.http.post(this.RootUrl + 'api/users/MyAccount', body, { headers: this.AuthJSON });
  }

  setEditMyProfile(body) {
    return this.http.post(this.RootUrl + 'api/users/EditMyProfile', body, { headers: this.AuthJSON });
  }
  CreateEmployee(body: any) {
    return this.http.post(this.RootUrl + "api/users/Employees/Create", body, { headers: this.AuthJSON });
  }
  UpdateEmployee(body: any) {
    return this.http.post(this.RootUrl + "api/users/Employees/Update", body, { headers: this.AuthJSON });
  }
  DeleteEmployee(body: any) {
    return this.http.post(this.RootUrl + "api/users/Employees/Delete", body, { headers: this.AuthJSON });
  }
  ResetEmployeePassword(body: any) {
    return this.http.post(this.RootUrl + "api/users/Employees/ResetPassword", body, { headers: this.AuthJSON });
  }
  AddCustomer(body:any){
    return this.http.post(this.RootUrl + "api/users/Customer/Create",body,{ headers: this.AuthJSON })
  }
  getUser(body) {
    return this.http.post(this.RootUrl + "api/users/Customer/Get", body,{ headers: this.AuthJSON });
  }
  EditCustomer(body) {
    return this.http.post(this.RootUrl + "api/users/Customer/Edit", body,{ headers: this.AuthJSON });
  }
  /*OTHER*/
  roleMatch(allowedRoles): boolean {
    var isMatch = false;
    var userRoles: string[] = JSON.parse(localStorage.getItem('userRoles'));
    allowedRoles.forEach(element => {
      if (userRoles.indexOf(element) > -1) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }

  updateIsLoggedNavbar(isLogged) {
    this.isLogged.next(isLogged);
  }
}
