import { Component, OnInit } from '@angular/core';
import { TicketService } from 'src/app/Shared/Service/ticket.service';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';
import { Project } from 'src/app/Shared/model/project.model';
import { TicketType } from 'src/app/Shared/model/ticketType.model';
import { Status } from 'src/app/Shared/model/status.model';
import { FormsModule } from '@angular/forms';
import { Priority } from 'src/app/Shared/model/priority.model';
import { NgxEditorModule } from 'ngx-editor';
import { TicketHeader } from 'src/app/Shared/model/ticketHeader.model';
import { HttpErrorResponse } from '@angular/common/http';
import { User } from 'src/app/Shared/model/user.model';
import { Group } from 'src/app/Shared/model/group.model';

@Component({
  selector: 'app-create-staff-ticket',
  templateUrl: './create-staff-ticket.component.html',
  styleUrls: ['./create-staff-ticket.component.scss']
})
export class CreateStaffTicketComponent implements OnInit {
  ticketForm: FormGroup;
  Project: Project[];
  Type: TicketType[];
  Status: Status[];
  Priority: Priority[];
  Group: Group[] = [];
  AssignTO: User[] = [];
  Requester: User[];

  FilteredGroupOrMember: string = "";
  FilteredRequester: string = "";
  step: number = 1;
  groupSelected: Group;
  memberSelected: User;
  requesterSelected: User;

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
    "placeholder": "Enter the description of your type here...",
    "imageEndPoint": "https://localhost:44330/api/Upload",
    "toolbar": [
      ["bold", "italic", "underline"],
      ["fontName", "fontSize", "color"],
      ["paragraph", "blockquote", "removeBlockquote", "horizontalLine", "orderedList", "unorderedList"],
      ["link", "unlink"],
      ["link", "unlink", "image", "video"]
    ]
  };
  constructor(private ticketService: TicketService, private navbarService: NavbarService,
    private toastr: ToastrService, private router: Router, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.ticketForm = new FormGroup({
      'Project': new FormControl(null, [Validators.required, Validators.min(1)]),
      'Type': new FormControl(null, [Validators.required, Validators.min(1)]),
      'Priority': new FormControl(null, [Validators.required, Validators.min(1)]),
      'Title': new FormControl(null, [Validators.required, Validators.minLength(5)]),
      'Group': new FormControl(null),
      'AssignTO': new FormControl(null),
      'Requester': new FormControl(null,[Validators.required]),
      'Description': new FormControl(null, [Validators.required, Validators.minLength(25)]),
      'CloseImmediatly': new FormControl(null),
    });
    this.ResetFom();
    this.ticketForm.controls['Project'].setValue(0);
    this.ticketForm.controls['Type'].setValue(0);
    this.ticketForm.controls['Priority'].setValue(0);
    this.ticketForm.controls['Requester'].setValue("");
    this.ticketForm.controls['AssignTO'].setValue("");


    this.ticketService.GenerateTicketHeader().subscribe((response: any) => {
      this.Project = response.Projects;
      this.Type = response.Types;
      this.Status = response.Status;
      this.Priority = response.Priorities;
      this.Group = response.Groups;
      this.AssignTO = response.Members;
      this.Requester = response.Customers;
    }, error => {
      if (error.status === 0)
        this.router.navigate(['500']);
      console.log(error)
    })
    
    this.navbarService.updateProgressBar(false);
  }
  ResetFom(){
    this.ticketForm.setValue({
      'Project': 0,
      'Type': 0,
      'Priority': 0,
      'Title': '',
      'Group': 0,
      'AssignTO': '',
      'Requester': '',
      'Description': '',
      'CloseImmediatly': false,
    });
  }
  onSubmit() {
    if(!this.ticketForm.valid)
      return

    this.navbarService.updateProgressBar(true);

    const value = {
      "Project": +this.ticketForm.controls['Project'].value,
      "Type": +this.ticketForm.controls['Type'].value,
      "Priority": +this.ticketForm.controls['Priority'].value,
      "Title": this.ticketForm.controls['Title'].value,
      "Group": +this.ticketForm.controls['Group'].value,
      "AssignTO": this.ticketForm.controls['AssignTO'].value,
      "Requester": this.ticketForm.controls['Requester'].value,
      "Description": this.ticketForm.controls['Description'].value,
      "CloseImmediatly": this.ticketForm.controls['CloseImmediatly'].value
    }
    
    this.ticketService.CreateStaff(value).subscribe(response => {
      this.toastr.success("Success", "", {
        timeOut: 30000
      });
      this.ResetFom();
    }, error => {
      console.log(error)
      this.toastr.error("Error", error.error, {
        timeOut: 30000
      });
    });
    this.navbarService.updateProgressBar(false);
  }

  GroupClick(Event) {
    this.step = 1;
    Event.stopPropagation();
  }
  AssignTOClick(Event) {
    this.step = 2;
    Event.stopPropagation();
  }
  Search(Event) {
    Event.preventDefault();
  }
  ButtonSearch(Event) {
    Event.stopPropagation();
  }
  SelectAssignTo(user: User) {
    this.memberSelected = user;
    this.ticketForm.controls['AssignTO'].setValue(user.Id);
  }
  SelectGroup(group: Group) {
    this.groupSelected = group;
    this.ticketForm.controls['Group'].setValue(group.Id);
  }
  SelectRequester(user: User) {
    this.requesterSelected = user;
    this.ticketForm.controls['Requester'].setValue(user.Id);
  }
}
