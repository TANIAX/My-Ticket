import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-side-panel',
  templateUrl: './side-panel.component.html',
  styleUrls: ['./side-panel.component.scss']
})
export class SidePanelComponent implements OnInit {
  isCollapse = true;
  constructor() { }

  ngOnInit() {
  }
  SidePanelCollapse(){
    if(this.isCollapse){
      document.getElementById("mySidepanel").style.width = "130px";
      document.getElementById("sidePanelButton").style.marginLeft = "130px";
      document.getElementById("iArrow").className = "fas fa-angle-left";
    }else{
      document.getElementById("mySidepanel").style.width = "0px";
      document.getElementById("sidePanelButton").style.marginLeft = "0px";
      document.getElementById("iArrow").className = "fas fa-angle-right";
    }
    this.isCollapse = !this.isCollapse;
  }
}
