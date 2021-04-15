import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { TicketHeader } from 'src/app/Shared/model/ticketHeader.model';
import { TicketLine } from 'src/app/Shared/model/ticketLine.model';
import { TicketService } from 'src/app/Shared/Service/ticket.service';
import { User } from 'src/app/Shared/model/user.model';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { ToastrService } from 'ngx-toastr';
import { NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { datepickerAnimation } from 'ngx-bootstrap/datepicker/datepicker-animations';
import { UserService } from 'src/app/Shared/Service/user.service';
import { Status } from 'src/app/Shared/model/status.model';
import { StatusService } from 'src/app/Shared/Service/status.service';
import { Satisfaction } from 'src/app/Shared/model/satisfaction.model';
import { SatisfactionService } from 'src/app/Shared/Service/satisfaction.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent implements OnInit {
  Id: number;
  FilteredStoredReply : string = "";
  ReplyForm: FormGroup;
  Ticket: TicketHeader;
  TicketLines: TicketLine[];
  Satisfaction : Satisfaction[];
  Status : Status[];
  CurrentUser: User;
  CurrentUserMemberOrAdmin: boolean = false;
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
  constructor(private userService: UserService,
    private toastr: ToastrService,
    private navbarService: NavbarService,
    private route: ActivatedRoute,
    private router: Router,
    private ticketService: TicketService,
    private statusService : StatusService,
    private satisfactionService : SatisfactionService) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.statusService.GetStatusList().subscribe((response:Status[])=>{
      this.Status = response;
    }, error =>{
      console.log(error)
    });
    this.route.params
      .subscribe(
        (params: Params) => {
          this.Id = params['id'];
        }
      );
    if (this.Id != 0 || this.Id != null) {
      this.ticketService.GetTicket(this.Id).subscribe((response: TicketHeader) => {
        this.Ticket = response;
        console.log(this.Ticket)
      }, error => {
        this.navbarService.updateProgressBar(false);
        this.toastr.error("error", error.error, {
          timeOut: 3000
        })
        this.router.navigate(['/']);
      });
    }
    this.userService.getCurrentUser().subscribe((response: User) => {
      console.log(response)
      this.CurrentUser = new User();
      this.CurrentUser.Email = response.Email;
      this.CurrentUser.PP = response.PP;
      this.CurrentUser.Signature = response.Signature;
      this.CurrentUser.StoredReply = response.StoredReply;
      if (this.userService.roleMatch(['Admin', 'Member']))
        this.CurrentUserMemberOrAdmin = true;
    }, error => {
      console.log(error)
    });
    if (this.userService.roleMatch(['Admin', 'Member']))
      this.CurrentUserMemberOrAdmin = true;

    this.satisfactionService.GetSatisfactionList().subscribe((response:Satisfaction[])=>{
      this.Satisfaction = response;
    },error=>{
      console.log(error);
    });

    this.ReplyForm = new FormGroup({
      'TicketHeaderId': new FormControl(null, [Validators.required]),
      'Response': new FormControl(null, [Validators.required, Validators.minLength(25)]),
      'AskForClose': new FormControl(null),
    });
    this.ReplyForm.setValue({
      'TicketHeaderId': this.Id,
      'Response': localStorage.getItem('Signature'),
      'AskForClose': false
    });
    this.navbarService.updateProgressBar(false);
  }

  onSubmit() {
    let tl = new TicketLine()
    this.navbarService.updateProgressBar(true);
    const value = {
      "TicketHeaderId" : +this.ReplyForm.get(["TicketHeaderId"]).value,
      "AskForClose" : this.ReplyForm.get(["AskForClose"]).value,
      "Response" : this.ReplyForm.get(["Response"]).value
    }
    this.ticketService.Reply(value).subscribe(response => {
      if(this.CurrentUserMemberOrAdmin){
        this.Ticket.Status = this.Status.find(x=>x.Name == "Waiting on customer");
      }else{
        this.Ticket.Status = this.Status.find(x=>x.Name == "Open");
      }
      
      if (this.ReplyForm.controls['AskForClose'].value == false) {
        var object: any = {
          'Content': this.ReplyForm.controls['Response'].value,
          'AskForClose': this.ReplyForm.controls['AskForClose'].value,
          'ResponseBy': {
            'LastName': this.CurrentUser.LastName,
            'FirstName': this.CurrentUser.FirstName,
            'Email': this.CurrentUser.Email,
            'PP': this.CurrentUser.PP
          },
          ResponseDate: new Date()
        }
        this.Ticket.TicketLines.push(object);
        this.ReplyForm.controls['Response'].setValue("");
      }
      else {
        this.Ticket.ClosedDate = new Date();
        this.Ticket.Status.Id = 4;
        this.Ticket.Status.Name = 'Closed'
      }
    }, error => {
      console.log(error)
      this.toastr.error("Error", error.error, {
        timeOut: 30000
      });
      this.navbarService.updateProgressBar(false);
    });

    this.navbarService.updateProgressBar(false);
  }
  Close() {
    this.ReplyForm.setValue({
      'TicketHeaderId': this.Ticket.Id,
      'AskForClose': true,
      'Response': ''
    });
    this.onSubmit();
  }

  SetSatisfaction(satisfaction: number) {
    switch(satisfaction){
      case 1:
        this.Ticket.Satisfaction = this.Satisfaction.find(x=>x.Name == "Hight");
        break;
      case 2:
          this.Ticket.Satisfaction = this.Satisfaction.find(x=>x.Name == "Mid");
        break;
      case 3:
          this.Ticket.Satisfaction = this.Satisfaction.find(x=>x.Name == "Low");
        break;
    }
    const value = {
      "TicketId" : this.Ticket.Id,
      "SatisfactionId" : this.Ticket.Satisfaction.Id
    }
    this.ticketService.setSatisfaction(value).subscribe(response =>{
      //do nothing
    },error=>{
      console.log(error);
    })
  }
  Reply() {
    document.getElementById("scrollTo").scrollIntoView();
  }
  Search(Event) {
    Event.preventDefault();
  }
  SelectStoredReply(reply : any){
    let CurrentReplyContent = this.ReplyForm.controls['Response'].value
    this.ReplyForm.controls['Response'].setValue("");
    this.ReplyForm.controls['Response'].setValue(reply.Reply + "<br>" + this.CurrentUser.Signature);
  }
  GoToTicketList(){
    if(this.CurrentUserMemberOrAdmin)
      this.router.navigate(["Staff/Ticket"])
    else
      this.router.navigate(["Ticket"])
  }
}

