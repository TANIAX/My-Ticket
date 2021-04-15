import { Component, OnInit, Inject } from '@angular/core';
import { TicketService } from 'src/app/Shared/Service/ticket.service';
import { UserService } from 'src/app/Shared/Service/user.service';
import { User } from 'src/app/Shared/model/user.model';
import { TicketHeader } from 'src/app/Shared/model/ticketHeader.model';
import { ProjectService } from 'src/app/Shared/Service/project.service';
import { GroupService } from 'src/app/Shared/Service/group.service';
import { StatusService } from 'src/app/Shared/Service/status.service';
import { PriorityService } from 'src/app/Shared/Service/priority.service';
import { Project } from 'src/app/Shared/model/project.model';
import { Status } from 'src/app/Shared/model/status.model';
import { Priority } from 'src/app/Shared/model/priority.model';
import { Group } from 'src/app/Shared/model/group.model';
import { TouchSequence } from 'selenium-webdriver';
import { filter } from 'minimatch';
import { TypeService } from 'src/app/Shared/Service/type.service';
import { Type } from 'src/app/Shared/model/type.model';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-list-tickets',
  templateUrl: './list-tickets.component.html',
  styleUrls: ['./list-tickets.component.scss']
})

export class ListTicketsComponent implements OnInit {
  ItemPerPage: number = 10;
  FilteredGroupOrMember: string = "";
  isCollapse = true;
  setTimer: number = 300;
  timeLeft: number = 300;
  interval;
  p: any;

  Title: string = "All tickets";
  SortByString: string = "Created date";
  step: number = 1;
  GroupBoolean: boolean = false;

  User: User[] = [];
  Ticket: TicketHeader[] = [];
  SelectedElement: TicketHeader[] = [];
  Project: Project[] = [];
  Status: Status[] = [];
  Priority: Priority[] = [];
  Group: Group[] = [];
  Type: Type[] = [];
  filterForm: FormGroup;
  filter: object = null;

  error: boolean = false;
  errorMessage: string = "";


  constructor(private ticketService: TicketService,
    private userService: UserService,
    private projectService: ProjectService,
    private groupService: GroupService,
    private statusService: StatusService,
    private priorityService: PriorityService,
    private typeService: TypeService,
    private formBuilder: FormBuilder,
    private router: Router,
    private navbarService: NavbarService,
    private cookieService: CookieService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);

    this.filterForm = new FormGroup({
      'CreationDate': new FormControl(null),
      'Requester': new FormControl(null),
      'AssignTO': new FormControl(null),
      'Group': new FormControl(null),
      'Priority': new FormControl(null),
      'Type': new FormControl(null),
      'Project': new FormControl(null),
      'Status': new FormControl(null),
    });
    this.filterForm.setValue({
      'CreationDate': '',
      'Requester': '',
      'AssignTO': '',
      'Group': '',
      'Priority': '',
      'Type': '',
      'Project': '',
      'Status': '',
    });
    this.GetFilterInCookie();
    //#region Initiate the data
    this.userService.getMemberList().subscribe((response: User[]) => {
      this.User = response;
    }, error => {
      console.log(error)
    });
    this.projectService.GetProjectList().subscribe((response: Project[]) => {
      this.Project = response;
    }, error => {
      console.log(error);
    });
    this.statusService.GetStatusList().subscribe((response: Status[]) => {
      this.Status = response;
    }, error => {
      console.log(error);
    });
    this.priorityService.GetPriorityList().subscribe((response: Priority[]) => {
      this.Priority = response;
    }, error => {
      console.log(error);
    });
    this.groupService.GetGroupListWithNumberOfMember().subscribe((response: Group[]) => {
      this.Group = response;
    }, error => {
      console.log(error);
    });
    this.typeService.GetTypeList().subscribe((response: Type[]) => {
      this.Type = response;
    }, error => {
      console.log(error);
    });
    this.GetTicketList();
    //#endregion
    if (this.cookieService.get("Timer") !== null) {
      this.setTimer = +this.cookieService.get("Timer");
      this.timeLeft = this.setTimer;
    } else {
      this.cookieService.set("Timer", this.setTimer.toString());
    }

    this.startTimer();

