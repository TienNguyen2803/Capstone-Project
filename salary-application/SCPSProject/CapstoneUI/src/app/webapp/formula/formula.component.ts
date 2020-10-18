import { Component, OnInit, ViewChild } from '@angular/core';
import { PrimitiveService } from 'src/app/primitive.service';
import { ApiService } from 'src/app/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/util.service';

declare var $: any;

@Component({
  selector: 'app-formula',
  templateUrl: './formula.component.html',
  styleUrls: ['./formula.component.css']
})
export class FormulaComponent implements OnInit {

  formulasAPI: any[] = [];
  formulas: any[] = [];

  Hash: any;

  UserID: any;

  txtSearch: any;

  // page divide
  pageQuantity = 7;
  pageLength: any;
  pageNum = 1;
  pageNumArray: Number[] = [];

  // formula details
  formulaDetails = new Array(1);
  index = 0;

  constructor(private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) {

  }
  ngOnInit(): void {

    this.api.getAllFormulas()
      .subscribe(
        res => {
          this.formulas = res;
          for (let i = 0; i < this.formulas.length; i++) {
            const el = this.formulas[i];
            el.CreateDate = this.util.formatDate(el.CreateDate);
          }
          console.log(this.formulas);

          this.jqueryInit();
        },
        err => {
          console.log(err);
        }
      )
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
    })
  }

}