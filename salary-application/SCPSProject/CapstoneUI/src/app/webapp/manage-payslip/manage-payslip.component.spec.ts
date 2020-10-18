import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagePayslipComponent } from './manage-payslip.component';

describe('ManagePayslipComponent', () => {
  let component: ManagePayslipComponent;
  let fixture: ComponentFixture<ManagePayslipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagePayslipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagePayslipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
