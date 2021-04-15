import { OnInit } from '@angular/core';

export class Group implements OnInit {
    Id : number;
    Name : string;
    ngOnInit(): void {
        this.Name = "";
    }
}
