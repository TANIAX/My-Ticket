import { Component, OnInit } from '@angular/core';
import { UserService } from '../../Service/user.service';
import { NavbarService } from '../../Service/navbar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  
  isLogged : Boolean = false;
  isLoading : Boolean = false;
  IsCollapse:boolean = false;
  year : number
  PP : string = "";
  userName: string = "";
  Lastname : string = "";
  Firstname : string = "";
  constructor(private userService: UserService, private router: Router, private navbarService: NavbarService) { }

  ngOnInit() {
    window.onscroll = function () { scrollFunction() };
    this.userService.isLogged$.subscribe(isLogged => this.isLogged = isLogged);
    this.navbarService.isLoading$.subscribe(isLoading => {
      this.isLoading = isLoading;
    }
    );
    if (localStorage.getItem('userToken') != null) {
      this.isLogged = true;
      this.PP = localStorage.getItem('PP');
      this.userName = localStorage.getItem('Username');
      this.Lastname = localStorage.getItem('LastName');
      this.Firstname = localStorage.getItem('FirstName');
    }
    this.year = new Date().getFullYear();
  }

  Logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('userRoles');
    localStorage.removeItem('PP');
    localStorage.removeItem('Username');
    localStorage.removeItem('LastName');
    localStorage.removeItem('FirstName');
    this.isLogged = false;
    this.router.navigateByUrl('/Home');
  }
  

}

function scrollFunction() {
  if (document.body.scrollTop > 30 || document.documentElement.scrollTop > 30) {
    document.getElementById("navbar").style.padding = "0px";
    document.getElementById("brand").style.fontSize = "24px";
    var pb = document.getElementById("progressBar");
    if (pb != null) {
      document.getElementById("progressBar").style.top = "46px";
    }

  } else {
    document.getElementById("navbar").style.padding = "20px";
    document.getElementById("brand").style.fontSize = "35px";
    var pb = document.getElementById("progressBar");
    if (pb != null) {
      document.getElementById("progressBar").style.top = "102px";
    }
  }
}
