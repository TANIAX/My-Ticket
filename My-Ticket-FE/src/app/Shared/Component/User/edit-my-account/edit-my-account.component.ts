import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Service/user.service';
import { User } from 'src/app/Shared/model/user.model';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { forbiddenNames } from 'src/app/Validator';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { StoredReply } from 'src/app/Shared/model/storedReply.model';
import { SelectionModel } from '@angular/cdk/collections';
import { StoredReplyService } from 'src/app/Shared/Service/storedReply.service';


@Component({
  selector: 'app-edit-my-account',
  templateUrl: './edit-my-account.component.html',
  styleUrls: ['./edit-my-account.component.scss']
})
export class EditMyAccountComponent implements OnInit {
  error: boolean = false;
  errorMessage: string = "";
  storedReplyError: boolean = false;
  storedReplyErrorMessage: string = "";
  user: User;
  FilteredStoredReply: string = "";
  password: string;
  date: string;
  isMemberOrAdmin: boolean = false;
  storedReplies: StoredReply[];
  storedReply: StoredReply;
  displayedColumns: string[] = ['select', 'Title', 'Reply', 'Id'];
  dataSource: MatTableDataSource<StoredReply>;
  selection = new SelectionModel<StoredReply>(true, []);
  position: number;
  CheckboxSelected: number = 0;
  iSCompanyCheckboxDisabled: boolean = false;
  userForm: FormGroup;
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
      ["link", "unlink"]
    ]
  };


  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private router: Router, private userService: UserService, private storedReplyService: StoredReplyService, private toastr: ToastrService, private navbarService: NavbarService) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.user = new User();
    this.storedReply = new StoredReply();
    //Init of the form
    this.userForm = new FormGroup({
      'FirstName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'LastName': new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20), forbiddenNames]),
      'PhoneNumber': new FormControl(null,[Validators.maxLength(50)]),
      'Birthdate': new FormControl(null),
      'Password': new FormControl(null, [Validators.minLength(6)]),
      'IsCompany': new FormControl(null),
      'CompanyName' : new FormControl(null,[Validators.maxLength(50)]),
      'Country': new FormControl(null,[Validators.maxLength(50)]),
      'District': new FormControl(null,[Validators.maxLength(50)]),
      'Locality': new FormControl(null,[Validators.maxLength(50)]),
      'ZipCode': new FormControl(null,[Validators.pattern("^[0-9]*$")]),
      'Street': new FormControl(null,[Validators.maxLength(255)]),
    });
    this.userService.getEditMyProfile().subscribe((response: any) => {
      this.user = response;
      this.storedReplies = response.StoredReplies;
      this.dataSource = new MatTableDataSource(this.storedReplies);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

      if (this.user.Birthdate != null)
        this.date = this.user.Birthdate.toString().substring(0, 10)

      if (response.NumberOfEmployee > 0)
        this.iSCompanyCheckboxDisabled = true;

      if (this.userService.roleMatch(['Member', 'Admin']))
        this.isMemberOrAdmin = true;

      this.userForm.setValue({
        'FirstName': this.user.FirstName,
        'LastName': this.user.LastName,
        'PhoneNumber': this.user.PhoneNumber,
        'Birthdate': this.date,
        'Password': '',
        'IsCompany': this.user.IsCompany,
        'CompanyName':this.user.CompanyName,
        'Country': this.user.Country,
        'District': this.user.District,
        'Locality': this.user.Locality,
        'ZipCode': this.user.ZipCode,
        'Street': this.user.Street,
      });

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

      document.getElementById('birthdate-control').classList.add("active");

      this.navbarService.updateProgressBar(false);
    }, error => {
      this.navbarService.updateProgressBar(false);
    })
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
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  masterToggle() {
    this.isAllSelected() ? this.selection.clear() : this.dataSource.data.forEach(row => this.selection.select(row));
    this.CheckboxSelected = this.selection.selected.length
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows
  }
  NumberRowSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected
  }
  checkboxLabel(row?: StoredReply): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
  LoadStoredReply() {
    let storedReplies = this.getElementsSelected(); // Should retrieve only 1 element
    this.storedReply = storedReplies[0];
  }
  UnloadStoredReply() {
    this.storedReply = null;
    this.storedReply = new StoredReply();
    this.storedReply.Id = 0;
    this.storedReply.Title = "";
    this.storedReply.Reply = "";
  }
  getElementsSelected() {
    return this.selection.selected;
  }
  SelectElement() {
    if (this.NumberRowSelected() == 1) {
      this.LoadStoredReply();
    } else {
      this.UnloadStoredReply();
    }
  }
  AddStoredReply() {
    this.navbarService.updateProgressBar(true);

    this.storedReplyError = false;
    this.storedReplyErrorMessage = "";

    this.storedReplyService.Create(this.storedReply).subscribe(response => {
      if (response == 0) {
        this.storedReplyError = true;
        this.storedReplyErrorMessage = "A stored reply with the same title and the same message already exist.";
      } else {
        this.storedReplies.push(this.storedReply)
        this.dataSource = new MatTableDataSource(this.storedReplies);
        this.storedReply.Id = +response;
        this.UnloadStoredReply();
      }
    }, error => {
      this.storedReplyError = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.storedReplyErrorMessage += element +"<br>";
        });
      }
    });

    this.navbarService.updateProgressBar(false);
  }
  EditStoredReply() {
    this.navbarService.updateProgressBar(true);

    this.storedReplyError = false;
    this.storedReplyErrorMessage = "";

    this.storedReplyService.Update(this.storedReply).subscribe(response => {
      if (response == 0) {
        this.storedReplyError = true;
        this.storedReplyErrorMessage = "A stored reply with the same title and the same message already exist.";
      } else {
        this.storedReplyError = false;
        this.storedReplyErrorMessage = "";
        this.UnloadStoredReply();
        this.selection.clear();
      }
    }, error => {
      this.storedReplyError = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element +"<br>";
        });
      }
    });

    this.navbarService.updateProgressBar(false);
  }
  DeleteStoredReply() {
    this.navbarService.updateProgressBar(true);

    let storedReplyIdList = [];

    this.storedReplyError = false;
    this.storedReplyErrorMessage = "";

    this.selection.selected.forEach(element => {
      storedReplyIdList.push(element.Id.toString());
    });

    this.storedReplyService.Delete(JSON.stringify({ storedReplyList: storedReplyIdList.toString() })).subscribe(response => {

      this.selection.selected.forEach(item => {
        let index: number = this.storedReplies.findIndex(d => d === item);
        this.storedReplies.splice(index, 1)
        this.dataSource = new MatTableDataSource<StoredReply>(this.storedReplies);
      });
      this.selection = new SelectionModel<StoredReply>(true, []);

    }, error => {
      this.storedReplyError = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element +"<br>";
        });
      }
    });

    this.navbarService.updateProgressBar(false);
  }

  onSubmit() {
    this.navbarService.updateProgressBar(true);
    this.error = false;
    this.errorMessage = "";

    this.userService.setEditMyProfile(JSON.stringify(this.userForm.value)).subscribe(response => {
      this.toastr.success("Your profil is updated", "", {
        timeOut: 3000
      })
    }, error => {
      this.error = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element + "<br>";
        });
      }
    })

    this.navbarService.updateProgressBar(false);
  }

}
