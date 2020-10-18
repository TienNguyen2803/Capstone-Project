import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
let DocumentComponent = class DocumentComponent {
    constructor(modalService, validator, api, route, router, util) {
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.formulasAPI = [];
        this.documentsAPI = [];
        this.documents = [];
        // page divide
        // pageQuantity = 7;
        // pageLength: any;
        // pageNum = 1;
        // pageNumArray: Number[] = [];
        this.documentToCreate = new SPCSObject;
    }
    ngOnInit() {
        this.role = localStorage.getItem('role');
        this.initDocumentToCreate();
        this.api.getAllDocument()
            .subscribe(res => {
            this.documentsAPI = res;
            for (let i = 0; i < this.documentsAPI.length; i++) {
                const el = this.documentsAPI[i];
                el.SignDate = this.util.formatDate(el.SignDate);
                el.ApplyDate = this.util.formatDate(el.ApplyDate);
                if (el.Status == 2)
                    el.State = "đang áp dụng";
                else {
                    el.State = "không áp dụng";
                }
                console.log(JSON.stringify(el));
                el.BtnDetail = `<i id = 'Print_` + el.Id + `' class='fa fa-angle-double-right fa-2x ast-black' onclick='location.href =\`` + `document/detail?documentId=` + JSON.stringify(el.Id) + `\`'></i>`;
            }
            this.documents = this.documentsAPI;
            console.log(this.documents);
            // call jquery at last
            this.jqueryInit();
        }, err => {
            console.log(err);
        });
        console.log(this.documents);
    }
    jqueryInit() {
        // this.jqueryDatePicker();
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
        var document_table = $('#document-datatable').DataTable({
            data: this.documents,
            columns: [
                { data: "Code" },
                { data: "State" },
                { data: "SignDate" },
                { data: "ApplyDate" },
                { data: "Description" },
                { data: "BtnDetail" }
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
            .appendTo('#document-datatable_wrapper .col-md-6:eq(0)');
    }
    filterData() {
        this.documents = this.documentsAPI.filter((el) => {
            // let temp1 = this.util.nonAccentVietnamese(this.txtSearch);
            // let temp2;
            // ID
            // temp2 = this.util.nonAccentVietnamese(el.AccessoryID);
            // if (temp2.indexOf(temp1) !== -1 || temp1.indexOf(temp2) !== -1) {
            //   return el;
            // }
        });
    }
    createDocument() {
        // format yyyy-mm-dd
        let SignDate = this.util.formatDateAPI(this.documentToCreate.SignDate);
        let ApplyDate = this.util.formatDateAPI(this.documentToCreate.ApplyDate);
        console.log(this.documentToCreate);
        const formData = new FormData();
        if (this.documentToCreate.Files != null) {
            formData.append("Files", this.documentToCreate.Files, this.documentToCreate.Files.name);
        }
        else {
            formData.append("Files", null);
        }
        formData.append("Code", this.documentToCreate.Code);
        formData.append("SignDate", SignDate);
        formData.append("ApplyDate", ApplyDate);
        formData.append("CloseDay", this.documentToCreate.CloseDay);
        formData.append("Deadline", this.documentToCreate.Deadline);
        formData.append("Description", this.documentToCreate.Description);
        this.api.createDocument(formData)
            .subscribe(res => {
            alert("Tạo quyết định thành công");
            this.ngOnInit();
            this.modalRef.hide();
        }, err => {
            // Đợi chị Hằng để tạo form confirm đè document
        });
    }
    onFileChange(files) {
        this.documentToCreate.Files = files.item(0);
    }
    initDocumentToCreate() {
        this.documentToCreate.Code = "";
        this.documentToCreate.SignDate = new Date();
        this.documentToCreate.ApplyDate = new Date();
        this.documentToCreate.CloseDay = 5;
        this.documentToCreate.Deadline = 10;
        this.documentToCreate.Description = "";
        this.documentToCreate.Files = null;
    }
    openCreateDocodal(createPair) {
        this.modalRef = this.modalService.show(createPair);
    }
};
DocumentComponent = __decorate([
    Component({
        selector: 'app-document',
        templateUrl: './document.component.html',
        styleUrls: ['./document.component.css']
    })
], DocumentComponent);
export { DocumentComponent };
//# sourceMappingURL=document.component.js.map