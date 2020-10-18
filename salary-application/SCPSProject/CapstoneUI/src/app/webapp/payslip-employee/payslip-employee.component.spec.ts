import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayslipEmployeeComponent } from './payslip-employee.component';

describe('PayslipEmployeeComponent', () => {
  let component: PayslipEmployeeComponent;
  let fixture: ComponentFixture<PayslipEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayslipEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayslipEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
