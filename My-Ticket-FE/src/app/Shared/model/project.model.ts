import { User } from './user.model';
import { OnInit } from '@angular/core';

export class Project implements OnInit {
    Id : number;
    Name : string;
    CreationDate : Date;
    CreatedBy : User;

    ngOnInit(): void {
        this.Name = "";
        this.CreationDate = new Date();
    }
}
