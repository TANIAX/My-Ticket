import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { User } from 'src/app/Shared/model/user.model';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UserService } from 'src/app/Shared/Service/user.service';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { forbiddenNames } from 'src/app/Validator';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-create-customer',
  templateUrl: './create-customer.component.html',
  styleUrls: ['./create-customer.component.scss']
})
export class CreateCustomerComponent implements OnInit {
  error: boolean = false;
  add: boolean = true;
  errorMessage: string = "";
  user: User;
  id: any;
  userId : string = "";

  customerForm: FormGroup;

  constructor(private router: Router, private route: ActivatedRoute, private userService: UserService, private toastr: ToastrService, private navbarService: NavbarService) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.user = new User();
       /* #region InputStyle */
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

    /* #endregion */
    //Init of the form
    this.customerForm = new FormGroup({
      'FirstName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'LastName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'Email': new FormControl(null, [Validators.required, Validators.maxLength(50), Validators.minLength(4), Validators.email]),
      'PhoneNumber': new FormControl(null, [Validators.required, Validators.maxLength(50)]),
      'CompanyName': new FormControl(null, [Validators.required, Validators.maxLength(50)]),
      'Country': new FormControl(null, [Validators.maxLength(50)]),
      'District': new FormControl(null, [Validators.maxLength(50)]),
      'Locality': new FormControl(null, [Validators.maxLength(50)]),
      'ZipCode': new FormControl(null, [Validators.pattern("^[0-9]*$")]),
      'Street': new FormControl(null, [Validators.maxLength(255)]),
      'Exist': new FormControl(null),
    });

    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = params['id'];
        }
      );
    if (this.id != undefined) {
      this.userService.getUser({ Id: this.id }).subscribe((response: User) => {
        this.add = false;
        
        this.customerForm.setValue({
          'FirstName': response.FirstName,
          'LastName': response.LastName,
          'Email': response.Email,
          'PhoneNumber': response.PhoneNumber,
          'CompanyName': response.CompanyName,
          'Country': response.Country,
          'District': response.District,
          'Locality': response.Locality,
          'ZipCode': response.ZipCode,
          'Street': response.Street,
          'Exist': true //Fix me when database mirgred
        });
        this.userId = response.Id
        /* #region InputStyle */
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

        /* #endregion */
      }, error => {
        console.log(error)
      });
    } else {
      this.customerForm.setValue({
        'FirstName': '',
        'LastName': '',
        'Email': '',
        'PhoneNumber': '',
        'CompanyName': '',
        'Country': '',
        'District': '',
        'Locality': '',
        'ZipCode': '',
        'Street': '',
        'Exist': true
      });
      
      if (history.state.data != null || history.state.data != undefined) {
        this.customerForm.controls['Email'].setValue(history.state.data.Email);
        (document.getElementById("email") as HTMLInputElement).value = history.state.data.Email;
      }
    }


    this.navbarService.updateProgressBar(false);
  }
  onSubmit() {
    this.user.Id = this.userId
    this.user.CompanyName = this.customerForm.controls["CompanyName"].value
    this.user.Country = this.customerForm.controls["Country"].value
    this.user.District = this.customerForm.controls["District"].value
    this.user.Email = this.customerForm.controls["Email"].value
    this.user.Exist = this.customerForm.controls["Exist"].value
    this.user.FirstName = this.customerForm.controls["FirstName"].value
    this.user.LastName = this.customerForm.controls["LastName"].value
    this.user.Locality = this.customerForm.controls["Locality"].value
    this.user.PhoneNumber = this.customerForm.controls["PhoneNumber"].value
    this.user.Street = this.customerForm.controls["Street"].value
    this.user.ZipCode = +this.customerForm.controls["ZipCode"].value

    if (this.add) {
      this.userService.AddCustomer(JSON.stringify(this.user)).subscribe((response: any) => {

      }, error => {
        console.log(error)
        this.error = true;
        for (const [key, value] of Object.entries(error.error)) {
          (value as Array<string>).forEach(element => {
            this.errorMessage += element + "<br>";
          });
        }
      });
    } else {
      this.userService.EditCustomer(JSON.stringify(this.user)).subscribe((response: any) => {
        console.log(response)
      }, error => {
        console.log(error)
        this.error = true;
        for (const [key, value] of Object.entries(error.error)) {
          (value as Array<string>).forEach(element => {
            this.errorMessage += element + "<br>";
          });
        }
      });
    }
  }
}
