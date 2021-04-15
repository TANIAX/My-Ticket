import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgcCookieConsentService, NgcInitializeEvent } from 'ngx-cookieconsent';
import {NgcCookieConsentModule} from 'ngx-cookieconsent';
import { Subscription }   from 'rxjs/Subscription';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  
  title = "MY-TICKET"
  //keep refs to subscriptions to be able to unsubscribe later
  private popupOpenSubscription: Subscription;
  private popupCloseSubscription: Subscription;
  private initializeSubscription: Subscription;
  private statusChangeSubscription: Subscription;
  private revokeChoiceSubscription: Subscription;
  private noCookieLawSubscription: Subscription;

  constructor(private ccService: NgcCookieConsentService){}

  ngOnInit() {

  }

  ngOnDestroy(){
    this.popupOpenSubscription.unsubscribe();
  }
}
