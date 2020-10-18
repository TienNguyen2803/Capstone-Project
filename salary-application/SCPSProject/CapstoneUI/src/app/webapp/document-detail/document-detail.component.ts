import { Component, OnInit, ViewChild } from '@angular/core';
import { PrimitiveService } from 'src/app/primitive.service';
import { ApiService } from 'src/app/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/util.service';
import { SPCSObject } from 'src/app/class/SPCSObject';
import { FormBuilder } from '@angular/forms';
import Swal from 'sweetalert2'
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DomSanitizer } from '@angular/platform-browser';

declare var $: any;

@Component({
  selector: 'app-document-detail',
  templateUrl: './document-detail.component.html',
  styleUrls: ['./document-detail.component.css']
})
export class DocumentDetailComponent implements OnInit {

  vm = this;

  Hash: any;
  UserID: any;
  documentId: any;

  txtSearch: any;

  // page divide
  pageQuantity = 9;
  pageLength: any;
  pageNum = 1;
  pageNumArray: Number[] = [];

  documentDetail = new SPCSObject;
  documentToUpdate = new SPCSObject;

  modalRef: BsModalRef;

  constructor(private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService, public fb: FormBuilder) {

  }

  ngOnInit(): void {
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

    this.modalRef = this.modalService.show(
      ModalMonthlyComponent, { initialState: initialState, class: "modal-lg" }
    );

  }

  //#region Document

  getDocumentById(Id) {
    this.api.getDocumentById(Id)
      .subscribe(res => {
        this.documentDetail = res;
        this.documentToUpdate = this.documentDetail;
        this.api.getImageURL(this.documentDetail.Code)
          .subscribe(
            res => {
              this.documentDetail.DocumentUrl = res.DocumentUrl;
              console.log(res);
            },
            err => {
              alert("Có lỗi khi tìm kiếm file hình");
              console.log(err);
            }
          )

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

  //#endregion

  //Tiền Code
  //khai báo documentName
  url: string = "create";
  urlEdit: string = "edit";
  documentName: string = "";
  //Khai báo Form Group
  TemplateUrl: string = "";

  //Khai báo danh sách template 
  listNameTemplate: string[] = null;
  GetListTemplate() {
    console.log(this.documentName)
    this.api.GetListTemplate(this.documentName)
      .subscribe(
        res => {
          this.listNameTemplate = res;
        },
        err => {
          console.log(err);
        }
      );
  }
  //Return html string to review template
  ReviewTemplate(name) {
    this.api.GetHtmlString(name)
      .subscribe(
        res => {
          //$('#select-payslip').val('');
          // $("#html-viewer").html(res);
          // $('#exampleModal').modal('show');
          $(`#html-viewer`).html(res);
          document.getElementById('trigger-modal').click();
  
        },
        err => {
          console.log(err);
        }
      );
  }
  //Show list template 
  async showListTemplate() {
    var myobj;
    var myArrayOfThings = [];
    for (let index = 0; index < this.listNameTemplate.length; index++) {
      myobj = JSON.parse(JSON.stringify(this.listNameTemplate[index]));
      myArrayOfThings.push({ id: myobj.TemplateUrl, name: myobj.TemplateUrl })
    }
    var options = {};
    $.map(myArrayOfThings,
      function (o) {
        options[o.id] = o.name;
      });
    const { value: fruit } = await Swal.fire({
      title: 'Chọn Mẫu Phiếu Lương',
      input: 'select',
      confirmButtonText: 'Xem',
      inputOptions: options
    })

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
      //this.ViewDataDemo(this.documentName, fruit);
      this.ReviewTemplate(fruit);
    }
  }
  //Get template default
  templateDefault: string = "";
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
    this.api.ViewDataDemo(code, template + ".html").subscribe(
      res => {
        console.log('.swal2-confirm.swal2-styled');
        $(`#html-viewer`).html(res);
        document.getElementById('trigger-modal').click();
        // $('#exampleModal').modal('show');
      }, err => {

      }
    )
  }
  //Set status Template Email
  SetStatus(name) {
    this.api.SetStatus(name, this.documentName)
      .subscribe(
        res => {
          this.GetListTemplate();
          this.GetTemplateDefault();
          Swal.fire({
            position: 'center',
            icon: 'success',
            title: 'Chọn thành công!',
            showConfirmButton: false,
            timer: 1500
          })

        },
        err => {
          console.log(err);
        }
      );
  }

}

@Component({
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

export class ModalMonthlyComponent implements OnInit {

  link: any;

  constructor(public sanitizer: DomSanitizer, public modalRef: BsModalRef, private api: ApiService) {
  }

  ngOnInit() {
  }

  photoURL() {
    let result = this.sanitizer.bypassSecurityTrustResourceUrl(this.link);
    return result;
  }
}