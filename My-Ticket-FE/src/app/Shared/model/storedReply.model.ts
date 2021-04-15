import { OnInit } from '@angular/core';

export class StoredReply implements OnInit {
    Id : number;
    Title : string;
    Reply : string;
    position: number;
    ngOnInit(): void {
        this.Title = "";
        this.Reply = "";
    }
}
