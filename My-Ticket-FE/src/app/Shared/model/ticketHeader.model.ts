import { OnInit } from '@angular/core';
import { User } from './user.model';
import { TicketLine } from './ticketLine.model';
import { Project } from './project.model';
import { Priority } from './priority.model';
import { Status } from './status.model';
import { Type } from '@angular/compiler';
import { TicketType } from './ticketType.model';
import { FormGroup } from '@angular/forms';
import { Group } from './group.model';
import { Satisfaction } from './satisfaction.model';

export class TicketHeader implements OnInit {
    Id: number;
    Title: string;
    Description: string;
    CreationDate: Date;
    ClosedDate: Date;
    CloseImmediatly: boolean;
    Email: string;
    Status: Status;
    Priority: Priority;
    AssignTO?: User;
    Requester?: User;
    Project?: Project;
    Type?: TicketType;
    Group? : Group;
    Satisfaction? : Satisfaction;
    TicketLines?: TicketLine[];
    position: number;
    checkedOrUnchecked: boolean = false;


    ngOnInit(): void {
        this.Title = "";
        this.Description = "";
        this.CreationDate = new Date();
        this.TicketLines = [];
        this.CloseImmediatly = false;
        this.Email = "";
    }
}

