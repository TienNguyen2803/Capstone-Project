import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { PrimitiveService } from 'src/app/primitive.service';
import { ApiService } from 'src/app/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/util.service';
import { SPCSObject } from 'src/app/class/SPCSObject';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

declare var $: any;

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.css']
})
export class DocumentComponent implements OnInit {

  formulasAPI: any[] = [];

  documentsAPI: any[] = [];
  documents: any[] = [];

  Hash: any;

  UserID: any;

  modalRef: BsModalRef;

  // page divide
  // pageQuantity = 7;
  // pageLength: any;
  // pageNum = 1;
  // pageNumArray: Number[] = [];

  documentToCreate = new SPCSObject;

  user: any;

  constructor(private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) {

  }

  ngOnInit(): void {

    this.user = JSON.parse(localStorage.getItem('user'));

    this.initDocumentToCreate();

    var vm = this;
    vm.getAllDocuments();
    setInterval(function () {
      vm.getAllDocuments();
    }, 3000);
  }

  getAllDocuments() {
    this.api.getAllDocument()
      .subscribe(
        res => {
          this.documentsAPI = res;
          for (let i = 0; i < this.documentsAPI.length; i++) {
            const el = this.documentsAPI[i];
            el.SignDate = this.util.formatDate(el.SignDate);
            el.ApplyDate = this.util.formatDate(el.ApplyDate);
            if (el.Status == 2) el.State = "đang áp dụng"
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
        },
        err => {
          console.log(err);
        }
      );

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
      "order": [],
      "columnDefs": [
        { orderable: false, targets: '_all' }
      ],
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
    })
  }

  onFileChange(files: any) {
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

  openCreateDocodal(createPair: TemplateRef<any>) {

    this.modalRef = this.modalService.show(createPair);

  }
}