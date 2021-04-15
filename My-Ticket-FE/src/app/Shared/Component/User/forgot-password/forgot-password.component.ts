import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../../Service/user.service';
import { NavbarService } from '../../../Service/navbar.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  forgotForm: FormGroup;//Our Forgot password form, it will contain the email of the user
  showMessage: boolean = false;//This will be set to true when the user will submit his request and it will show in the html a message to prevent him he should receive an email
  constructor(private router: Router, private userService: UserService, private navbarService: NavbarService) { }

  // Creation of our forgot password form
  ngOnInit() {
    if(localStorage.getItem('userToken') != null){
      this.router.navigate(['/']);
    }
    this.forgotForm = new FormGroup({
      'Email': new FormControl(null, [Validators.required, Validators.email])
    });
    //Set default value to avoid error
    this.forgotForm.setValue({
      'Email': ''
    });
  }
  //When the user submit his request
  onSubmit() {
    //Loading annimation for adverting the user the application is working
    this.navbarService.updateProgressBar(true);
    //If the form is not valid and the user tryed to bypass that, stop the animation and get back to the html
    if (!this.forgotForm.valid) {
      this.navbarService.updateProgressBar(false);
      return;
    }
    //Call or api to send an email for resetting the user password
    this.userService.forgotPassword(this.forgotForm.controls.Email.value).subscribe(response => {
      //Show the message in the html
      this.showMessage = true;
      //Stop the animation
      this.navbarService.updateProgressBar(false);
    },error=>{
      this.navbarService.updateProgressBar(false);
    });
  }

}
