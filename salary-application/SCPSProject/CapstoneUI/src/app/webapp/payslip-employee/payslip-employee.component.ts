import { Component, OnInit } from "@angular/core";
import { ApiService } from "src/app/api.service";
import { ActivatedRoute, Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";

declare var $: any;
@Component({
  selector: "app-payslip-employee",
  templateUrl: "./payslip-employee.component.html",
  styleUrls: ["./payslip-employee.component.css"],
})
export class PayslipEmployeeComponent implements OnInit {
  Hash: any;

  UserID: any;
  constructor(
    private api: ApiService,
    private route: ActivatedRoute,
    public router: Router,
    public fb: FormBuilder
  ) {}
  counter = 0;
  employee: any;
  listPayroll : string[] = [];
  employeeCode: any;
  ngOnInit(): void {
    var user = JSON.parse(localStorage.getItem('user'));
    this.employeeCode = user.Code;
    this.getAllPayroll();

  }
  nameTitle = "";
  getAllPayroll() {
    this.api.getAllPayroll().subscribe(
      (res) => {
      this.month = res[0].Month;
      this.year = res[0].Year;
       this.GetPayslipDefault(res[0].Month,res[0].Year)
        res.forEach((element) => {
          console.log(element.Month , element.Year)
          this.listPayroll.push(
            "Tháng " + element.Month + " Năm " + element.Year
          );
        });
      },
      (err) => {
        console.log(err);
      }
    );
  }

  numberToString(num) {
    let string = Math.round(num).toString();

    string = string.toString().replace(/,/g, "");
    string = string.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    return string;
  }

  renderTemplate(data, num) {
    let value = 0;
    let $template = $(
      `<ul class="${
        num === 3 ? "formula-type" : num === 2 ? "ref-type" : "field-type"
      }"></ul>`
    );

    if (num !== 4) {
      this.counter++;
    }

    let $li =
      num === 4
        ? $('<li class="empty"></li>')
        : $(
            `<li class="name" data-value="${data["Value"]}" data-name="${
              data[num === 3 ? "Formula" : "Name"]
            }"><span class="item ${this.counter % 2 === 0 ? "even" : "odd"}">${
              data[num === 3 ? "Formula" : "Name"]
            } </span></li>`
          );

    if (num === 3) {
      $li
        .find(".item")
        .append(
          `<span class="value">${this.numberToString(data["Value"])}</span>`
        );

      data["Details"].forEach((element) => {
        const retrieveData =
          element[
            element["Type"] === 3
              ? "FormulaType"
              : element["Type"] === 2
              ? "RefType"
              : "FieldType"
          ];

        $li.append(this.renderTemplate(retrieveData, element["Type"]));
      });
    } else if (num === 1 || num === 2) {
      $li
        .find(".item")
        .append(
          `<span class="value">${this.numberToString(data["Value"])}</span>`
        );
    }

    if (num !== 4) $template.append($li);

    return $template;
  }

  month = 0;
  year = 0;
  GetPayslipDefault(month , year ) {
  this.api.getPayslipEmployee(month, year,this.employeeCode).subscribe(
    (res) => {
      $("#template-block #head").html(`
        <span class="total">Họ và tên: ${res["employee"]["fullName"]}</span>
        <span class="total">Email: ${res["employee"]["email"]}</span>
        <span class="total">Bộ phận: ${res["employee"]["department"]}</span>
          `);
      $("#template-block #body").html(
        this.renderTemplate(res["payroll"]["formula"], 3)
      );
      $("#template-block #body > ul").prepend(
        '<li class="column-name"><span>Thành phần tính Lương</span><span>Giá trị</span></li>'
      );
      $("#template-block #footer").html(
        `<span class="total">Thực nhận: ${this.numberToString(
          res["payroll"]["formula"]["Value"]
        )} VND</span>`
      );

      

      let c = 0;
      let cmpArray = [];

      $(".odd, .even").removeClass("odd even");
      $(".name").each(function () {
        let isExisted = true;

        const dataName = $(this).data("name");
        const dataValue = $(this).data("value");

        if (!cmpArray.includes(dataName)) {
          cmpArray = [...cmpArray, dataName];
        } else {
          isExisted = false;
          $(this).remove();
        }

        if (dataValue == "0") {
          isExisted = false;
          $(this).remove();
        }

        if (isExisted) {
          const className = c++ % 2 === 0 ? "even" : "odd";


          $(this).children(".item").addClass(className);
        }
      });

      this.counter = 0;
    },
    (err) => {
      console.log(err);
    }
  );
}
  GetPayslip( ) {
      let  month = $("#pay-month").val().split(' ')[1];
      let year = $("#pay-month").val().split(' ')[3];
      this.month = month;
      this.year = year;
    this.api.getPayslipEmployee(month, year,this.employeeCode).subscribe(
      (res) => {
        $("#template-block #head").html(`
          <span class="total">Họ và tên: ${res["employee"]["fullName"]}</span>
          <span class="total">Email: ${res["employee"]["email"]}</span>
          <span class="total">Bộ phận: ${res["employee"]["department"]}</span>
            `);
        $("#template-block #body").html(
          this.renderTemplate(res["payroll"]["formula"], 3)
        );
        $("#template-block #body > ul").prepend(
          '<li class="column-name"><span>Thành phần tính Lương</span><span>Giá trị</span></li>'
        );
        $("#template-block #footer").html(
          `<span class="total">Thực nhận: ${this.numberToString(
            res["payroll"]["formula"]["Value"]
          )} VND</span>`
        );

        

        let c = 0;
        let cmpArray = [];

        $(".odd, .even").removeClass("odd even");
        $(".name").each(function () {
          let isExisted = true;

          const dataName = $(this).data("name");
          const dataValue = $(this).data("value");

          if (!cmpArray.includes(dataName)) {
            cmpArray = [...cmpArray, dataName];
          } else {
            isExisted = false;
            $(this).remove();
          }

          if (dataValue == "0") {
            isExisted = false;
            $(this).remove();
          }

          if (isExisted) {
            const className = c++ % 2 === 0 ? "even" : "odd";


            $(this).children(".item").addClass(className);
          }
        });

        this.counter = 0;
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
