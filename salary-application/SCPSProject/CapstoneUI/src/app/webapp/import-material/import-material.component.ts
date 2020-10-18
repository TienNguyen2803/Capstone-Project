import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { UtilityService } from 'src/app/util.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PrimitiveService } from 'src/app/primitive.service';
import { SPCSObject } from 'src/app/class/SPCSObject';
import * as XLSX from 'xlsx';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subscription, combineLatest } from 'rxjs';
import Swal from 'sweetalert2';

declare var $: any;

@Component({
  selector: 'app-import-material',
  templateUrl: './import-material.component.html',
  styleUrls: ['./import-material.component.css']
})
export class ImportMaterialComponent implements OnInit {

  excelFileSalary: any;

  salarycomponents = [];

  empSalaryComponent: any;

  Hash: any;
  UserID: any;

  modalRef: BsModalRef;

  constructor(private changeDetection: ChangeDetectorRef, private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) { }

  ngOnInit(): void {
    this.getEmpSalaryComponents();
  }

  getEmpSalaryComponents() {
    this.api.getEmpSalaryComponents()
      .subscribe(
        res => {
          this.empSalaryComponent = res;

          this.jqueryDataTable();
        },
        err => {

        }
      )
  }

  jqueryDataTable() {

    var cols = [
      { sTitle: "MÃ", data: "Code" },
      { sTitle: "HỌ VÀ TÊN", data: "Fullname" },
    ]
    for (let k = 0; k < this.empSalaryComponent.emps.length; k++) {
      const element = this.empSalaryComponent.emps[k];
      for (let i = 0; i < element.SalaryComponents.length; i++) {
        var el = element.SalaryComponents[i];
        el.Value = this.util.commaForBigNum2(el.Value);
      }

    }
    for (let i = 0; i < this.empSalaryComponent.emps[0].SalaryComponents.length; i++) {
      var el = this.empSalaryComponent.emps[i].SalaryComponents[i];
      cols.push(
        { sTitle: el.FieldName, data: ("SalaryComponents." + i + ".Value") }
      );
    }
    console.log(this.empSalaryComponent);
    console.log(cols);
    // jquery for datatable
    var document_table = $('#datatable').DataTable({
      data: this.empSalaryComponent.emps,
      columns: cols,
      destroy: true,
      lengthChange: false,
      buttons: ['colvis'],
      pageLength: 7,
      // language
      "language": {
        "decimal": "",
        "emptyTable": "Không có quyết định nào trong hệ thống",
        "info": "Hiện Quyết định từ _START_ tới _END_ trên tổng số _TOTAL_ ",
        "infoEmpty": "",
        "infoFiltered": "(tìm kiếm trên tổng số _MAX_ Quyết định)",
        "infoPostFix": "",
        "thousands": ",",
        "loadingRecords": "Đang tải dữ liệu...",
        "processing": "Đang xử lý dữ liệu...",
        "search": "Tìm kiếm:",
        "zeroRecords": "Không có quyết định nào trong hệ thống",
        "paginate": {
          "first": "Đầu",
          "last": "Cuối",
          "next": "Sau",
          "previous": "Trước"
        },
        "aria": {
          "sortAscending": ": Sấp xếp tăng dần theo cột",
          "sortDescending": ": Sấp xếp giảm dần theo cột"
        }
      }
    });
    document_table.buttons().container()
      .appendTo('#payroll-datatable_wrapper .col-md-6:eq(0)');
  }

  openSalaryModal() {

    var subscriptions = [] as Subscription[];

    const initialState = {

    };

    this.modalRef = this.modalService.show(
      ModalSalaryComponent, { initialState }
    );

    const _combine = combineLatest(
      this.modalService.onShow,
      this.modalService.onShown,
      this.modalService.onHide,
      this.modalService.onHidden
    ).subscribe(() => this.changeDetection.markForCheck());

    subscriptions.push(
      this.modalService.onHidden.subscribe(reason => {
        // do after hidden in here

        subscriptions.forEach((subscription: Subscription) => {
          subscription.unsubscribe();
        });
        subscriptions = [];
      })
    );

    subscriptions.push(_combine);
  }

}


@Component({
  selector: 'modal-salary',
  template: `
  <div class="modal-header">
            <h5 class="modal-title" style="font-size: 20px"><b>Thông Tin Lương</b></h5>
            <button type="button" class="close" (click)="symbolModalRef.hide()" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="modal-body">
            <div class="row" style="width: 100%;">
                    <div class="card" style="width: 100%;">
                        <div class="card-body" style="width: 100%;">
                            <!-- Tạo nội dung vào đây -->

                            <div style="margin-left: 5px; width: 100%; clear: both;">
                                <h4 style="font-size: 18px;">Thông Tin Lương Nhân Viên
                                    <i (click)="downloadSalary()" style="cursor: pointer;" class="fa fa-download"></i>
                                </h4>
                                <input (change)="onFileChange1($event)" class="form-control"
                                    style="float: left; width: 60%;" type="file">
                            </div>

                        </div>
                    </div>
                </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-success" (click)="UploadSalary();">
            <i class="fa fa-check-square-o"></i>
            Hoàn thành
          </button>
        </div>
    `,
})

export class ModalSalaryComponent implements OnInit {

  excelFileSalary: any;

  salarycomponents = [];

  constructor(public symbolModalRef: BsModalRef, private api: ApiService) { }

  ngOnInit() {

  }

  onFileChange1(evt: any) {
    this.excelFileSalary = new SPCSObject;
    /* wire up file reader */
    const target: DataTransfer = <DataTransfer>(evt.target);
    if (target.files.length !== 1) throw new Error('Cannot use multiple files');
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });

      /* grab first sheet */
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];

      /* save data */
      this.excelFileSalary = <XLSX.AOA2SheetOpts>(XLSX.utils.sheet_to_json(ws, { header: 1 }));
      console.log(this.excelFileSalary);
    };
    reader.readAsBinaryString(target.files[0]);
  }

  UploadSalary() {
    for (let i = 3; i < this.excelFileSalary.length; i++) {
      const eli = this.excelFileSalary[i];
      for (let j = 2; j < eli.length; j++) {
        let fieldId = this.excelFileSalary[1][j]
        let employeeCode = eli[0];
        const elj = eli[j];
        this.salarycomponents.push(
          {
            EmpId: employeeCode,
            FieldId: fieldId,
            Value: elj,
            ApplyDate: "2020-03-14T10:16:02.508Z",
          }
        );
      }
    }
    console.log(this.salarycomponents);
    this.api.createSalaryComponents(this.salarycomponents)
      .subscribe(
        res => {
          Swal.fire("Thành Công", "Nạp thành công", "success");
          this.symbolModalRef.hide();
          location.href = "importmaterial";
        },
        err => {
          Swal.fire("Cảnh Báo", "File không hợp lệ", "warning");
          console.log(err);
        }
      )
  }
  downloadSalary() {
    window.location.href = this.api.getApiUrl() + 'Payroll/GetExcelTemplate?status=1'
  }
  // downloadSalary() {
  //   this.api.downloadSalaryFull([]).subscribe(
  //     res => {
  //       var blob = new Blob([res], {
  //         type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
  //       });
  //       saveAs(blob, 'Mẫu Thông Tin Lương Nhân Viên' + '.xlsx');
  //     },
  //     err => {

  //     }
  //   )
  // }

}