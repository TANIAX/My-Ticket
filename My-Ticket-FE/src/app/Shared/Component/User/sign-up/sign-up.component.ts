import { Component, OnInit } from '@angular/core';
import { NgForm, FormsModule, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Major, confirmPassword, forbiddenNames, forbiddenMailDomain } from '../../../../Validator'
import { DatePipe } from '@angular/common';
import { User } from '../../../model/user.model'
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { UserService } from '../../../Service/user.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NavbarService } from '../../../Service/navbar.service';



@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  step: number = 1; // Number of step for the registration
  signupForm: FormGroup;
  responseCode: string; // response of the api to check if user have been receive the mail
  u: User = null; // User object -> we will store the user registration in it 
  password: string = ""; // We declare a password string for the password strenght checker
  confirmPassword: string = "";
  userInputCode: string = ""; // this will me usefull to compare the response code var
  codeMatch: boolean = true; // this will contain the result of the difference between the code response from the api and the user input, it will be helpfull to print error on the html
  registrationError: string = "";
  emailExist: boolean = false;
  isLoading = true;
  constructor(private userService: UserService, private toastr: ToastrService, private router: Router, private navbarService: NavbarService) { }
  ngOnInit() {
    if (localStorage.getItem('userToken') != null) {
      this.router.navigate(['/']);
    }

    // We instancie the object now to avoid error during the store data  in the object later
    this.u = new User();
    //Init of the form
    this.signupForm = new FormGroup({
      'Email': new FormControl(null, [Validators.required, Validators.email, forbiddenMailDomain]),
      'FirstName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'LastName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'Password': new FormControl(null, [Validators.required, Validators.minLength(6)]),
      'ConfirmPassword': new FormControl(null, [Validators.required, confirmPassword])
    });
    //Set default value to the form to avoid error
    this.signupForm.setValue({
      'Email': '',
      'FirstName': '',
      'LastName': '',
      'Password': '',
      'ConfirmPassword': ''
    });

    //This will check if password as change after setting the confirm password 
    this.signupForm.controls.Password.valueChanges.subscribe(
      x => this.signupForm.controls.ConfirmPassword.updateValueAndValidity())

    //When the fully page is loaded
    window.onload = function () {
      //We declare a function there will be usefull for showing error with annimation here
      function ShakeIt(e) {
        var codeMatch = document.getElementById('codeMatch');
        // Add a class that defines an animation if codeMatchExist
        if (codeMatch != null) {
          codeMatch.classList.add('error');

          // remove the class after the animation completes
          setTimeout(function () {
            codeMatch.classList.remove('error');
          }, 300);

          e.preventDefault();
        }

      }
      // The reasion that we make 2 identic fonction is because we got an error if we do in once, the first error annimation is working but not the second //FIXME
      function ShakeIt2(e) {
        var email = document.getElementById('email');
        // Add a class that defines an animation if exist
        if (email != null) {
          email.classList.add('error');

          // remove the class after the animation complete
          setTimeout(function () {
            email.classList.remove('error');
          }, 300);

          e.preventDefault();
        }

      }
      //Attach an event on the input button
      document.getElementById('validateRegistration').addEventListener('click', ShakeIt);
      document.getElementById('submit').addEventListener('submit', ShakeIt2);
    }
    /*FloatLabel inside unput*/
    const FloatLabel = (() => {

      // add active class and placeholder 
      const handleFocus = (e) => {
        const target = e.target;
        target.parentNode.classList.add('active');
        target.setAttribute('placeholder', target.getAttribute('data-placeholder'));
      };

      // remove active class and placeholder
      const handleBlur = (e) => {
        const target = e.target;
        if (!target.value) {
          target.parentNode.classList.remove('active');
        }
        target.removeAttribute('placeholder');
      };

      // register events
      const bindEvents = (element) => {
        const floatField = element.querySelector('input');
        floatField.addEventListener('focus', handleFocus);
        floatField.addEventListener('blur', handleBlur);
      };

      // get DOM elements
      const init = () => {
        const floatContainers = document.querySelectorAll('.float-container');

        floatContainers.forEach((element) => {
          if (element.querySelector('input').value) {
            element.classList.add('active');
          }

          bindEvents(element);
        });
      };

      return {
        init: init
      };
    })();

    FloatLabel.init();
  }
  onSubmit() {
    this.navbarService.updateProgressBar(true)
    if (!this.signupForm.valid) {
      this.navbarService.updateProgressBar(false)
      return;
    }
    //This will check if the user input email already exist in our database and it will wait for a response about our api
    this.userService.EmailExist(this.signupForm.controls.Email.value).subscribe(response => {
      // If everything rules, store the data into the user object
      this.u.Email = this.signupForm.controls.Email.value;
      this.u.FirstName = this.signupForm.controls.FirstName.value;
      this.u.LastName = this.signupForm.controls.LastName.value;
      this.u.Password = this.signupForm.controls.Password.value;
      this.u.confirmPassword = this.signupForm.controls.ConfirmPassword.value;
      this.navbarService.updateProgressBar(false)
      this.SendCodeVerification();
      this.swipeLeft();
    },// If the email exist
      error => {
        console.error(error);
        this.navbarService.updateProgressBar(false)
        if (this.emailExist) {
          var email = document.getElementById('email');
          // Add a class that defines an animation if exist
          if (email != null) {
            email.classList.add('error');

            // remove the class after the animation complete
            setTimeout(function () {
              email.classList.remove('error');
            }, 300);
            event.preventDefault();
          }
          
        }
        this.emailExist = true;
      });

  }

  //this will increment our var to make the next swipe annimation store in the folder ../Swipe
  swipeLeft() {
    this.step++;
  }
  //this will decrement our var to make the previous swipe annimation store in the folder ../Swipe
  swipeRight() {
    this.step--;
  }
  ShowHidePassword() {
    var element = document.getElementById('PasswordInput');
    if ((element as HTMLInputElement).type == "password") {
      (element as HTMLInputElement).type = "text";
      document.getElementById('PasswordShowHideIcon').className = "far fa-eye-slash";
    } else {
      (element as HTMLInputElement).type = "password";
      document.getElementById('PasswordShowHideIcon').className = "fas fa-eye";
    }
  }

  //TODO: block the user if too much request 
  SendCodeVerification() {
    this.userService.GenerateNewCode(this.u.Email).subscribe((response:any) => {
      //The response of the api cannot be highter than 5 lenght, so if it is, this is an error
      if (response.codeConfirmation.lenght > 5) {
        return;
      }
      //The response code is at object format so we make it string
      this.responseCode = response.codeConfirmation;
    },error=>{
      console.error(error);
    });
  }

  ValidateRegistration() {
    this.navbarService.updateProgressBar(true)
    if (this.userInputCode.toUpperCase() != this.responseCode.toUpperCase()) {
      this.codeMatch = false;
      this.navbarService.updateProgressBar(false)
      return;
    } else {
      this.codeMatch = true;
    }

    //If the code is correct, we will try to store the user to our database
    if (this.codeMatch) {
      this.userService.Register(this.u).subscribe(async response => {
        this.navbarService.updateProgressBar(false)
        this.toastr.success('Success', 'Your account is succefuly created, You will be redirected in a few second.', {
          timeOut: 30000
        });
        //We wait 3 seconds before redirect him to the base url ( index )
        await delay(3000);
        this.router.navigate(['/User/Login']);
      }, error => {
        this.navbarService.updateProgressBar(false)
        // If something is going wrong inform the user  //TODO: Try to show a more efficient message 
        console.log(error)
        this.toastr.error('Error', error.message, {
          timeOut: 3000
        });
      });
    }
  }
}
//This function is a sleep() fonction
function delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
