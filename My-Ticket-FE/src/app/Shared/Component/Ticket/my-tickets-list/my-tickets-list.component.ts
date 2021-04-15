import { Component, OnInit, ViewChild } from '@angular/core';
import { TicketHeader } from 'src/app/Shared/model/ticketHeader.model';
import { TicketService } from 'src/app/Shared/Service/ticket.service';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { Router } from '@angular/router';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { Status } from 'src/app/Shared/model/status.model';


@Component({
  selector: 'app-my-tickets-list',
  templateUrl: './my-tickets-list.component.html',
  styleUrls: ['./my-tickets-list.component.scss']
})
export class MyTicketsListComponent implements OnInit {
  displayedColumns: string[] = ['select','Project','Title','CreationDate','Type','Status','Id'];
  dataSource: MatTableDataSource<TicketHeader>;
  selection = new SelectionModel<TicketHeader>(true, []);
  Tickets: TicketHeader[];
  Ticket: TicketHeader;
  position: number;
  CheckboxSelected: number = 0;
  error: boolean = false;
  errorMessage : string  = "";
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('table', { static: true }) table;
  constructor(private ticketService: TicketService, private toastr: ToastrService, private navbarService: NavbarService, private router: Router) { }
  ngOnInit() {
    this.navbarService.updateProgressBar(true);

    this.error = false;
    this.errorMessage = "";

    const value = {
      "CreationDate": 0,
      "Requester": "",
      "AssignTO": "",
      "Group": 0,
      "Priority": 0,
      "Type": 0,
      "Project": 0,
      "Status": 0
    }

    this.dataSource = new MatTableDataSource(this.Tickets);
    this.ticketService.GetTicketList(value).subscribe((response: any) => {
      this.Tickets = response;
      this.dataSource = new MatTableDataSource(this.Tickets);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      
    }, error => {
      this.error = true;
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element +"<br>";
        });
      }
    });
    this.navbarService.updateProgressBar(false);
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ? this.selection.clear() : this.dataSource.data.forEach(row => this.selection.select(row));
    this.CheckboxSelected = this.selection.selected.length
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: TicketHeader): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows
  }


  SelectCheckbox(event: any) {
    if (event.checked) {
      this.CheckboxSelected++; 
    }
    else {
      this.CheckboxSelected--;
    }
  }
  getElementsSelected() {
    return this.selection.selected;
  }
  Open() {
    let ticketHeader = this.getElementsSelected(); // Should retrieve only 1 element
    this.router.navigate(['/Ticket/'+ticketHeader[0].Id]);
  }

  Close(){
    this.navbarService.updateProgressBar(true);

    this.error = false;
    this.errorMessage = "";

    let ticketHeaders = this.getElementsSelected();
    this.ticketService.Close({TicketList:ticketHeaders}).subscribe( response =>{
      let status = new Status();
      status.Name = "Closed";
      ticketHeaders.forEach(element => {
        (element.Status as any) = status;
      });
      this.table.renderRows();
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
  Delete(){
    this.navbarService.updateProgressBar(true);

    let TicketIdList = [];

    this.error = false;
    this.errorMessage = "";

    this.selection.selected.forEach(element => {
      TicketIdList.push(element.Id.toString());
    });

    this.ticketService.Delete(JSON.stringify({ TicketList: TicketIdList.toString() })).subscribe(response => {

      this.selection.selected.forEach(item => {
        let index: number = this.Tickets.findIndex(d => d === item);
        this.Tickets.splice(index, 1)
        this.dataSource = new MatTableDataSource<TicketHeader>(this.Tickets);
      });
      this.selection = new SelectionModel<TicketHeader>(true, []);

    }, error => {
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
