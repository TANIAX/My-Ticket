import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../Service/user.service';
import { confirmPassword } from '../../../../Validator';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { NavbarService } from '../../../Service/navbar.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup;//This wil contain our reset form
  code: string = ""; // This is our code we will retrieve by the URL
  password: string = "";// The password var is here for the password-strength-meter component
  constructor(private userService: UserService, private route: ActivatedRoute, private router: Router,private toastr: ToastrService, private navbarService: NavbarService) { }

  ngOnInit() {
    if(localStorage.getItem('userToken') != null){
      this.router.navigate(['/']);
    }
    //We create our form
    this.resetForm = new FormGroup({
      'Email': new FormControl(null, [Validators.required, Validators.email]),
      'Password': new FormControl(null, [Validators.required, Validators.minLength(6)]),
      'ConfirmPassword': new FormControl(null, [Validators.required, confirmPassword])
    });
    //We set default value to avoid error
    this.resetForm.setValue({
      'Email': '',
      'Password': '',
      'ConfirmPassword': ''
    });

    //This will check if password as change after setting the confirm password 
    this.resetForm.controls.Password.valueChanges.subscribe(
      x => this.resetForm.controls.ConfirmPassword.updateValueAndValidity())

    //this will retrieve the first parameters of the url and bind it to our code var 
    this.route.params
      .subscribe(
        (params: Params) => {
          this.code = params['code'];
        }
      );
    let re = /\{/gi; // This is a regex, It is made for detecting the char { of the code string
      //We do that because our backend replace the / with a { to prevent the detection of multiple parameters
    this.code = this.code.replace(re, "/");  
    //We add this because the code we need to resend to our backend always finish with a == and when it pass by the url , it ignored  ( I don't know why but it do )
  }

  //When the user submit his reset password form
  onSubmit() {
    //Loading annimation for adverting the user the application is working
    this.navbarService.updateProgressBar(true)
    //We check if the form is not valid, and if does we do go further
    if (!this.resetForm.valid){
      //Stop the annimation
      this.navbarService.updateProgressBar(false)
      return;
    }
    //Call our api and sent it the user email, password and the code we have been replace the { char  
    this.userService.resetPassword(this.resetForm.controls.Email.value, this.resetForm.controls.Password.value, this.code).subscribe(response => {
      //If our backed tell is that's ok, show a success toastr, stop the loading animation and redirect to the login page
      this.toastr.success('Success', "Your password is been reset, you will be redirected to login page in a few seconds", {
        timeOut: 3000
      });
      this.navbarService.updateProgressBar(false)
      delay(3000);     
      this.router.navigate(['/User/Login'])
      //If something is going wrong show a toastr error and stop the loading animation
    }, error => {
      this.toastr.error('Error', error.error.error, {
        timeOut: 3000
      });
      console.error(error);
      this.navbarService.updateProgressBar(false)
    });
  }

}
//This function is a sleep() fonction
function delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