    this.navbarService.updateProgressBar(false);
  }
  GroupClick(Event) {
    this.step = 1;
    Event.stopPropagation();
  }
  CustomerCreation(email: string){
    this.router.navigate(['/Staff/User/Create'], {state: {data: {"Email":email}}});
  }
  MemberClick(Event) {
    this.step = 2;
    Event.stopPropagation();
  }
  Search(Event) {
    Event.preventDefault();
  }
  ButtonSearch(Event) {
    Event.stopPropagation();
  }
  ChangeItemPerPage() {
    this.ItemPerPage = +(document.getElementById("item-per-page-control") as HTMLInputElement).value
    if (this.ItemPerPage < 1) {
      (document.getElementById("item-per-page-control") as HTMLInputElement).value = "1";
      this.ItemPerPage = 1;
    }
    else if (this.ItemPerPage > 30) {
      (document.getElementById("item-per-page-control") as HTMLInputElement).value = "30";
      this.ItemPerPage = 30;
    }
  }
  GetElementSelected() {
    this.SelectedElement = [];
    this.Ticket.forEach(element => {
      if (element.checkedOrUnchecked) {
        this.SelectedElement.push(element);
      }
    });
  }
  FilterPanelCollapse() {
    var container = document.getElementById("container")
    var filterPanel = document.getElementById("filter-panel")
    filterPanel.style.height = (container.offsetHeight + 105) + "px";

    if (this.isCollapse) {
      document.getElementById("filter-panel").style.width = "420px";
      document.getElementById("filter-panel-button").style.marginRight = "420px";
    } else {
      document.getElementById("filter-panel").style.width = "0px";
      document.getElementById("filter-panel-button").style.marginRight = "0px";
    }
    this.isCollapse = !this.isCollapse;
  }
  filterFormSubmit() {
    this.navbarService.updateProgressBar(true);
    this.error = false;
    this.errorMessage = "";

    this.cookieService.set("filter",
      "AssignTO=" + this.filterForm.controls['AssignTO'].value +
      ";Group=" + this.filterForm.controls['Group'].value +
      ";Priority=" + this.filterForm.controls['Priority'].value +
      ";Type=" + +this.filterForm.controls['Type'].value +
      ";Project=" + this.filterForm.controls['Project'].value +
      ";Status=" + this.filterForm.controls['Status'].value +
      ";CreationDate=" + this.filterForm.controls['CreationDate'].value);
    this.navbarService.updateProgressBar(false);
    this.GetTicketList();
  }
  OpenButton() {
    this.GetElementSelected();
    this.router.navigate(['/Ticket/' + this.SelectedElement[0].Id])
  }
  CloseButton() {
    this.navbarService.updateProgressBar(true)
    this.error = false;
    this.errorMessage = "";

    this.ticketService.Close(JSON.stringify({ TicketList: this.SelectedElement })).subscribe(response => {
      this.SelectedElement.forEach(element => {
        element.Status = this.Status.find(x => x.Name == "Closed");
      });
    }, error => {
      this.error = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element + "<br>";
        });
      }
    })

    //Update all selected ticket
    this.SelectedElement.forEach(element => {
      this.RefreshTicketRow(element)
    });

    //Uncheck them all
    this.UncheckAllTicket();
    this.navbarService.updateProgressBar(false);
  }
  AssignToButton(me: string = "", user: User) {
    this.navbarService.updateProgressBar(true);
    var currentUser: User = null;
    this.error = false;
    this.errorMessage = "";


    this.SelectedElement.forEach(element => {
      this.ticketService.GetTicket(element.Id).subscribe((response: TicketHeader) => {
        if (this.checkConcurency(element, response)) {
          if (me != "") {
            currentUser = this.User.find(x => x.Email == localStorage.getItem("Username"));
            response.AssignTO = currentUser;
          } else {
            response.AssignTO = user;
          }

          this.ticketService.UpdateTickets(response).subscribe(response => {
            if (me != "") {
              this.Ticket.find(x => x.Id == element.Id).AssignTO = currentUser
            } else {
              this.Ticket.find(x => x.Id == element.Id).AssignTO = user
            }

          }, error => {
            this.error = true;
            for (const [key, value] of Object.entries(error.error)) {
              (value as Array<string>).forEach(element => {
                this.errorMessage += element + "<br>";
              });
            }
          });
        } else {
          //CONCURENCY EXCEPTION
          this.RefreshTicketRow(element);
          document.getElementById("ticket-" + element.Id).style.backgroundColor = "#ffb6b4";
        }
      }, error => {
        //Cannot retrieve the original ticket
        this.error = true;
        for (const [key, value] of Object.entries(error.error)) {
          (value as Array<string>).forEach(element => {
            this.errorMessage += element + "<br>";
          });
        }
      });
    });
    this.UncheckAllTicket();
    this.navbarService.updateProgressBar(false);
  }
  checkConcurency(TicketHeader1: any, TicketHeader2: any): boolean {
    if (TicketHeader1.AssignTO === null) {
      TicketHeader1.AssignTO = { Id: "", LastName: null, FirstName: null, Email: null, PP: null }
    }
    if (TicketHeader2.AssignTO === null) {
      TicketHeader2.AssignTO = { Id: "", LastName: null, FirstName: null, Email: null, PP: null }
    }

    if (TicketHeader1.Requester === null) {
      TicketHeader1.Requester = { Id: "", LastName: null, FirstName: null, Email: null, PP: null }
    }
    if (TicketHeader2.Requester === null) {
      TicketHeader2.Requester = { Id: "", LastName: null, FirstName: null, Email: null, PP: null }
    }

    if (TicketHeader1.AssignTO.Id === TicketHeader2.AssignTO.Id) {
      if (TicketHeader1.Priority.Id === TicketHeader2.Priority.Id) {
        if (TicketHeader1.Project.Id === TicketHeader2.Project.Id) {
          if (TicketHeader1.Requester.Id === TicketHeader2.Requester.Id) {
            if (TicketHeader1.Status.Id === TicketHeader2.Status.Id) {
              if (TicketHeader1.Title === TicketHeader2.Title) {
                if (TicketHeader1.Type.Id === TicketHeader2.Type.Id) {
                  if (TicketHeader1.ClosedDate === TicketHeader2.ClosedDate) {
                    if (TicketHeader1.CreationDate === TicketHeader2.CreationDate) {
                      if (TicketHeader1.Description === TicketHeader2.Description) {
                        if (TicketHeader1.Email === TicketHeader2.Email) {
                          if (TicketHeader1.Id === TicketHeader2.Id) {
                            return true;
                          } else {
                            console.log(1)
                            return false;
                          }
                        } else {
                          console.log(2)
                          return false;
                        }
                      } else {
                        console.log(3)
                        return false;
                      }
                    } else {
                      console.log(4)
                      return false;
                    }
                  } else {
                    console.log(5)
                    return false;
                  }
                } else {
                  console.log(6)
                  return false;
                }
              } else {
                console.log(7)
                return false;
              }
            } else {
              console.log(8)
              return false;
            }
          } else {
            console.log(9)
            return false;
          }
        } else {
          console.log(10)
          return false;
        }
      } else {
        console.log(11)
        return false;
      }
    } else {
      console.log(12)
      return false;
    }
  }
  startTimer() {
    this.interval = setInterval(() => {
      if (this.setTimer > 0) {
        if (this.timeLeft > 0) {
          this.timeLeft--;
        } else {
          this.GetTicketList();
          this.timeLeft = this.setTimer;
        }
      } else {
        this.timeLeft = -1
      }
    }, 1000)
  }
  GetTicketList() {
    if (this.Ticket.length !== 0) {
      this.timeLeft = this.setTimer;
    }
    this.filter = {
      "CreationDate": +this.filterForm.controls['CreationDate'].value,
      "Requester": this.filterForm.controls['Requester'].value,
      "AssignTO": this.filterForm.controls['AssignTO'].value,
      "Group": +this.filterForm.controls['Group'].value,
      "Priority": +this.filterForm.controls['Priority'].value,
      "Type": +this.filterForm.controls['Type'].value,
      "Project": +this.filterForm.controls['Project'].value,
      "Status": +this.filterForm.controls['Status'].value
    }
    console.log(this.filter);
    this.ticketService.GetTicketList(this.filter).subscribe((response: TicketHeader[]) => {
      this.Ticket = response;
      console.log(this.Ticket)
    }, error => {
      console.log(error);
    });
  }
  SetTimerInCookie() {
    if (this.setTimer < 29) {
      this.setTimer = -1;
    }
    this.cookieService.set("Timer", this.setTimer.toString());
  }
  Delete(reason : string = ""){
    this.navbarService.updateProgressBar(true);
    this.error = false;
    this.errorMessage = "";

    let ticketSelected = [];

    this.SelectedElement.forEach(element => {
      ticketSelected.push(element.Id.toString());
    });

    this.ticketService.Delete(JSON.stringify({ TicketList: ticketSelected.toString(),Reason : reason})).subscribe(response => {
      this.SelectedElement.forEach(element => {
        for (var i = 0; i < this.Ticket.length; i++)
          if (this.Ticket[i].Id === element.Id) {
            this.Ticket.splice(i, 1);
            break;
          }
      });
      this.GetElementSelected();
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
  DeleteButton() {
    this.openDialog();
  }
  PutTicket(ticket: TicketHeader, data: any, type: string) {
    this.navbarService.updateProgressBar(true);
    this.error = false;
    this.errorMessage = "";

    this.ticketService.GetTicket(ticket.Id).subscribe((response: TicketHeader) => {
      if (this.checkConcurency(ticket, response)) {
        switch (type) {
          case "Priority":
            response.Priority = data;
            break;
          case "Project":
            response.Project = data;
            break;
          case "Group":
            response.Group = data;
            break;
          case "AssignTO":
            response.AssignTO = data;
            break;
          case "Status":
            response.Status = data;
            break;
        }
        this.ticketService.UpdateTickets(response).subscribe(response => {
          switch (type) {
            case "Priority":
              this.Ticket.find(x => x.Id == ticket.Id).Priority = data;
              break;
            case "Project":
              this.Ticket.find(x => x.Id == ticket.Id).Project = data;
              break;
            case "Group":
              this.Ticket.find(x => x.Id == ticket.Id).Group = data;
              break;
            case "AssignTO":
              this.Ticket.find(x => x.Id == ticket.Id).AssignTO = data;
              break;
            case "Status":
              this.Ticket.find(x => x.Id == ticket.Id).Status = data;
              break;
          }
        }, error => {
          this.error = true;
          for (const [key, value] of Object.entries(error.error)) {
            (value as Array<string>).forEach(element => {
              this.errorMessage += element + "<br>";
            });
          }
        });
      } else {
        //CONCURENCY EXCEPTION
        this.RefreshTicketRow(ticket);
        document.getElementById("ticket-" + ticket.Id).style.backgroundColor = "#ffb6b4";

      }
    }, error => {
      //CANNOT RETRIEVE ORINAL TICKER
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element + "<br>";
        });
      }
    });
    this.navbarService.updateProgressBar(false);
  }
  GetFilterInCookie() {
    let filterString: string = "";
    let splitedString: string[] = [];
    let splitedKeyValue: string[] = [];

    filterString = this.cookieService.get("filter");
    splitedString = filterString.split(';');
    splitedString.forEach(element => {
      splitedKeyValue = [];
      splitedKeyValue = element.split('=');
      switch (splitedKeyValue[0]) {
        case "AssignTO":
          this.filterForm.controls["AssignTO"].setValue(splitedKeyValue[1])
          break;
        case "Group":
          this.filterForm.controls["Group"].setValue(+splitedKeyValue[1])
          break;
        case "Priority":
          this.filterForm.controls["Priority"].setValue(+splitedKeyValue[1])
          break;
        case "Type":
          this.filterForm.controls["Type"].setValue(+splitedKeyValue[1])
          break;
        case "Project":
          this.filterForm.controls["Project"].setValue(+splitedKeyValue[1])
          break;
        case "Status":
          this.filterForm.controls["Status"].setValue(+splitedKeyValue[1])
          break;
        case "CreationDate":
          this.filterForm.controls["CreationDate"].setValue(+splitedKeyValue[1])
          break;
      }
    });
  }
  async RefreshTicketRow(ticket) {
    setTimeout(() => {
      this.ticketService.GetTicket(ticket.Id).subscribe((response: TicketHeader) => {
        let index = this.Ticket.findIndex(x => x.Id == ticket.Id);
        this.Ticket.splice(index, 1, response);
      });
    }, 2000);
  }
  UncheckAllTicket() {
    this.SelectedElement = [];
    this.Ticket.forEach(element => {
      element.checkedOrUnchecked = false
    });
  }
  
  openDialog() {
    const dialogRef = this.dialog.open(DialogDeleteReason);

    dialogRef.afterClosed().subscribe(result => {
      
      if(result != "")
        
        this.Delete(result);
      else
        alert("You must enter a reason.");
    });
  }
}

export interface IFilter {
  CreationDate: number;
  Requester: User;
  AssignTo: User;
  Group: Group;
  Priority: Priority;
  Type: Type;
  Project: Project;
  Status: Status;
}

@Component({
  selector: 'Dialog-Delete-Reason',
  templateUrl: 'Dialog-Delete-Reason.html',
})
export class DialogDeleteReason {
  deleteReason : string = "";
  constructor(
    public dialogRef: MatDialogRef<DialogDeleteReason>,
    @Inject(MAT_DIALOG_DATA) public data: User) { }

  onValidClick() {
    this.dialogRef.close(this.deleteReason);
  }
}
