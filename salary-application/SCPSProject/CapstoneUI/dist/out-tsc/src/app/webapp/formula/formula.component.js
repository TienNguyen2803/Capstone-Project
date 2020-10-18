import { __decorate } from "tslib";
import { Component } from '@angular/core';
let FormulaComponent = class FormulaComponent {
    constructor(validator, api, route, router, util) {
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.formulasAPI = [];
        this.formulas = [];
        // page divide
        this.pageQuantity = 7;
        this.pageNum = 1;
        this.pageNumArray = [];
        // formula details
        this.formulaDetails = new Array(1);
        this.index = 0;
    }
    ngOnInit() {
        this.api.getAllFormulas()
            .subscribe(res => {
            this.formulas = res;
            for (let i = 0; i < this.formulas.length; i++) {
                const el = this.formulas[i];
                el.CreateDate = this.util.formatDate(el.CreateDate);
            }
            console.log(this.formulas);
            this.jqueryInit();
        }, err => {
            console.log(err);
        });
    }
    jqueryInit() {
        this.jqueryDatePicker();
        this.jqueryDataTable();
    }
    jqueryDatePicker() {
        // date single picker
        $('.autoclose-datepicker').datepicker({
            autoclose: true,
            todayHighlight: true,
            format: 'dd/mm/yyyy',
            formatSubmit: 'yyyy-mm-dd',
        });
    }
    jqueryDataTable() {
        // jquery for datatable
        var document_table = $('#formula-datatable').DataTable({
            data: this.formulas,
            columns: [
                { data: "Name" },
                { data: "CreateDate" },
                { data: "Type" },
                { data: "Formula" },
            ],
            destroy: true,
            lengthChange: false,
            buttons: ['colvis'],
            pageLength: 8,
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
            .appendTo('#formula-datatable_wrapper .col-md-6:eq(0)');
    }
    filterData() {
        this.formulas = this.formulasAPI.filter((el) => {
            let temp1 = this.util.nonAccentVietnamese(this.txtSearch);
            let temp2;
            // ID
            // temp2 = this.util.nonAccentVietnamese(el.AccessoryID);
            // if (temp2.indexOf(temp1) !== -1 || temp1.indexOf(temp2) !== -1) {
            //   return el;
            // }
        });
    }
};
FormulaComponent = __decorate([
    Component({
        selector: 'app-formula',
        templateUrl: './formula.component.html',
        styleUrls: ['./formula.component.css']
    })
], FormulaComponent);
export { FormulaComponent };
//# sourceMappingURL=formula.component.js.map