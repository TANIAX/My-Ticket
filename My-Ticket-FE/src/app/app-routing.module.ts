import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { SignUpComponent } from './Shared/Component/User/sign-up/sign-up.component';
import { LogInComponent } from './Shared/Component/User/log-in/log-in.component';
import { ForgotPasswordComponent } from './Shared/Component/User/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './Shared/Component/User/reset-password/reset-password.component';
import { MyAccountComponent } from './Shared/Component/User/my-account/my-account.component'
import { AuthGuard } from './Shared/auth/auth.guard';
import { EditMyAccountComponent } from './Shared/Component/User/edit-my-account/edit-my-account.component';
import { MainComponent } from './Shared/Component/main/main.component';
import { HomeComponent } from './Shared/Component/home/home.component';
import { EmployeeListComponent } from './Customer/Component/Employee/employee-list/employee-list.component';
import { CreateStaffTicketComponent } from './Staff/Component/Ticket/add-ticket/create-staff-ticket.component';
import { MyTicketsListComponent } from './Shared/Component/Ticket/my-tickets-list/my-tickets-list.component';
import { TicketComponent } from './Shared/Component/Ticket/ticket/ticket.component';
import { ListTicketsComponent } from './Staff/Component/Ticket/list-tickets/list-tickets.component';
import { Error404Component } from './Shared/Component/Layout/error/error404/error404.component';
import { Error403Component } from './Shared/Component/Layout/error/error403/error403.component';
import { Error500Component } from './Shared/Component/Layout/error/error500/error500.component';
import { CreateCustomerTicketComponent } from './Customer/Component/Ticket/create-customer-ticket/create-customer-ticket.component';
import { DashboardComponent } from './Staff/Component/dashboard/dashboard.component';
import { CreateCustomerComponent } from './Staff/Component/Customer/create-customer/create-customer.component';
import { ListCustomerComponent } from './Staff/Component/Customer/list-customer/list-customer.component';

const routes: Routes = [
  {
    path: '', component: MainComponent,
    children: [
      { path: '404', component: Error404Component },
      { path: '403', component: Error403Component },
      { path: '500', component: Error500Component },
      
      /*User*/
  { path: 'User', children: [
    { path: 'SignUp', component: SignUpComponent },
    { path: 'Login', component: LogInComponent },
    { path: 'ForgotPassword', component: ForgotPasswordComponent },
    { path: 'ResetPassword/:code', component: ResetPasswordComponent },
    { path: 'MyAccount', component: MyAccountComponent, canActivate: [AuthGuard] },
    { path: 'EditMyProfil', component: EditMyAccountComponent, canActivate: [AuthGuard] },
  ] },
  /*Ticket*/ 
  { path: 'Ticket', children: [
    { path: '', component: MyTicketsListComponent, canActivate: [AuthGuard], data: { roles: ['Customer'] }},
    { path: 'Create', component: CreateCustomerTicketComponent, canActivate: [AuthGuard], data: { roles: ['Customer'] }},
    { path: ':id', component: TicketComponent, canActivate: [AuthGuard]},
  ] },
  /*Employee*/
  { path: 'Employee', children: [
    { path: 'List', component: EmployeeListComponent },
  ] },
  /*Staff*/
  { path: 'Staff', children: [
      { path: 'Dashboard', component: DashboardComponent, canActivate: [AuthGuard],data: { roles: ['Admin','Member']}},
        /*Staff Ticket*/
      { path: 'Ticket', children: [
        {path: '' ,component: ListTicketsComponent, canActivate: [AuthGuard], data: { roles: ['Admin','Member']}},
        {path: 'Ticket/Create', component: CreateStaffTicketComponent, canActivate: [AuthGuard],data: { roles: ['Admin','Member']}}
      ]},
        /*User*/
      { path: 'User', children: [
        {path: '' ,component: ListCustomerComponent, canActivate: [AuthGuard], data: { roles: ['Admin','Member']}},
        {path: 'Edit/:id', component: CreateCustomerComponent, canActivate: [AuthGuard],data: { roles: ['Admin','Member']}},
        {path: 'Create', component: CreateCustomerComponent, canActivate: [AuthGuard],data: { roles: ['Admin','Member']}}
      ]},
    ]},
  ]
  },
  { path: 'Home', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
