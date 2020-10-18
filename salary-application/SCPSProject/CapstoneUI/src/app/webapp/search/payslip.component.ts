import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { UtilityService } from 'src/app/util.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PrimitiveService } from 'src/app/primitive.service';
import { SPCSObject } from 'src/app/class/SPCSObject';

@Component({
  selector: 'app-payslip',
  templateUrl: './payslip.component.html',
  styleUrls: ['./payslip.component.css']
})
export class PayslipComponent implements OnInit {

  Hash: any;

  UserID: any;

  // linkPDF: any;
  // link: any[] = [];
  // index: any;

  // e: any;

  payslips = new SPCSObject;
  formulasComponents = [];
  temp: any;

  payrollId: any;
  payrolls: any[] = []

  employeeCode: any;
  employees: any[] = []

  constructor(private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) { }

  ngOnInit(): void {
    // this.index = 0;
    // this.e = document.getElementById("payslipFrame") as HTMLIFrameElement;
    // this.link[0] = "https://toolsfortransformation.net/wp-content/uploads/2017/03/Annex-E-%E2%80%93-Sample-Payslip.pdf";
    // this.link[1] = "https://www.dsal.gov.mo/download/pdf_en/laborlawtemp/pay_receipt_e.pdf";
    // this.linkPDF = this.link[this.index];
    // this.e.src = this.linkPDF;

    this.payrollId = 0;
    this.employeeCode = "";

    this.getAllPayroll();
    this.getAllEmployee();
  }

  extractPayslip(detail) {
    console.log(detail);
    console.log(this.formulasComponents);
    switch (detail.Type) {
      case 1:
        this.formulasComponents.push(
          {
            Name: detail.FieldType.Name,
            Value: detail.FieldType.Value,
          }
        );
        break;
      case 2:
        this.formulasComponents.push(
          {
            Name: detail.RefType.Name,
            Value: detail.RefType.Value,
          }
        );
        break;
      case 3:
        this.formulasComponents.push(
          {
            Name: detail.FormulaType.Formula,
            Value: detail.FormulaType.Value,
          }
        );
        for (let i = 0; i < detail.FormulaType.Details.length; i++) {
          const el = detail.FormulaType.Details[i];
          this.extractPayslip(el);
        }
        break;
      case 4:
        // this.formulasComponents.push(
        //   {
        //     Name: detail.ConstantType.Value,
        //     Value: detail.ConstantType.Value,
        //   }
        // );
        break;
      default:
        break;
    }
  }

  getAllPayroll() {
    this.api.getAllPayroll()
      .subscribe(
        res => {
          this.payrolls = res;
          this.payrollId = this.payrolls[0].Id;
        },
        err => {

        }
      )
  }

  getAllEmployee() {
    this.api.getAllEmployee()
      .subscribe(
        res => {
          this.employees = res;
          this.employeeCode = this.employees[0].Code;
        },
        err => {

        }
      )
  }

  getPayslipByPayrollIdAndEmployeeId() {
    this.formulasComponents = [];
    this.api.getPayslipByPayrollIdAndEmployeeId(this.employeeCode, this.payrollId)
      .subscribe(
        res => {
          this.payslips = res;
          // config payslip
          this.temp = this.payslips.payroll;
          this.temp.Type = 3;
          this.temp.FormulaType = this.payslips.payroll.formula;
          this.temp.formula = {};
          this.extractPayslip(this.temp);
        },
        err => {

        }
      )
  }

  setPayrollId(payrollId) {
    this.payrollId = payrollId;
  }

  setEmployeeId(employeeCode) {
    this.employeeCode = employeeCode;
  }

  // getPayslip() {

  // }
}
