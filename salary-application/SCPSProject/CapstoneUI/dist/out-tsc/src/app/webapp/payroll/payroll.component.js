import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
import * as XLSX from 'xlsx';
import { combineLatest } from 'rxjs';
let PayrollComponent = class PayrollComponent {
    constructor(changeDetection, modalService, validator, api, route, router, util) {
        this.changeDetection = changeDetection;
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.payrollsAPI = [];
        this.payrolls = [];
        // page divide
        this.pageQuantity = 6;
        this.pageNum = 1;
        this.pageNumArray = [];
        // payroll details
        // payrollDetails = new Array(1);
        // index = 0;
        this.payrollToCreate = new SPCSObject;
        this.payrollToUpdate = new SPCSObject;
        this.ft = new SPCSObject;
    }
    openModal(template) {
        this.modalRef = this.modalService.show(template);
    }
    ngOnInit() {
        this.ft.importModal = [];
        this.payrollToCreate.Month = 1;
        this.payrollToCreate.Year = 1;
        this.payrollToCreate.StandardWorkDay = 26;
        this.payrollToCreate.Revenue = 0;
        this.payrollToCreate.FromDate = "";
        this.payrollToCreate.ToDate = "";
        this.payrollToCreate.PayDate = "";
        this.payrollToUpdate.Month = 1;
        this.payrollToUpdate.Year = 1;
        this.payrollToUpdate.StandardWorkDay = 26;
        this.payrollToUpdate.Revenue = 0;
        this.payrollToUpdate.FromDate = "";
        this.payrollToUpdate.ToDate = "";
        this.payrollToUpdate.PayDate = "";
        let vm = this;
        setInterval(function () {
            vm.getAllPayrolls();
        }, 3000);
    }
    sendEmail() {
        this.api.sendEmail().subscribe(res => {
        }, err => {
        });
    }
    getAllPayrolls() {
        this.api.getAllPayroll()
            .subscribe(res => {
            this.payrolls = res;
            for (let i = 0; i < this.payrolls.length; i++) {
                const el = this.payrolls[i];
                // chnage status
                switch (el.Status) {
                    case 1:
                        el.Status = "Mới";
                        break;
                    case 2:
                        el.Status = "Đã đăng tải";
                        break;
                    case 3:
                        el.Status = "Đã thanh toán";
                        break;
                    case 4:
                        el.Status = "Quá hạn thanh toán";
                        break;
                    default:
                        el.Status = "Mới";
                        break;
                }
                // combine month year and fromdate to date
                el.MY = "Tháng " + el.Month + " Năm " + el.Year;
                el.FT = "Từ <b>" + this.util.formatDate(el.FromDate)
                    + "</b> Đến <b>" + this.util.formatDate(el.ToDate) + "</b>";
                // format paydate
                el.PayDate = this.util.formatDate(el.PayDate);
                // el.BtnDetail = `<i id = 'Print_` + el.Id + `' class='fa fa-angle-double-right fa-2x ast-black' onclick='location.href =\`` + `payroll/detail?payrollId=` + JSON.stringify(el.Id) + `\`'></i>`;
                el.BtnImport = `<i id = "Import_` + el.Id + `" class="fa fa-share fa-2x ast-black trigger-modal-import" ></i>`;
                el.BtnEdit = `<i id = "Edit_` + el.Id + `" class="fa fa-edit fa-2x ast-yellow" data-toggle="modal" data-target="#modal-publishPayroll"></i>`;
                el.BtnPrint = `<i id = "Print_` + el.Id + `" class="fa fa-print fa-2x ast-black" data-toggle="modal" data-target="#modal-downloadPayroll"></i>`;
            }
            // console.log(this.payrolls);
            this.jqueryInit();
        }, err => {
        });
    }
    ngAfterViewChecked() {
        // var btnImports = document.getElementsByClassName('trigger-modal-import');
        // for (let i = 0; i < btnImports.length; i++) {
        //   const btn = btnImports[i];
        //   let payrollId = btn.id.split('_')[1];
        //   let vm = this;
        //   // if (this.ft.importModal[i] != null){
        //   //   btn.removeEventListener('click', this.ft.importModal[i]);
        //   //   console.log(`remove`+i);
        //   // }
        //   // this.ft.importModal[i] = function (e) {
        //   //   vm.openMonthlyModal(payrollId);
        //   // };
        //   // btn.addEventListener('click', this.ft.importModal[i]);
        //   // console.log(`add`+i);
        //   if (this.ft.importModal[i] == null) {
        //     this.ft.importModal[i] = function (e) {
        //       vm.openMonthlyModal(payrollId);
        //     };
        //     btn.addEventListener('click', this.ft.importModal[i]);
        //     console.log(`add` + i);
        //   }
        // }
        // // console.log(btnImports);
    }
    getActiveDocument() {
        console.log(this.payrollToCreate.Month, this.payrollToCreate.Year);
        this.api.getActiveDocument(this.payrollToCreate.Month, this.payrollToCreate.Year).subscribe(res => {
            console.log(res);
            this.payrollToCreate.FromDate = this.util.formatDate(res.FromDate);
            this.payrollToCreate.ToDate = this.util.formatDate(res.ToDate);
            this.payrollToCreate.PayDate = this.util.formatDate(res.PayDate);
        }, err => {
        });
    }
    jqueryInit() {
        this.jqueryDatePicker();
        this.jqueryDataTable();
        this.jqueryBtn();
    }
    jqueryDatePicker() {
        // date single picker
        // $('.autoclose-datepicker').datepicker({
        //   autoclose: true,
        //   todayHighlight: true,
        //   format: 'dd/mm/yyyy',
        //   formatSubmit: 'yyyy-mm-dd',
        // });
    }
    jqueryDataTable() {
        // jquery for datatable
        var document_table = $('#payroll-datatable').DataTable({
            data: this.payrolls,
            columns: [
                { data: "MY" },
                { data: "DocumentVM.Code" },
                { data: "FT" },
                { data: "PayDate" },
                { data: "Status" },
                // { data: "BtnDetail"},
                { data: "BtnImport" },
                { data: "BtnEdit" },
                { data: "BtnPrint" },
            ],
            destroy: true,
            lengthChange: false,
            buttons: ['colvis'],
            pageLength: 10,
            // language
            "language": {
                "decimal": "",
                "emptyTable": "Không có bảng lương nào trong hệ thống",
                "info": "Hiện Bảng lương từ _START_ tới _END_ trên tổng số _TOTAL_ ",
                "infoEmpty": "",
                "infoFiltered": "(tìm kiếm trên tổng số _MAX_ Bảng lương)",
                "infoPostFix": "",
                "thousands": ",",
                "loadingRecords": "Đang tải dữ liệu...",
                "processing": "Đang xử lý dữ liệu...",
                "search": "Tìm kiếm:",
                "zeroRecords": "Không có bảng lương nào trong hệ thống",
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
    jqueryBtn() {
        var vm = this;
        for (let i = 0; i < this.payrolls.length; i++) {
            const el = this.payrolls[i];
            let id_btn_import = "#Import_" + el.Id;
            $(id_btn_import).click(function () {
                vm.openMonthlyModal(el.Id);
            });
            let id_btn_edit = "#Edit_" + el.Id;
            $(id_btn_edit).click(function () {
                vm.setPayroll(el.Id);
            });
            let id_btn_print = "#Print_" + el.Id;
            $(id_btn_print).click(function () {
                vm.setPayroll(el.Id);
                vm.showModalDownload();
            });
        }
    }
    filterData() {
        this.payrolls = this.payrollsAPI.filter((el) => {
            let temp1 = this.util.nonAccentVietnamese(this.txtSearch);
            let temp2;
            // ID
            // temp2 = this.util.nonAccentVietnamese(el.AccessoryID);
            // if (temp2.indexOf(temp1) !== -1 || temp1.indexOf(temp2) !== -1) {
            //   return el;
            // }
        });
    }
    publishPayroll() {
        this.api.publishPayroll(this.payrollToUpdate.Id)
            .subscribe(res => {
            alert("Đăng tải thành công");
            this.ngOnInit();
        }, err => {
            alert(err.error);
            console.log(err);
        });
    }
    setPayroll(payrollId) {
        let tempPayroll = this.payrolls.filter((el) => {
            if (payrollId == el.Id) {
                return el;
            }
        });
        this.payrollToUpdate = tempPayroll[0];
        console.log(tempPayroll);
    }
    showModalDownload() {
        document.getElementById('modal-downloadPayroll').style.display = 'block';
    }
    showModalDetail() {
        document.getElementById('modal-detailPayroll').style.display = 'block';
    }
    downloadPayroll() {
        this.api.downloadPayroll(this.payrollToUpdate.Id).subscribe(res => {
            location.href = "http://spcs.azurewebsites.net/api/Payroll/DownloadPayroll?payrollId=" + this.payrollToUpdate.Id;
        }, err => {
            alert("Bảng lương chưa được đăng tải");
        });
    }
    openMonthlyModal(payrollId) {
        var subscriptions = [];
        const initialState = {
            payrollId: payrollId
        };
        this.modalRef = this.modalService.show(ModalMonthlyComponent, { initialState });
        const _combine = combineLatest(this.modalService.onShow, this.modalService.onShown, this.modalService.onHide, this.modalService.onHidden).subscribe(() => this.changeDetection.markForCheck());
        subscriptions.push(this.modalService.onHidden.subscribe(reason => {
            // do after hidden in here
            subscriptions.forEach((subscription) => {
                subscription.unsubscribe();
            });
            subscriptions = [];
        }));
        subscriptions.push(_combine);
    }
};
PayrollComponent = __decorate([
    Component({
        selector: 'app-payroll',
        templateUrl: './payroll.component.html',
        styleUrls: ['./payroll.component.css']
    })
], PayrollComponent);
export { PayrollComponent };
let ModalMonthlyComponent = class ModalMonthlyComponent {
    constructor(symbolModalRef, api) {
        this.symbolModalRef = symbolModalRef;
        this.api = api;
        this.activeSalary = false;
        this.btnStateSalary = "Mở thêm thông tin lương nhân viên";
        this.checkSalary = false;
        this.checkMonthly = false;
        this.salarycomponents = [];
        this.monthlycomponents = [];
        this.payrollcomponents = [];
    }
    ngOnInit() {
    }
    stateSalary() {
        this.activeSalary = !this.activeSalary;
        if (this.activeSalary) {
            this.btnStateSalary = "Đóng thêm thông tin lương nhân viên";
        }
        else {
            this.btnStateSalary = "Mở thêm thông tin lương nhân viên";
        }
    }
    complete() {
        if (this.excelFileMonthly == null && this.excelFileSalary == null) {
            alert("Chưa upload file thông tin lương !");
        }
        else {
            // this.UploadSalary();
            this.UploadMonthly();
        }
    }
    onFileChange1(evt) {
        this.excelFileSalary = new SPCSObject;
        /* wire up file reader */
        const target = (evt.target);
        if (target.files.length !== 1)
            throw new Error('Cannot use multiple files');
        const reader = new FileReader();
        reader.onload = (e) => {
            /* read workbook */
            const bstr = e.target.result;
            const wb = XLSX.read(bstr, { type: 'binary' });
            /* grab first sheet */
            const wsname = wb.SheetNames[0];
            const ws = wb.Sheets[wsname];
            /* save data */
            this.excelFileSalary = (XLSX.utils.sheet_to_json(ws, { header: 1 }));
            this.checkSalary = true;
            console.log(this.excelFileSalary);
        };
        reader.readAsBinaryString(target.files[0]);
    }
    onFileChange(evt) {
        this.excelFileMonthly = new SPCSObject;
        /* wire up file reader */
        const target = (evt.target);
        if (target.files.length !== 1)
            throw new Error('Cannot use multiple files');
        const reader = new FileReader();
        reader.onload = (e) => {
            /* read workbook */
            const bstr = e.target.result;
            const wb = XLSX.read(bstr, { type: 'binary' });
            /* grab first sheet */
            const wsname = wb.SheetNames[0];
            const ws = wb.Sheets[wsname];
            /* save data */
            this.excelFileMonthly = (XLSX.utils.sheet_to_json(ws, { header: 1 }));
            this.checkMonthly = true;
            console.log(this.excelFileMonthly);
        };
        reader.readAsBinaryString(target.files[0]);
    }
    UploadSalary() {
        this.salarycomponents = [];
        for (let i = 3; i < this.excelFileSalary.length; i++) {
            const eli = this.excelFileSalary[i];
            for (let j = 2; j < eli.length; j++) {
                let fieldId = this.excelFileSalary[1][j];
                let employeeCode = eli[0];
                const elj = eli[j];
                this.salarycomponents.push({
                    EmpId: employeeCode,
                    FieldId: fieldId,
                    Value: elj,
                    ApplyDate: "2020-03-14T10:16:02.508Z",
                });
            }
        }
        console.log(this.salarycomponents);
        this.api.createSalaryComponents(this.salarycomponents)
            .subscribe(res => {
            alert("Nạp thành công");
            this.symbolModalRef.hide();
        }, err => {
            alert("File không hợp lệ");
            console.log(err);
        });
    }
    UploadMonthly() {
        this.payrollcomponents = [];
        this.monthlycomponents = [];
        // let start = 3;
        let startRow = 2;
        while (this.excelFileMonthly != null) {
            let el = this.excelFileMonthly[startRow];
            if (this.excelFileMonthly[startRow + 1][1] == "monthly") {
                break;
            }
            this.payrollcomponents.push({
                "PayrollId": this.payrollId,
                "FieldId": el[0],
                "Value": el[2] + ""
            });
            startRow++;
            if (this.excelFileMonthly[startRow + 2][1] == "monthly") {
                break;
            }
        }
        startRow += 2;
        if (this.excelFileMonthly != null)
            for (let i = startRow + 1; i < this.excelFileMonthly.length; i++) {
                const eli = this.excelFileMonthly[i];
                for (let j = 3; j < eli.length; j++) {
                    let fieldId = this.excelFileMonthly[startRow - 1][j];
                    let employeeCode = eli[1];
                    const elj = eli[j];
                    this.monthlycomponents.push({
                        EmpId: employeeCode,
                        PayrollId: this.payrollId,
                        FieldId: fieldId,
                        Value: elj + "",
                    });
                }
            }
        console.log(this.payrollcomponents);
        console.log(this.monthlycomponents);
        this.api.createMonthlySalaryComponents(this.monthlycomponents)
            .subscribe(res => {
            this.api.createPayrollComponents(this.payrollcomponents)
                .subscribe(res => {
                if (this.activeSalary) {
                    this.UploadSalary();
                }
                else {
                    alert("Nạp thành công");
                    this.symbolModalRef.hide();
                }
            }, err => {
                alert("File không hợp lệ");
                console.log(err);
            });
        }, err => {
            alert("File không hợp lệ");
            console.log(err);
        });
    }
    downloadSalary() {
        window.location.href = this.api.getApiUrl() + 'Payroll/GetExcelTemplate?status=1';
    }
    downloadMonthly() {
        window.location.href = this.api.getApiUrl() + 'Payroll/GetExcelTemplate?status=2';
    }
};
ModalMonthlyComponent = __decorate([
    Component({
        selector: 'modal-monthly',
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

                            <div style="margin-left: 5px; margin-top: 30px; width: 100%; clear: both;">
                                <h4 style="font-size: 18px;">Thông Tin Lương Hàng Tháng
                                    <i (click)="downloadMonthly()" style="cursor: pointer;" class="fa fa-download"></i>
                                </h4>
                                <input id="monthly" class="form-control" style="float: left; width: 60%;"
                                    (change)="onFileChange($event)" type="file">
                            </div>

                            <!-- <button style="margin-top: 2%" class="btn btn-light" (click)="stateSalary()">{{btnStateSalary}}</button> -->
                            <div style="width: 100%; height: 50px">
                            </div>
                            <i (click)="stateSalary()" style="font-size:30px; transition: 0.3s;" [ngStyle]="{transform: activeSalary ? 'rotate(90deg)' : ''}"
                            class="fa fa-angle-double-right pull-left"></i>
                            <ng-container *ngIf="activeSalary">
                            <div style="margin-left: 5px; width: 100%; clear: both;">
                                <h4 style="font-size: 18px;">Thông Tin Lương Nhân Viên
                                    <i (click)="downloadSalary()" style="cursor: pointer;" class="fa fa-download"></i>
                                </h4>
                                <input (change)="onFileChange1($event)" class="form-control"
                                    style="float: left; width: 60%;" type="file">
                            </div>
                            </ng-container>
                        </div>
                    </div>
                </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-success" (click)="complete()">
            <i class="fa fa-check-square-o"></i>
            Hoàn thành
          </button>
        </div>
    `,
    })
], ModalMonthlyComponent);
export { ModalMonthlyComponent };
//# sourceMappingURL=payroll.component.js.map