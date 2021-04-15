import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    //This function is empty for override(unload) the function in the home component 
    window.onscroll = function () { scrollFunction() };
  }

}
function scrollFunction() {
  
}
