import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyTicketsListComponent } from './my-tickets-list.component';

describe('ListTicketComponent', () => {
  let component: MyTicketsListComponent;
  let fixture: ComponentFixture<MyTicketsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyTicketsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyTicketsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
