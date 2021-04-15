import { Component, OnInit } from '@angular/core';
import { NgxEditorModule } from 'ngx-editor';
import { FormGroup, FormControl } from '@angular/forms';
import { UserService } from 'src/app/Shared/Service/user.service';
import { User } from 'src/app/Shared/model/user.model';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { Router } from '@angular/router';
import { NavbarComponent } from 'src/app/Shared/Component/Layout/navbar/navbar.component';
import { AlifeFileToBase64Module } from 'alife-file-to-base64';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  htmlContent: string = "";
  form: FormGroup;
  user: User
  currentUserIsMember: boolean = false;
  editorConfig = {
    "editable": true,
    "spellcheck": true,
    "height": "auto",
    "minHeight": "150px",
    "width": "auto",
    "minWidth": "0",
    "translate": "yes",
    "enableToolbar": true,
    "showToolbar": true,
    "placeholder": "Enter your signature here...",
    "imageEndPoint": "https://localhost:44330/api/Upload",
    "toolbar": [
      ["bold", "italic", "underline"],
      ["fontName", "fontSize", "color"],
      ["paragraph", "blockquote", "removeBlockquote", "horizontalLine", "orderedList", "unorderedList"],
      ["link", ,"image","unlink"]
    ]
  };
  constructor(private userService: UserService, private toastr: ToastrService, private navbarService: NavbarService, private navbar: NavbarComponent, private router: Router) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);

    this.userService.getMyAccount().subscribe((response: any) => {
      this.user = response;
      console.log(this.user)
      if (this.userService.roleMatch(['Member'])) {
        this.currentUserIsMember = true;
      }
    }, error => {

    });
    this.form = new FormGroup({
      'Language': new FormControl(),
      'Signature': new FormControl(),
      'imageData': new FormControl()
    });
    //We set default value to avoid error
    this.form.setValue({
      'Language': '',
      'Signature': '',
      'imageData': ''
    });

    this.navbarService.updateProgressBar(false);
  }
  onSubmit() {
    this.navbarService.updateProgressBar(true);
    this.userService.updateMyAccount(JSON.stringify(this.form.value)).subscribe(response => {
      this.toastr.success("Update success", "", {
        timeOut: 3000,
      })
    }, error => {
      this.toastr.error("Update error", error.error.error, {
        timeOut: 3000
      });
    });

    this.navbarService.updateProgressBar(false);
  }
  fileChange(event) {
    var reader = new FileReader();
    reader.onload = (event: any) => {
      this.user.PP = event.target.result;
      this.setImageToForm(event.target.result.split(',')[1]);
    }
    reader.readAsDataURL(event.target.files[0]);
  }
  setImageToForm(value){
    this.form.controls['imageData'].setValue(value);
  }
  
}
