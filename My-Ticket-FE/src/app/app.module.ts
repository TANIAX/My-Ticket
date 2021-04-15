import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ChangeDetectorRef } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgForm, FormsModule, FormArray, FormControl, ReactiveFormsModule, FormGroup, Validators } from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { DatePipe, CommonModule } from '@angular/common';
import { ShowHidePasswordModule } from 'ngx-show-hide-password';
import { PasswordStrengthMeterModule } from 'angular-password-strength-meter';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { ToastrModule } from 'ngx-toastr';
import { NgxCaptchaModule } from 'ngx-captcha';
import { AuthInterceptor } from './Shared/auth/auth.interceptor';
import { MatProgressBarModule } from '@angular/material';
import {NgcCookieConsentModule, NgcCookieConsentConfig} from 'ngx-cookieconsent';
import { NgxEditorModule } from 'ngx-editor';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { SwipeComponent } from './Shared/Component/Swipe/Swipe.component';
import { NavbarComponent } from './Shared/Component/Layout/navbar/navbar.component';
import { SignUpComponent } from './Shared/Component/User/sign-up/sign-up.component';
import { FooterComponent } from './Shared/Component/Layout/footer/footer.component';
import { LogInComponent } from './Shared/Component/User/log-in/log-in.component';
import { SidePanelComponent } from './Shared/Component/Layout/side-panel/side-panel.component';
// import { pbMarginTop } from './Shared/directive/pb-margin-top.directive';
import { ForgotPasswordComponent } from './Shared/Component/User/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './Shared/Component/User/reset-password/reset-password.component';
import { MyAccountComponent } from './Shared/Component/User/my-account/my-account.component';
import { AuthGuard } from './Shared/auth/auth.guard';
import { EditMyAccountComponent } from './Shared/Component/User/edit-my-account/edit-my-account.component';
import { MainComponent } from './Shared/Component/main/main.component';
import { HomeComponent } from './Shared/Component/home/home.component';
import { EmployeeListComponent, DialogCRUDEmployee } from './Customer/Component/Employee/employee-list/employee-list.component';
import { MaterialModule } from './material-module';
import { User } from './Shared/model/user.model';
import { CreateStaffTicketComponent } from './Staff/Component/Ticket/add-ticket/create-staff-ticket.component';
import { MyTicketsListComponent } from './Shared/Component/Ticket/my-tickets-list/my-tickets-list.component';
import { TicketComponent } from './Shared/Component/Ticket/ticket/ticket.component';
import { ListTicketsComponent, DialogDeleteReason } from './Staff/Component/Ticket/list-tickets/list-tickets.component';
import { FilterPipe } from './Shared/pipes/filter.pipe';
import { CookieService } from 'ngx-cookie-service';
import { Error404Component } from './Shared/Component/Layout/error/error404/error404.component';
import { Error403Component } from './Shared/Component/Layout/error/error403/error403.component';
import { Error500Component } from './Shared/Component/Layout/error/error500/error500.component';
import { CreateCustomerTicketComponent } from './Customer/Component/Ticket/create-customer-ticket/create-customer-ticket.component';
import { DashboardComponent } from './Staff/Component/dashboard/dashboard.component';
import { CreateCustomerComponent } from './Staff/Component/Customer/create-customer/create-customer.component';
import { ListCustomerComponent } from './Staff/Component/Customer/list-customer/list-customer.component';

const cookieConfig:NgcCookieConsentConfig = {
  "cookie": {
    "domain": "localhost"
  },
  "position": "bottom-right",
  "theme": "classic",
  "palette": {
    "popup": {
      "background": "#000000",
      "text": "#ffffff",
      "link": "#ffffff"
    },
    "button": {
      "background": "#f1d600",
      "text": "#000000",
      "border": "transparent"
    }
  },
  "type": "info",
  "content": {
    "message": "This website uses cookies to ensure you get the best experience on our website.",
    "dismiss": "Got it!",
    "deny": "Refuse cookies",
    "link": "Learn more",
    "href": "https://cookiesandyou.com",
    "policy": "Cookie Policy"
  }
};

@NgModule({
  declarations: [
    AppComponent,
    SwipeComponent,
    NavbarComponent,
    SignUpComponent,
    FooterComponent,
    LogInComponent,
    SidePanelComponent,
    // pbMarginTop, not used anymore, decoment this is we use directive again
    ForgotPasswordComponent,
    ResetPasswordComponent,
    MyAccountComponent,
    EditMyAccountComponent,
    MainComponent,
    HomeComponent,
    EmployeeListComponent,
    DialogCRUDEmployee,
    CreateStaffTicketComponent,
    MyTicketsListComponent,
    TicketComponent,
    ListTicketsComponent,
    FilterPipe,
    Error404Component,
    Error403Component,
    Error500Component,
    CreateCustomerTicketComponent,
    DashboardComponent,
    DialogDeleteReason,
    CreateCustomerComponent,
    ListCustomerComponent,
  ],
  entryComponents: [DialogCRUDEmployee,DialogDeleteReason],
  imports: [
    CommonModule,
    BrowserModule,
    ShowHidePasswordModule,
    HttpClientModule,
    PasswordStrengthMeterModule,
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot(),
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    NgcCookieConsentModule.forRoot(cookieConfig),
    NgxCaptchaModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatProgressBarModule,
    NgxEditorModule,
    AngularFontAwesomeModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    ReactiveFormsModule,
    NgbModule,
    NgxPaginationModule,
    
  ],
  providers: [AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
