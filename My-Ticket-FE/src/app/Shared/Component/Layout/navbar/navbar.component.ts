import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../Service/user.service';
import { Router } from '@angular/router';
import { NavbarService } from '../../../Service/navbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isLogged: Boolean = false;
  isLoading: Boolean = false;
  IsCollapse:boolean = false;
  PP: string = "";
  userName: string = "";
  Lastname : string = "";
  Firstname : string = "";
  constructor(private userService: UserService, private router: Router, private navbarService: NavbarService) { }

  ngOnInit() {
    this.userService.isLogged$.subscribe(isLogged => this.isLogged = isLogged);
    this.navbarService.isLoading$.subscribe(isLoading => this.isLoading = isLoading);
    if (localStorage.getItem('userToken') != null) {
      this.isLogged = true;
      this.PP = localStorage.getItem('PP');
      this.userName = localStorage.getItem('Username');
      this.Lastname = localStorage.getItem('LastName');
      this.Firstname = localStorage.getItem('FirstName');
    }
  }

  Logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('userRoles');
    localStorage.removeItem('PP');
    localStorage.removeItem('Username');
    localStorage.removeItem('LastName');
    localStorage.removeItem('FirstName');
    localStorage.removeItem('Signature');
    this.isLogged = false;
    this.router.navigateByUrl('/Home');
  }
  CreateTicket(){
    if(this.userService.roleMatch(["Member","Admin"]))
      this.router.navigate(["Staff/Ticket/Create"]);
    else
      this.router.navigate(["Ticket/Create"]);
  }
}


