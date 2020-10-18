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

@Component({
  selector: 'app-payroll-detail',
  templateUrl: './payroll-detail.component.html',
  styleUrls: ['./payroll-detail.component.css']
})
export class PayrollDetailComponent implements OnInit {

  vm = this;

  Hash: any;
  UserID: any;
  payrollId: any;

  documentDetail = new SPCSObject;
  documentToUpdate = new SPCSObject;

  modalRef: BsModalRef;

  constructor(private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService, public fb: FormBuilder) {

  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {

      this.payrollId = JSON.parse(params.get('payrollId'));
      console.log(this.payrollId);
      this.getDocumentById(this.payrollId);
    });
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


        console.log(res);
      }, err => {
        console.log(err);
      });
  }

  //#endregion

}
