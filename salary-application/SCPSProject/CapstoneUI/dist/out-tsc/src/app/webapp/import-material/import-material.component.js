import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
import * as XLSX from 'xlsx';
import { combineLatest } from 'rxjs';
let ImportMaterialComponent = class ImportMaterialComponent {
    constructor(changeDetection, modalService, validator, api, route, router, util) {
        this.changeDetection = changeDetection;
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.salarycomponents = [];
    }
    ngOnInit() {
        this.getEmpSalaryComponents();
    }
    getEmpSalaryComponents() {
        this.api.getEmpSalaryComponents()
            .subscribe(res => {
            this.empSalaryComponent = res;
            this.jqueryDataTable();
        }, err => {
        });
    }
    jqueryDataTable() {
        var cols = [
            { sTitle: "MÃ", data: "Code" },
            { sTitle: "HỌ VÀ TÊN", data: "Fullname" },
        ];
        for (let k = 0; k < this.empSalaryComponent.emps.length; k++) {
            const element = this.empSalaryComponent.emps[k];
            for (let i = 0; i < element.SalaryComponents.length; i++) {
                var el = element.SalaryComponents[i];
                el.Value = this.util.commaForBigNum2(el.Value);
            }
        }
        for (let i = 0; i < this.empSalaryComponent.emps[0].SalaryComponents.length; i++) {
            var el = this.empSalaryComponent.emps[i].SalaryComponents[i];
            cols.push({ sTitle: el.FieldName, data: ("SalaryComponents." + i + ".Value") });
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
        var subscriptions = [];
        const initialState = {};
        this.modalRef = this.modalService.show(ModalSalaryComponent, { initialState });
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
ImportMaterialComponent = __decorate([
    Component({
        selector: 'app-import-material',
        templateUrl: './import-material.component.html',
        styleUrls: ['./import-material.component.css']
    })
], ImportMaterialComponent);
export { ImportMaterialComponent };
let ModalSalaryComponent = class ModalSalaryComponent {
    constructor(symbolModalRef, api) {
        this.symbolModalRef = symbolModalRef;
        this.api = api;
        this.salarycomponents = [];
    }
    ngOnInit() {
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
            console.log(this.excelFileSalary);
        };
        reader.readAsBinaryString(target.files[0]);
    }
    UploadSalary() {
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
            location.href = "importmaterial?Hash=";
        }, err => {
            alert("File không hợp lệ");
            console.log(err);
        });
    }
    downloadSalary() {
        window.location.href = this.api.getApiUrl() + 'Payroll/GetExcelTemplate?status=1';
    }
};
ModalSalaryComponent = __decorate([
    Component({
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
], ModalSalaryComponent);
export { ModalSalaryComponent };
//# sourceMappingURL=import-material.component.js.map