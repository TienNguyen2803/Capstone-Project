import { __awaiter, __decorate } from "tslib";
import { Component } from '@angular/core';
//import * as $ from 'jquery';
import Swal from 'sweetalert2';
let ManagePayslipComponent = class ManagePayslipComponent {
    constructor(api, route, router, fb) {
        this.api = api;
        this.route = route;
        this.router = router;
        this.fb = fb;
        this.p = 1;
        //Onchange danh sách document
        this.documentName = '';
        //Show list template 
        this.TemplateUrl = "";
        //Get Document Code Active
        // DocumentActive(){
        //   this.api.GetDocumentActive().subscribe(
        //     res => {
        //       this.documentName = res.Code;
        //       this.GetListTemplate();
        //     },err => {
        //       console.log(err) 
        //     }
        //   )
        // }
        //Khai báo Form Group
        this.selectTemplateForm = this.fb.group({
            template_name: [''],
            document_name: ['']
        });
        //Danh sách Document 
        //Khai báo biến danh sách document
        this.listDocument = null;
        //GetAll Template
        this.listAllTemplate = [];
        //Back Url
        this.url = "manager";
    }
    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            if (params["document"] != undefined) {
                this.documentName = params["document"];
                //get list template
                this.GetAlltemplate();
            }
            else {
                //this.DocumentActive();
            }
        });
        $(document).on('click', '[data-event="show-modal"]', (event) => {
            const $obj = $(event.target);
            this.ReviewTemplate($obj.data('code'), $obj.data('templateurl'), $obj.data('index'));
        });
        $(document).on('click', '[data-event="set-status"]', (event) => {
            const $obj = $(event.target);
            this.SetStatus($obj.data('templateurl'), $obj.data('code'));
        });
        $(document).on('click', '[data-event="edit-template"]', (event) => {
            const $obj = $(event.target);
            window.location.href = `/payslip-template?template=${$obj.data('templateurl')}&url=${this.url}&document=${$obj.data('code')}`;
        });
        $(document).on('click', '[data-event="view-datademo"]', (event) => {
            const $obj = $(event.target);
            this.ViewDataDemo($obj.data('code'), $obj.data('templateurl'), $obj.data('index'));
        });
        this.GetAlltemplate();
        //get list document
        this.ListDocument();
    }
    changeDocumentItems(e) {
        this.selectTemplateForm.get('document_name').setValue(e.target.value, {
            onlySelf: true
        });
        this.documentName = this.selectTemplateForm.get('document_name').value;
        //this.GetListTemplate();
        console.log(this.selectTemplateForm.get('document_name').value);
    }
    //Onchange danh sách document 
    changeTemplateItems(e) {
        this.selectTemplateForm.get('template_name').setValue(e.target.value, {
            onlySelf: true
        });
        document.getElementById("panel").style.visibility = "visible";
        console.log(this.selectTemplateForm.get('document_name').value);
        //load content editor
        //this.StringContentEditor();
    }
    showListTemplate() {
        return __awaiter(this, void 0, void 0, function* () {
            var myArrayOfThings = [];
            for (let index = 0; index < this.listDocument.length; index++) {
                myArrayOfThings.push({ id: this.listDocument[index], name: this.listDocument[index] });
            }
            var options = {};
            $.map(myArrayOfThings, function (o) {
                options[o.id] = o.name;
            });
            const { value: fruit } = yield Swal.fire({
                title: 'Quyết Định',
                input: 'select',
                confirmButtonText: 'Tìm',
                inputOptions: options
            });
            if (fruit) {
                document.getElementById("search").style.visibility = "visible";
                document.getElementById("refresh").style.visibility = "visible";
                this.selectTemplateForm.get('document_name').setValue(fruit);
                this.documentName = fruit;
                this.GetListTemplate(fruit);
            }
        });
    }
    ListDocument() {
        this.api.GetListNameDocument()
            .subscribe(res => {
            this.listDocument = res;
            console.log(res);
        }, err => {
            console.log(err);
        });
    }
    refresh() {
        this.GetAlltemplate();
    }
    //Khai báo danh sách template 
    GetListTemplate(document) {
        this.api.GetListTemplate(document)
            .subscribe(res => {
            this.listAllTemplate = res;
        }, err => {
            console.log(err);
        });
    }
    GetAlltemplate() {
        this.api.GetAllTemplate().subscribe(res => {
            // document.getElementById("refresh").style.visibility = "hidden";
            // document.getElementById("search").style.visibility = "hidden";
            this.listAllTemplate = res;
            // jquery for datatable
            var document_table = $('#payroll-datatable').DataTable({
                data: res,
                language: {
                    "decimal": "",
                    "emptyTable": "Không có quyết định nào trong hệ thống",
                    "lengthMenu": "Hiển Thị _MENU_ Phiếu Lương",
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
                    },
                },
                columns: [
                    {
                        data: 'TemplateUrl'
                    },
                    {
                        data: 'Code'
                    },
                    {
                        data: 'Status',
                        mRender: function (data, type, row) {
                            return (row.Status === true) ? 'Bản Chính' : 'Bản Nháp';
                        }
                    },
                    {
                        mRender: function (data, type, row) {
                            return `<i data-event="view-datademo"  data-id="#exampleModal${row.Id}"  style="cursor: pointer;margin-left: 10px !important;font-size: 30px !important;" data-toggle="modal" data-target="#exampleModal${row.Id}" data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}" data-index="${row.Id}" class="fa fa-eye view-btn"></i>
                        <i  data-event="set-status" style="cursor: pointer;margin-left: 10px !important;font-size: 30px !important;" data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}"   class="fa fa-check"></i>
                        <i data-event="edit-template"  data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}"  style="cursor: pointer;margin-left: 10px !important;font-size: 30px !important;" class="fa fa-edit fa-2x ast-yellow"></i></a>
                        <div class="modal fade" id="exampleModal${row.Id}"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg" role="document" >
                            <div class="modal-content">
                                <div class="modal-header" style="display: contents !important;">
                                    <h5 class="modal-title" id="exampleModalLongTitle" style="margin-left: 20px;font-weight: bold;margin-top: 20px;">Phiếu lương : ${row.TemplateUrl}</h5>
                                </div>
                                <div class="modal-body" style="height: 500px;overflow-y: scroll;">
                                    <div class="viewer" id="html-viewer${row.Id}">

                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button  data-event="set-status" data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}"  type="button" class="btn btn-secondary" data-dismiss="modal" (click)="SetStatus(item.TemplateUrl,item.Code)">Sử dụng</button>
                                    <button  data-event="edit-template" data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}"   type="button" class="btn btn-primary">Chỉnh Sửa</button>
                                </div>
                            </div>
                        </div>
                    </div> `;
                        }
                    }
                ]
            });
            // <button data-event="view-datademo" type="button" id="btn-review${row.Id}" style="visibility: hidden;" class="btn btn-secondary" data-code="${row.Code}" data-templateUrl="${row.TemplateUrl}" >Xem với dữ liệu mẫu</button>
            // document_table.buttons().container()
            //   .appendTo('#document-datatable_wrapper .col-md-6:eq(0)');
        }, err => {
            console.log(err);
        });
    }
    ViewDataDemo(code, template, index) {
        this.api.ViewDataDemo(code, template + ".html").subscribe(res => {
            $(`#html-viewer${index}`).html(res);
        }, err => {
        });
    }
    //Return html string to review template
    ReviewTemplate(code, name, indexId) {
        console.log(code + " " + name + " " + indexId);
        this.api.GetHtmlString(name)
            .subscribe(res => {
            this.index = indexId;
            this.api.IsDocumentFormularId(code).subscribe(res => {
                if (res == "True") {
                    document.getElementById(`btn-review${indexId}`).style.visibility = "visible";
                }
            }, err => {
                console.log(err);
            });
            $(`#html-viewer${indexId}`).html(res);
        }, err => {
            console.log(err);
        });
    }
    //Set status Template Email
    SetStatus(name, code) {
        this.api.SetStatus(name, code)
            .subscribe(res => {
            $('#payroll-datatable').DataTable().destroy();
            this.GetAlltemplate();
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Chọn thành công!',
                showConfirmButton: false,
                timer: 1500
            });
        }, err => {
            console.log(err);
        });
    }
};
ManagePayslipComponent = __decorate([
    Component({
        selector: 'app-manage-payslip',
        templateUrl: './manage-payslip.component.html',
        styleUrls: ['./manage-payslip.component.css']
    })
], ManagePayslipComponent);
export { ManagePayslipComponent };
//# sourceMappingURL=manage-payslip.component.js.map