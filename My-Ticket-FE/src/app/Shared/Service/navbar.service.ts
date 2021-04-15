import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {
  private isLoading: BehaviorSubject<Boolean> = new BehaviorSubject<Boolean>(null);
  public isLoading$: Observable<Boolean> = this.isLoading.asObservable();
  private needRefresh: BehaviorSubject<Boolean> = new BehaviorSubject<Boolean>(null);
  public needRefresh$: Observable<Boolean> = this.needRefresh.asObservable();
  constructor() { }
  updateProgressBar(status:boolean) {
    this.isLoading.next(status);
  }
  refreshNavbar(){
    this.needRefresh.next(true)
  }
}

