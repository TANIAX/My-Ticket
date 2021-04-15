import { User } from './user.model';
import { OnInit } from '@angular/core';

export class Priority implements OnInit {
    Id : number;
    Name : string;
    ngOnInit(): void {
        this.Name = "";
    }
}
