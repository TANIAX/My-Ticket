import { OnInit } from '@angular/core';

export class Type implements OnInit {
    Id : number;
    Name : string;
    ngOnInit(): void {
        this.Name = "";
    }
}
