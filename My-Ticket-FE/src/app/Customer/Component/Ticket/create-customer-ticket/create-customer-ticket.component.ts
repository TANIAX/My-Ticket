import { Component, OnInit } from '@angular/core';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { TicketService } from 'src/app/Shared/Service/ticket.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-create-customer-ticket',
  templateUrl: './create-customer-ticket.component.html',
  styleUrls: ['./create-customer-ticket.component.scss']
})
export class CreateCustomerTicketComponent implements OnInit {
  ticketForm: FormGroup;
  error: boolean = false;
  errorMessage :string = "";
  editorConfig = {
    "editable": true,
    "spellcheck": true,
    "height": "auto",
    "minHeight": "300px",
    "width": "auto",
    "minWidth": "0",
    "translate": "yes",
    "enableToolbar": true,
    "showToolbar": true,
    "placeholder": "",
    "imageEndPoint": "https://localhost:44330/api/Upload",
    "toolbar": [
      ["bold", "italic", "underline"],
      ["fontName", "fontSize", "color"],
      ["paragraph", "blockquote", "removeBlockquote", "horizontalLine", "orderedList", "unorderedList"],
      ["link", "unlink"],
      ["link", "unlink", "image", "video"]
    ]
  };
  constructor(private navbarService : NavbarService, private ticketService : TicketService , private router : Router,private toastr: ToastrService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.ticketForm = new FormGroup({
      'Title': new FormControl(null, [Validators.required, Validators.minLength(3),Validators.maxLength(50)]),
      'Description': new FormControl(null, [Validators.required, Validators.minLength(25)])
    });

    this.ticketForm.setValue({
      'Title': '',
      'Description': localStorage.getItem("Signature"),
    });
    this.navbarService.updateProgressBar(false);
  }

  onSubmit(){
    this.navbarService.updateProgressBar(true);
    this.error = false;
    this.errorMessage = "";
    console.log(this.ticketForm);
    if(!this.ticketForm.valid)
      return;
    
      this.ticketService.CreateCustomer(this.ticketForm.getRawValue()).subscribe(response=>{
      this.toastr.success("Success","Creation success",{
        timeOut : 3000
      });
      this.router.navigate(['Ticket']);
    },error=>{
      this.error = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element +"<br>";
        });
      }
    });
    this.navbarService.updateProgressBar(false);
  }

}
