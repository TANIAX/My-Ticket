import { Component, OnInit, ViewChild, AfterViewInit, Inject, ɵConsole } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UserService } from 'src/app/Shared/Service/user.service';
import { User } from 'src/app/Shared/model/user.model';
import { ToastrService } from 'ngx-toastr';
import { NavbarService } from 'src/app/Shared/Service/navbar.service';
import { Router } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit, AfterViewInit {
  error: boolean = false;
  errorMessage : string = "";
  edit:boolean;
  displayedColumns: string[] = ['select', 'Email', 'LastName', 'FirstName', 'PhoneNumber', 'Id'];
  dataSource: MatTableDataSource<User>;
  selection = new SelectionModel<User>(true, []);
  employees: User[];
  employee: User;
  position: number;
  width: number;
  ShowDiv: boolean = false;
  CheckboxSelected: number = 0;
  AddElement: HTMLInputElement;
  EditElement: HTMLInputElement;
  DeleteElement: HTMLInputElement;
  animal: string;
  name: string;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(public dialog: MatDialog, private userService: UserService, private user: User, private toastr: ToastrService, private navbarService: NavbarService, private router: Router) {

  }

  ngOnInit() {
    this.navbarService.updateProgressBar(true);
    this.employee = new User();
    this.dataSource = new MatTableDataSource(this.employees);
    this.userService.GetMyEmployees().subscribe((response: any) => {
      this.employees = response.EmployeeList;
      this.dataSource = new MatTableDataSource(this.employees);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.navbarService.updateProgressBar(false);
    }, error => {
      this.navbarService.updateProgressBar(false);
      //TODO REDIRECT TO FIRBIDEN PAGE
      this.toastr.error("Page could not be loaded", "", {
        timeOut: 3000
      })
      this.router.navigate(['/']);
    });
    this.width = document.body.clientWidth;
  }
  masterToggle() {
    this.isAllSelected() ? this.selection.clear() : this.dataSource.data.forEach(row => this.selection.select(row));
    this.CheckboxSelected = this.selection.selected.length
  }
  checkboxLabel(row?: User): string {
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
      if (this.CheckboxSelected == 1)
        this.LoadEmployee();
    }
    else {
      this.CheckboxSelected--;
    }

    if (this.ShowDiv && this.CheckboxSelected > 1) {
      this.ShowDiv = false;
    }
    if (!this.AddElement.disabled && this.CheckboxSelected >= 1) {
      this.ShowDiv = false;
    }
    if (this.EditElement.disabled) {
      this.ShowDiv = false;
    }
  }
  AddEmployee() {
    //TODO this is a test
    var user = new User()
    this.employees.push(user)
    this.dataSource = new MatTableDataSource(this.employees);
  }
  LoadEmployee() {
    let employees = this.getElementsSelected(); // Should retrieve only 1 element
    this.employee = employees[0];
  }
  Add() {
    this.edit = false;
    this.UnloadEmployee();
    this.GetScreenWitdh();
    this.employee.Id = "";
    if (this.width <= 1200)
      this.openDialog();
    else
      this.ShowDiv = !this.ShowDiv;
  }
  Edit() {
    this.edit = true;
    this.LoadEmployee();
    this.GetScreenWitdh();
    if (this.width <= 1200)
      this.openDialog();
    else
      this.ShowDiv = !this.ShowDiv;
  }
  Delete() {
    this.navbarService.updateProgressBar(true);

    this.edit = false;

    let employeeIdList = [];

    this.error = false;
    this.errorMessage = "";

    this.selection.selected.forEach(element => {
      employeeIdList.push(element.Id.toString());
    });

    this.userService.DeleteEmployee(JSON.stringify({ EmployeeList: employeeIdList.toString() })).subscribe(response => {

      this.selection.selected.forEach(item => {
        let index: number = this.employees.findIndex(d => d === item);
        this.employees.splice(index, 1)
        this.dataSource = new MatTableDataSource<User>(this.employees);
      });
      this.selection = new SelectionModel<User>(true, []);
      this.CheckboxSelected = 0;

    }, error => {
      for (const [key, value] of Object.entries(error.error)) {
        (value as Array<string>).forEach(element => {
          this.errorMessage += element +"<br>";
        });
      }
    });

    this.navbarService.updateProgressBar(false);
  }
  ResetPassword(){
    this.navbarService.updateProgressBar(true);

    this.userService.ResetEmployeePassword(JSON.stringify(this.employee)).subscribe(response =>{
      this.toastr.success("Password has been reset","Success",{
        timeOut:3000
      });
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
  getElementsSelected() {
    return this.selection.selected;
  }
  ngAfterViewInit(): void {
    this.AddElement = document.getElementById("AddElement") as HTMLInputElement;
    this.EditElement = document.getElementById("AddElement") as HTMLInputElement;
    this.DeleteElement = document.getElementById("AddElement") as HTMLInputElement;
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DialogCRUDEmployee, {
      width: '350px',
      data: this.employee
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == "Valid") {
        this.ValidButton();
      }
    });
  }
  UnloadEmployee() {
    this.employee = null;
    this.employee = new User();
    this.employee.Id = "";
    this.employee.Email = "";
    this.employee.LastName = "";
    this.employee.FirstName = "";
    this.employee.PhoneNumber = "";
  }
  GetScreenWitdh() {
    this.width = document.body.clientWidth;
  }
  ValidButton() {
    this.navbarService.updateProgressBar(true)

    this.error= false;
    this.errorMessage = "";
    //Add
    if (!(document.getElementById("AddElement") as HTMLInputElement).disabled) {
      this.userService.CreateEmployee(this.employee).subscribe((response : any)=> {
        var user = this.employee;
        user.Id = response.Id
        this.UnloadEmployee();
        this.employees.push(user);
        this.dataSource = new MatTableDataSource(this.employees);
      }, error => {
        this.error= true;
        for (const [key, value] of Object.entries(error.error)) {
          (value as Array<string>).forEach(element => {
            this.errorMessage += element +"<br>";
          });
        }
      });
    }
    //Edit
    if (!(document.getElementById("EditElement") as HTMLInputElement).disabled) {
      console.log("hello") 
      this.userService.UpdateEmployee(JSON.stringify(this.employee)).subscribe(response => {
      }, error => {
        console.log(error)
        this.error= true;
        for (const [key, value] of Object.entries(error.error)) {
          (value as Array<string>).forEach(element => {
            this.errorMessage += element +"<br>";
          });
        }
      });
    }

    this.navbarService.updateProgressBar(false);
  }
  onResize(event) {
    if(event.target.innerWidth< 1200){
      this.ShowDiv = false;
    }
  }
}

@Component({
  selector: 'Dialog-CRUD-Employee',
  templateUrl: 'Dialog-CRUD-Employee.html',
})
export class DialogCRUDEmployee {

  constructor(
    public dialogRef: MatDialogRef<DialogCRUDEmployee>,
    @Inject(MAT_DIALOG_DATA) public data: User) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
  onValidClick() {
    this.dialogRef.close("Valid");
  }

}
