import { OnInit } from '@angular/core';
import { StoredReply } from './storedReply.model';

export class User implements OnInit {
    Id : string;
    Email: string;
    FirstName: string;
    LastName: string;
    Birthdate:Date;
    Password: string;
    confirmPassword: string;
    UserName: string;
    
    PhoneNumber:string;
    Country:string;
    District:string;
    ZipCode:number;
    Locality:string;
    Street:string;

    Signature:string;
    Language:string;
    PP:string;
    Exist: boolean;
    IsCompany:boolean;
    CompanyName:string;
    isEmployee:boolean;
    UserList:User[];
    StoredReply : StoredReply[];

    position:number;
    

    ngOnInit() {
    this.Id = "";
    this.Email = "";
    this.FirstName = "";
    this.LastName = "";
    this.Birthdate = new Date();
    this.Password = "";
    this.confirmPassword = "";
    this.UserName = "";
    
    this.PhoneNumber = "";
    this.Country = "";
    this.District = "";
    this.ZipCode = 0;
    this.Locality = "";
    this.Street = "";

    this.Signature = "";
    this.Language = "";
    this.PP = "";
    
    this.IsCompany = false;
    this.CompanyName = "";
    this.isEmployee = false;
    }

}
