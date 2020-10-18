import { __awaiter, __decorate } from "tslib";
import { Component } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
import Swal from 'sweetalert2';
let DocumentDetailComponent = class DocumentDetailComponent {
    constructor(modalService, validator, api, route, router, util, fb) {
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.fb = fb;
        this.vm = this;
        // page divide
        this.pageQuantity = 9;
        this.pageNum = 1;
        this.pageNumArray = [];
        this.documentDetail = new SPCSObject;
        this.documentToUpdate = new SPCSObject;
        //#endregion
        //Tiền Code
        //khai báo documentName
        this.url = "create";
        this.urlEdit = "edit";
        this.documentName = "";
        //Khai báo Form Group
        this.TemplateUrl = "";
        //Khai báo danh sách template 
        this.listNameTemplate = null;
        //Get template default
        this.templateDefault = "";
    }
    ngOnInit() {
        this.route.queryParamMap.subscribe(params => {
            this.documentId = JSON.parse(params.get('documentId'));
            console.log(this.documentId);
            this.getDocumentById(this.documentId);
        });
    }
    openViewDocModal() {
        const initialState = {
            link: this.documentDetail.DocumentUrl,
        };
        this.modalRef = this.modalService.show(ModalMonthlyComponent, { initialState: initialState, class: "modal-lg" });
    }
    //#region Document
    getDocumentById(Id) {
        this.api.getDocumentById(Id)
            .subscribe(res => {
            this.documentDetail = res;
            this.documentToUpdate = this.documentDetail;
            this.api.getImageURL(this.documentDetail.Code)
                .subscribe(res => {
                this.documentDetail.DocumentUrl = res.DocumentUrl;
                console.log(res);
            }, err => {
                alert("Có lỗi khi tìm kiếm file hình");
                console.log(err);
            });
            //#region Tiền code
            this.documentName = this.documentDetail.Code;
            this.GetListTemplate();
            this.GetTemplateDefault();
            //#endregion
            console.log(res);
        }, err => {
            console.log(err);
        });
    }
    GetListTemplate() {
        console.log(this.documentName);
        this.api.GetListTemplate(this.documentName)
            .subscribe(res => {
            this.listNameTemplate = res;
        }, err => {
            console.log(err);
        });
    }
    //Return html string to review template
    ReviewTemplate(name) {
        this.api.GetHtmlString(name)
            .subscribe(res => {
            //$('#select-payslip').val('');
            $("#html-viewer").html(res);
            $('#exampleModal').modal('show');
        }, err => {
            console.log(err);
        });
    }
    //Show list template 
    showListTemplate() {
        return __awaiter(this, void 0, void 0, function* () {
            var myobj;
            var myArrayOfThings = [];
            for (let index = 0; index < this.listNameTemplate.length; index++) {
                myobj = JSON.parse(JSON.stringify(this.listNameTemplate[index]));
                myArrayOfThings.push({ id: myobj.TemplateUrl, name: myobj.TemplateUrl });
            }
            var options = {};
            $.map(myArrayOfThings, function (o) {
                options[o.id] = o.name;
            });
            const { value: fruit } = yield Swal.fire({
                title: 'Chọn Mẫu Phiếu Lương',
                input: 'select',
                confirmButtonText: 'Xem',
                inputOptions: options
            });
            if (fruit) {
                this.TemplateUrl = fruit;
                // this.api.IsDocumentFormularId(this.documentName).subscribe(
                //   res => {
                //     if(res == "True" ){
                //       document.getElementById(`btn-review`).style.visibility ="visible";
                //     }
                //     this.ReviewTemplate(this.TemplateUrl);
                //   },err => {
                //     console.log(err) 
                //   }
                // );
                this.ViewDataDemo(this.documentName, fruit);
            }
        });
    }
    GetTemplateDefault() {
        this.api.TemplateDefault(this.documentName)
            .subscribe(res => {
            this.templateDefault = res.split(".")[0];
        }, err => {
            this.templateDefault = "Chưa có mẫu phiếu lương.";
            console.log(this.templateDefault);
            console.log(err);
        });
    }
    //ViewDataDemo
    ViewDataDemo(code, template) {
        this.api.ViewDataDemo(code, template + ".html").subscribe(res => {
            console.log('.swal2-confirm.swal2-styled');
            $(`#html-viewer`).html(res);
            document.getElementById('trigger-modal').click();
            // $('#exampleModal').modal('show');
        }, err => {
        });
    }
    //Set status Template Email
    SetStatus(name) {
        this.api.SetStatus(name, this.documentName)
            .subscribe(res => {
            this.GetListTemplate();
            this.GetTemplateDefault();
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
DocumentDetailComponent = __decorate([
    Component({
        selector: 'app-document-detail',
        templateUrl: './document-detail.component.html',
        styleUrls: ['./document-detail.component.css']
    })
], DocumentDetailComponent);
export { DocumentDetailComponent };
let ModalMonthlyComponent = class ModalMonthlyComponent {
    constructor(sanitizer, modalRef, api) {
        this.sanitizer = sanitizer;
        this.modalRef = modalRef;
        this.api = api;
    }
    ngOnInit() {
    }
    photoURL() {
        let result = this.sanitizer.bypassSecurityTrustResourceUrl(this.link);
        return result;
    }
};
ModalMonthlyComponent = __decorate([
    Component({
        selector: 'modal-monthly',
        template: `
        <div class="modal-header">
            <h5 class="modal-title" style="font-size = 20px"><b>Quyết Định</b></h5>
            <button type="button" class="close" (click)="modalRef.hide()" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="modal-body">
              <div class="row" style="width: 100%;">
                    <div class="card" style="width: 100%;">
                        <div class="card-body" style="width: 100%;">
                            <!-- Tạo nội dung vào đây -->
                            <iframe frameborder="0" scrolling="no" style="width: 720px;height: 400px;"
                            [src]='photoURL()' name="imgbox" id="imgbox" 
                            webkitallowfullscreen mozallowfullscreen allowfullscreen>
                            <p>iframes are not supported by your browser.</p>
                         </iframe>
                        </div>
                    </div>
              </div>
        </div>
    `,
    })
], ModalMonthlyComponent);
export { ModalMonthlyComponent };
//# sourceMappingURL=document-detail.component.js.map