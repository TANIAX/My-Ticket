import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateStaffTicketComponent } from './create-staff-ticket.component';

describe('AddTicketComponent', () => {
  let component: CreateStaffTicketComponent;
  let fixture: ComponentFixture<CreateStaffTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateStaffTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateStaffTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
