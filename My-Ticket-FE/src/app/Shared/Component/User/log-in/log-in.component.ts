import { Component, OnInit, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../../Service/user.service';
import { NgxCaptchaModule, ReCaptchaV3Service } from 'ngx-captcha';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from '../../../Service/navbar.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.scss']
})
export class LogInComponent implements OnInit {
  loginForm: FormGroup; // Our login form, it will contain the user name and the password
  loginError: boolean = false; // This will set to true if the email and password doens't match to an account
  detectedAsBot:boolean = false; // This will set to true if recaptchav3 will atribute a note <0.3
  firstTry: boolean = true; // If the user never fail is login before
  readonly siteKey = '6Lf9K64UAAAAADqj1mDA7v1Qb1s3C3WckbKjZuXk'; // This is the public key of recaptcha v3
  token: string = "" // This will contain the recaptcha response
  isLogged: boolean = false; // This will set to true if the user as succefully connected to our backend

  constructor(private router: Router, private userService: UserService, private reCaptchaV3Service: ReCaptchaV3Service, private toastr: ToastrService, private navbarService: NavbarService) { }
  // Creation of our login form
  ngOnInit() {
    if(localStorage.getItem('userToken') != null){
      this.router.navigate(['/']);
    }
    this.loginForm = new FormGroup({
      'username': new FormControl(null, [Validators.required, Validators.email]),
      'password': new FormControl(null, [Validators.required])
    });
    //Set default value to the first form to avoid error
    this.loginForm.setValue({
      'username': '',
      'password': ''
    });


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
        if(!target.value) {
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

    if(!this.loginForm.valid){
      this.navbarService.updateProgressBar(false)
      return;
    }

    this.reCaptchaV3Service.execute(this.siteKey, 'Login', (response) => {

      this.token = response;

      this.userService.getCaptchaResponse(this.token).subscribe((response: any) => {
        if (response.IsSuccessStatusCode) {
          //Call our api to try connecting with the user information
          this.userService.userAuthentication(this.loginForm.controls.username.value, this.loginForm.controls.password.value).subscribe((response: any) => {
            //TODO set cookie if remember me
            
            localStorage.setItem('userToken', response.UserToken);
            localStorage.setItem('userRoles', response.UserRoles);
            localStorage.setItem('PP',response.PP);
            localStorage.setItem('Username',response.Username);
            localStorage.setItem('LastName',response.LastName);
            localStorage.setItem('FirstName',response.FirstName);
            localStorage.setItem('Signature',response.Signature);
            
            //Tell the application we got the token
            this.isLogged = true;
            //This will refresh the navbar (hide signup, login and show sign out ect....)
            this.userService.updateIsLoggedNavbar(true);

            this.navbarService.updateProgressBar(false)
            //Reload the whole website, it will redirect to the home page
            window.location.reload();

          }, error => {
            console.error(error);
            this.navbarService.updateProgressBar(false);
            //If the user is at his first tentative of connexion
            if (this.firstTry == true) {
              //Show a login error 
              this.loginError = true;
              this.firstTry = false;
            } else {
              var badLoginOrPassword = document.getElementById('error');
              if (badLoginOrPassword != null) {
                //Show a shake error annimation during 300 milli seconds
                badLoginOrPassword.classList.add('error');
                setTimeout(function () {
                  badLoginOrPassword.classList.remove('error');
                }, 3000);
              }
            }
          });
          //If our user is detected as a bot
        } else {
          //same as above
          this.navbarService.updateProgressBar(false)
          this.detectedAsBot = true;
          var bot = document.getElementById('errorBot');
          if (bot != null) {
            
            bot.classList.add('error');
            setTimeout(function () {
              bot.classList.remove('error');
            }, 3000);
          }
        }
        //We enter here if something gone wrong during the google api call
      }, error => {
        console.error(error);
        this.navbarService.updateProgressBar(false)
        this.router.navigate(['/500']);
      });
    },{
      //To be honest .... I don't know, I just know if i remove this, that's not working, i suppose that like a try catch FINNALY 
        useGlobalDomain: false
      });
  }
}
//This is a sleep() function
function delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
