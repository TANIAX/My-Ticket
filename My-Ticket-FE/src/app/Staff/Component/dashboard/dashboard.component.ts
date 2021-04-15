import { Component, OnInit } from '@angular/core';



@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  p = 80;
  m = 5;
  dc = 15;
  constructor() { }
 
  ngOnInit() {
    this.m += this.p;
  }

}
