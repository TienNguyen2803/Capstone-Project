import { __awaiter, __decorate } from "tslib";
import { Component } from '@angular/core';
import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';
import Swal from 'sweetalert2';
let PayslipTemplateComponent = class PayslipTemplateComponent {
    constructor(api, route, router, fb) {
        this.api = api;
        this.route = route;
        this.router = router;
        this.fb = fb;
        // //Khai báo Form Group
        // selectTemplateForm = this.fb.group({
        //   //template_name: [''],
        //   document_name: ['']
        // })
        this.document = "";
        this.template = "";
        this.url = "";
        this.header = "";
        this.buttonText = "";
        this.DocumentId = "";
        //khai báo editor 
        this.Editor = DecoupledEditor;
        //Lấy data từ editor
        this.editorContent_update = '';
        //Danh sách field phiếu lương 
        this.listFields = null;
        //List Documnet
        this.listDocument = null;
        //Lây string Html load  editor 
        this.model = {
            editorData: ''
        };
        //Lưu file template xuống server
        this.flag = false;
        this.listFieldActive = [];
        this.listFieldClone = [];
    }
    //On init
    ngOnInit() {
        //check modal-backdrop fade show
        $('.modal-backdrop').remove();
        //reset data 
        this.editorContent_update = "";
        //get query param 
        this.route.queryParams.subscribe(params => {
            console.log(params);
            if (params["url"] == "create") {
                this.url = "/document/detail";
                this.header = "Tạo Phiếu Lương";
                this.buttonText = "Lưu Phiếu Lương";
            }
            else if (params["url"] == "edit") {
                document.getElementById("upload").classList.add('d-none');
                document.getElementById("upload-payslip").classList.add('d-none');
                this.url = "/document/detail";
                this.header = "Chỉnh sửa phiếu lương";
                this.buttonText = "Cập nhật ";
            }
            else if (params["url"] == "manager") {
                document.getElementById("upload").classList.add('d-none');
                document.getElementById("upload-payslip").classList.add('d-none');
                this.url = "/template";
                this.header = "Chỉnh sửa phiếu lương";
                this.buttonText = "Cập nhật ";
            }
            console.log(params["document"]);
            this.DocumentId = params["DocumentId"];
            if (params["document"] != undefined) {
                this.document = params["document"];
                this.template = params["template"];
                //load content editor
                this.StringContentEditor(this.template);
            }
        });
        //load list field
        this.ListField();
        this.Listdocumnet();
    }
    //onchange select 
    Change(value) {
        this.ListTemplateByDoc(value);
        if (this.Templates == undefined) {
            //document.getElementById("pop-up-table").style.visibility = "hidden";
        }
    }
    //Review Templates
    //Return html string to review template
    ReviewTemplate(name) {
        this.api.GetHtmlString(name)
            .subscribe(res => {
            const styleSheet = '<link rel="stylesheet" type="text/css" href="../../../wwwroot/assets/css/html-doc.css">';
            res = res.replace(`<figure class="table">`, `<figure>`);
            $(`#html-viewer`).html(styleSheet + '<br>' + res);
            // document.getElementById('trigger-modal').click();
        }, err => {
            console.log(err);
        });
    }
    onReady(editor) {
        editor.ui.getEditableElement().parentElement.insertBefore(editor.ui.view.toolbar.element, editor.ui.getEditableElement());
    }
    onChange({ editor }) {
        //const styleSheet = '<link rel="stylesheet" type="text/css" href="D:/Capstones/SCPSProject/CapstoneUI/src/assets/css/html-doc.css">';
        const styleSheet = '<link rel="stylesheet" type="text/css" href="../../wwwroot/assets/css/html-doc.css">';
        this.editorContent_update = styleSheet + '<br/>' + editor.getData();
        // this.editorContent_update = this.editorContent_update.replace(/<table/g,'<table border="1"');
        // console.log(this.editorContent_update);
    }
    SearchField(e) {
        this._listField = [];
        const array = e.target.value.split(" ");
        // this.listFields.forEach(element => {
        //   // if(element.toLowerCase().indexOf(e.target.value.toLowerCase()) !== -1){
        //   //   this._listField.push(element)
        //   // }
        // });
        this.listFields.forEach(element => {
            let compare = false;
            array.forEach(character => {
                if (element.toLowerCase().indexOf(character.toLowerCase()) !== -1) {
                    compare = true;
                }
                else {
                    compare = false;
                }
            });
            if (compare) {
                this._listField.push(element);
            }
        });
        console.log(this._listField);
    }
    ListField() {
        this.api.GetListFields(this.document)
            .subscribe(res => {
            this.listFields = res;
            this._listField = res;
        }, err => {
            console.log(err);
        });
    }
    Listdocumnet() {
        this.api.getAllDocument().subscribe(res => {
            this.listDocument = res;
        }, err => {
        });
    }
    ListTemplateByDoc(documentName) {
        this.api.GetListTemplate(documentName).subscribe(res => {
            console.log(res);
            //document.getElementById("pop-up-table").style.visibility = "visible";
            this.Templates = res;
        }, err => {
            console.log(err);
        });
    }
    //Back Url
    BackURL() {
        console.log(this.url == "/document/detail");
        console.log(this.DocumentId);
        if (this.url == "/document/detail") {
            location.href = `document/detail?documentId=${this.DocumentId}`;
        }
        else if (this.url == "/template") {
            this.router.navigate([this.url]);
        }
    }
    StringContentEditor(template) {
        this.api.GetHtmlString(template)
            .subscribe(res => {
            const styleSheet = '<link rel="stylesheet" type="text/css" href="../../wwwroot/assets/css/html-doc.css">';
            this.model.editorData = styleSheet + '<br/>' + res;
        }, err => {
            console.log(err);
        });
    }
    //Cập nhật nội dung file template
    SaveContentEditor() {
        return __awaiter(this, void 0, void 0, function* () {
            let value = "";
            if (this.template == undefined) {
                const { value: email } = yield Swal.fire({
                    title: 'Tên Phiếu Lương',
                    input: 'text',
                    inputPlaceholder: 'Nhập tên phiếu lương',
                    confirmButtonText: 'Lưu',
                });
                if (email == "" || email == undefined) {
                    Swal.fire('Tên phiếu lương không phù hợp!', "", 'error');
                    return;
                }
                else {
                    value = email;
                }
            }
            else {
                value = this.template;
            }
            //convert file
            var blob = new Blob([this.editorContent_update], { type: "text/html;charset=utf-8" });
            const file = new File([blob], value + ".html", { type: "text/html;charset=utf-8" });
            console.log(file);
            const formData = new FormData();
            formData.append('file', file);
            formData.append('template', this.document);
            console.log(this.document);
            if (this.template == undefined) {
                Swal.fire({
                    title: 'Bạn có muốn chọn làm mặc định?',
                    text: "",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Có',
                    cancelButtonText: 'Không',
                    customClass: {
                        confirmButton: 'swal2-confirm swal2-styled',
                        cancelButton: 'swal2-cancel swal2-styled'
                    },
                    reverseButtons: true
                }).then((result) => {
                    if (result.value) {
                        this.api.UploadTemplate(formData)
                            .subscribe(res => {
                            Swal.fire('Tạo Thành Công!', '', 'success');
                            this.BackURL();
                            //this.ListTemplate();
                        }, err => {
                            console.log(err);
                        });
                    }
                    else if (result.dismiss === Swal.DismissReason.cancel) {
                        console.log(formData);
                        this.api.UploadTemplateV2(formData)
                            .subscribe(res => {
                            Swal.fire('Tạo Thành Công!', '', 'success');
                            this.BackURL();
                            //this.ListTemplate();
                        }, err => {
                            Swal.fire('Tên phiếu lương đã tồn tại!', '', 'error');
                            console.log(err);
                        });
                    }
                });
            }
            else {
                if (this.flag) {
                    Swal.fire({
                        title: 'Bạn có muốn chọn làm mặc định?',
                        text: "",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonText: 'Có',
                        cancelButtonText: 'Không',
                        customClass: {
                            confirmButton: 'swal2-confirm swal2-styled',
                            cancelButton: 'swal2-cancel swal2-styled'
                        },
                        reverseButtons: true
                    }).then((result) => {
                        if (result.value) {
                            this.api.UploadTemplate(formData)
                                .subscribe(res => {
                                Swal.fire('Tạo Thành Công!', '', 'success');
                                console.log("dasdasdasdas");
                                this.BackURL();
                            }, err => {
                                console.log(err);
                            });
                        }
                        else if (result.dismiss === Swal.DismissReason.cancel) {
                            this.api.UploadTemplateV3(formData)
                                .subscribe(res => {
                                Swal.fire('Tạo Thành Công!', '', 'success');
                                this.BackURL();
                            }, err => {
                                Swal.fire('Tên phiếu lương đã tồn tại!', '', 'error');
                                console.log(err);
                            });
                        }
                    });
                }
                else {
                    this.api.TemplateDefault(this.document).subscribe(res => {
                        if (res == file.name) {
                            this.api.UploadTemplate(formData)
                                .subscribe(res => {
                                Swal.fire('Cập Nhật Thành Công!', '', 'success');
                                this.BackURL();
                            }, err => {
                                console.log(err);
                            });
                        }
                        else {
                            Swal.fire({
                                title: 'Bạn có muốn chọn làm mặc định?',
                                text: "",
                                icon: 'warning',
                                showCancelButton: true,
                                confirmButtonText: 'Có',
                                cancelButtonText: 'Không',
                                customClass: {
                                    confirmButton: 'swal2-confirm swal2-styled',
                                    cancelButton: 'swal2-cancel swal2-styled'
                                },
                                reverseButtons: true
                            }).then((result) => {
                                if (result.value) {
                                    this.api.UploadTemplate(formData)
                                        .subscribe(res => {
                                        Swal.fire('Cập Nhật Thành Công!', '', 'success');
                                        this.BackURL();
                                    }, err => {
                                        console.log(err);
                                    });
                                }
                                else if (result.dismiss === Swal.DismissReason.cancel) {
                                    console.log("dasdasdasdsad");
                                    this.api.UploadTemplateV3(formData)
                                        .subscribe(res => {
                                        Swal.fire('Cập Nhật Thành Công!', '', 'success');
                                        this.BackURL();
                                    }, err => {
                                        Swal.fire('Tên phiếu lương đã tồn tại!', '', 'error');
                                        console.log(err);
                                    });
                                }
                            });
                        }
                    }, err => {
                    });
                }
            }
        });
    }
    onSubmitFile(fileInput) {
        return __awaiter(this, void 0, void 0, function* () {
            const fileData = fileInput.target.files[0];
            // const { value: file } = await Swal.fire({
            //   title: 'Chọn Mẫu Phiếu Lương',
            //   input: 'file',  confirmButtonText: 'Tải lên'
            // })
            // if (file != null) {
            const formData = new FormData();
            formData.append('file', fileData);
            formData.append('template', this.document);
            // const formData = new FormData();
            // formData.append('file', 'test');
            // formData.append('template', this.document);
            this.api.UploadTemplateV2(formData)
                .subscribe(res => {
                let name = fileData.name;
                this.template = name.split(".")[0];
                this.StringContentEditor(this.template);
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Tải lên thành công !',
                    showConfirmButton: false,
                    timer: 1500
                });
                this.flag = true;
            }, err => {
                Swal.fire('Phiếu lương đã tồn tại !', "", 'error');
            });
            // }  
        });
    }
    CloneTemplate() {
        return __awaiter(this, void 0, void 0, function* () {
            this.api.GetListFields(this.document).subscribe(res => {
                this.listFieldActive = res;
                this.api.GetListFields($("#doc-selected").val()).subscribe(res => {
                    res.forEach(element => {
                        if (this.listFieldActive.includes(element) === false) {
                            this.listFieldClone.push(element);
                        }
                    });
                    let htmlValue = $('#html-viewer').html();
                    this.listFieldClone.forEach(element => {
                        const re = new RegExp(element, 'g');
                        htmlValue = htmlValue.replace(re, '');
                    });
                    this.model.editorData = htmlValue;
                }, err => {
                    console.log(err);
                });
            }, err => {
                console.log(err);
            });
            // console.log(this.listFieldActive)
            // this.listFieldClone.forEach(element => {
            //   console.log(element)
            // });
            // this.listFieldClone.forEach(element => {
            //   console.log(element)
            //     // if(this.listFieldActive.includes(element) === true){
            //     //   this.result.push(element);
            //     // }
            // });
            // console.log(this.result)
            // //   this.api.GetListFields($("#doc-selected").val()).subscribe(
            //   res => {
            //     let htmlValue = $('#html-viewer').html();
            //     res.forEach(element => {
            //       const re = new RegExp(element, 'g');
            //       htmlValue = htmlValue.replace(re, '');
            //     });
            //     this.model.editorData = htmlValue;
            //     console.log(htmlValue);
            //   }, err => {
            //     console.log(err)
            //   }
            // );
        });
    }
};
PayslipTemplateComponent = __decorate([
    Component({
        selector: 'app-payslip-template',
        templateUrl: './payslip-template.component.html',
        styleUrls: ['./payslip-template.component.css']
    })
], PayslipTemplateComponent);
export { PayslipTemplateComponent };
//# sourceMappingURL=payslip-template.component.js.map