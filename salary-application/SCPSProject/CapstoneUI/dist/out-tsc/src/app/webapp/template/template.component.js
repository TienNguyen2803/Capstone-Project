import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { MailObject } from 'src/app/class/MailObject';
import * as $ from 'jquery';
let TemplateComponent = class TemplateComponent {
    //khai báo contructor
    constructor(api, route, router, fb) {
        this.api = api;
        this.route = route;
        this.router = router;
        this.fb = fb;
        //Khai báo Form Group
        this.selectTemplateForm = this.fb.group({
            name: [''],
            documentName: ['']
        });
        //tạo list Name template và list name document 
        this.listNameTemplate = null;
        this.listDocument = null;
        this.listFields = null;
        this.temp = null;
        this.fileData = null;
        this.ob = new MailObject;
    }
    changeDocument(e) {
        this.selectTemplateForm.get('documentName').setValue(e.target.value, {
            onlySelf: true
        });
        //set value default template
        this.api.TemplateDefault(this.selectTemplateForm.get('documentName').value)
            .subscribe(res => {
            console.log(res);
            const temp = res;
            this.api.LoadDocumetTemplate(res.split(".")[0])
                .subscribe(res => {
                console.log(res);
                const blob = new Blob([res], { type: 'application/octet-stream' });
                const file = new File([blob], temp, { type: 'application/octet-stream' });
                console.log(file);
                loadDocument(file);
            }, err => {
                console.log(err);
            });
        }, err => {
            console.log(err);
        });
        //get list field
        this.api.GetListFields(this.selectTemplateForm.get('documentName').value)
            .subscribe(res => {
            this.listFields = res;
        }, err => {
            console.log(err);
        });
        // get list Name Template
        this.api.GetListTemplate(this.selectTemplateForm.get('documentName').value)
            .subscribe(res => {
            this.listNameTemplate = res;
            // setTimeout(function () {
            //   //console.log($("#select-spaylip").val())
            //   this.temp = $("#select-spaylip").val();
            // }, 3000);
        }, err => {
            console.log(err);
        });
        console.log(this.selectTemplateForm.get('documentName').value);
    }
    ngOnInit() {
        // get list Name Document  
        this.api.GetListNameDocument()
            .subscribe(res => {
            this.listDocument = res;
            //  this.selectTemplateForm.get('documentName').setValue(this.listDocument[0], {
            //   onlySelf: true
            // })
            console.log(res);
        }, err => {
            console.log(err);
        });
    }
    fileProgress(fileInput) {
        this.fileData = fileInput.target.files[0];
    }
    onSubmitFile() {
        console.log(this.selectTemplateForm.get('documentName').value);
        const formData = new FormData();
        formData.append('file', this.fileData);
        formData.append('template', this.selectTemplateForm.get('documentName').value);
        this.api.UploadTemplate(formData)
            .subscribe(res => {
            this.api.GetListTemplate(this.selectTemplateForm.get('documentName').value)
                .subscribe(res => {
                this.listNameTemplate = res;
                console.log(res);
            }, err => {
                console.log(err);
            });
        }, err => {
            console.log(err);
        });
    }
    changeSuit(e) {
        this.selectTemplateForm.get('name').setValue(e.target.value, {
            onlySelf: true
        });
        console.log(this.selectTemplateForm.get('name').value);
        this.api.TemplateDefault(this.selectTemplateForm.get('documentName').value)
            .subscribe(res => {
            console.log(res);
            const temp = res;
            this.api.LoadDocumetTemplate(this.selectTemplateForm.get('name').value)
                .subscribe(res => {
                console.log(res);
                const blob = new Blob([res], { type: 'application/octet-stream' });
                const file = new File([blob], temp, { type: 'application/octet-stream' });
                console.log(file);
                loadDocument(file);
            }, err => {
                console.log(err);
            });
        }, err => {
            console.log(err);
        });
        //get html string
        this.api.GetHtmlString(this.selectTemplateForm.get('name').value)
            .subscribe(res => {
            // console.log( res)
            WordtoHtml(res);
        }, err => {
            console.log(err);
        });
    }
    SendPayslip() {
        this.api.SendPayslip(this.selectTemplateForm.get('name').value)
            .subscribe(res => {
            remove();
            console.log(res);
        }, err => {
            console.log(err);
        });
    }
    //Edit template
    onClickSave() {
        const that = this;
        saveDocument(function (document) {
            console.log(document);
            that.api.TemplateDefault(that.selectTemplateForm.get('documentName').value)
                .subscribe(res => {
                console.log(that.selectTemplateForm.get('name').value + ".docx");
                that.ob.Base64 = document;
                that.ob.FileName = that.selectTemplateForm.get('name').value + ".docx";
                that.api.UploadDocx(that.ob)
                    .subscribe(res => {
                    alert("Tạo phiếu lương thành công");
                    that.api.GetListTemplate(that.selectTemplateForm.get('documentName').value)
                        .subscribe(res => {
                        that.listNameTemplate = res;
                        console.log(res);
                    }, err => {
                        console.log(err);
                    });
                }, err => {
                    console.log(err);
                });
            }, err => {
                console.log(err);
            });
        });
    }
    onClickAddMergeField() {
        addMergeField();
    }
    LoadTemplateHtml() {
        document.getElementById("editor").style.visibility = "hidden";
        document.getElementById("editor").style.height = "0px";
        document.getElementById("html").style.visibility = "visible";
        document.getElementById("field").style.visibility = "visible";
        document.getElementById("btnBack").style.visibility = "visible";
        document.getElementById("btnCreate").style.visibility = "hidden";
        remove();
        document.getElementById("html-viewer").innerHTML = "";
        // //set value default template
        // this.api.TemplateDefault(this.selectTemplateForm.get('documentName').value)
        //   .subscribe(
        //     res => {
        //       console.log(res)
        //       //get html string
        //       this.api.GetHtmlString(res)
        //         .subscribe(
        //           res => {
        //             // console.log( res)
        //             WordtoHtml(res);
        //           },
        //           err => {
        //             console.log(err);
        //           }
        //         );
        //     },
        //     err => {
        //       console.log(err);
        //     }
        //   );
    }
    LoadTemplateTxt() {
        document.getElementById("editor").style.visibility = "visible";
        document.getElementById("editor").style.height = "700px";
        document.getElementById("html").style.visibility = "hidden";
        document.getElementById("field").style.visibility = "hidden";
        document.getElementById("btnBack").style.visibility = "hidden";
        document.getElementById("btnCreate").style.visibility = "visible";
        remove();
        //get template txtcontroll
        // //set value default template
        // this.api.TemplateDefault(this.selectTemplateForm.get('documentName').value)
        //   .subscribe(
        //     res => {
        //       const temp = res;
        //       console.log(res)
        //       this.api.LoadDocumetTemplate(res)
        //         .subscribe(
        //           res => {
        //             console.log(res);
        //             const blob = new Blob([res], { type: 'application/octet-stream' });
        //             const file = new File([blob], temp, { type: 'application/octet-stream' });
        //             console.log(file);
        //             loadDocument(file);
        //           },
        //           err => {
        //             console.log(err);
        //           }
        //         );
        //     },
        //     err => {
        //       console.log(err);
        //     }
        //   );
    }
    SubmitFileHtml() {
        var blob = new Blob([$("#html-viewer").html()], { type: "text/html;charset=utf-8" });
        const file = new File([blob], this.selectTemplateForm.get('name').value + ".html", { type: "text/html;charset=utf-8" });
        console.log(file);
        const formData = new FormData();
        formData.append('file', file);
        formData.append('template', this.selectTemplateForm.get('documentName').value);
        this.api.UploadTemplate(formData)
            .subscribe(res => {
            alert("Tạo phiếu lương thành công .");
            this.api.GetListTemplate(this.selectTemplateForm.get('documentName').value)
                .subscribe(res => {
                this.listNameTemplate = res;
                console.log(res);
            }, err => {
                console.log(err);
            });
        }, err => {
            console.log(err);
        });
    }
};
TemplateComponent = __decorate([
    Component({
        selector: 'app-template',
        templateUrl: './template.component.html',
        styleUrls: ['./template.component.css']
    })
], TemplateComponent);
export { TemplateComponent };
//# sourceMappingURL=template.component.js.map