import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCustomerTicketComponent } from './create-customer-ticket.component';

describe('CreateCustomerTicketComponent', () => {
  let component: CreateCustomerTicketComponent;
  let fixture: ComponentFixture<CreateCustomerTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateCustomerTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateCustomerTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
