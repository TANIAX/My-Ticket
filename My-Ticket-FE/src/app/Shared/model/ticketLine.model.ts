import { OnInit } from '@angular/core';
import { User } from './user.model';

export class TicketLine implements OnInit {
    Id : number;
    content : string;
    askForClose : boolean;
    responseBy : User;
    responseDate : Date;
    Email : string;

    ngOnInit(): void {
        this.content = "";
        this.askForClose = false;
        this.responseDate = new Date();
        this.Email = "";
    }
}
