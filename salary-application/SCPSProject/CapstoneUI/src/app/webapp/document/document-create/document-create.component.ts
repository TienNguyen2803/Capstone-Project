import { Component, OnInit, ViewChild, TemplateRef, Inject } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
import { PrimitiveService } from 'src/app/primitive.service';
import { ApiService } from 'src/app/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from 'src/app/util.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs/public_api';
import { Select2OptionData } from 'ng-select2';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { MatStepper } from '@angular/material/stepper';
import { MAT_STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Swal from 'sweetalert2';

declare var $: any;
@Component({
  selector: 'app-document-create',
  templateUrl: './document-create.component.html',
  styleUrls: ['./document-create.component.css'],
  providers: [{
    provide: MAT_STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }
  }]
})
export class DocumentCreateComponent implements OnInit {

  Hash: any;

  UserID: any;

  thisDate = new Date();

  @ViewChild('createDocumentFlow', { static: false }) createDocumentFlow: TabsetComponent;


  public formulaComponents: Array<Select2OptionData>;
  public selectedList = [[]];
  public selectedList2 = [];
  public selectedList3 = [];
  cache = new SPCSObject;

  combinedComponents: any;

  formulaToCreate = new SPCSObject;

  modalRef: BsModalRef;

  documentToCreateNewModel = new FormData;
  documentToCreate = new SPCSObject;
  documentAfterCreate = new SPCSObject;

  isLinearStepper = true;
  checkStepperArr = [false, false, false, false]

  // listField for test
  listTest = []
  listField = [];

  listResultTest = [];

  btnCheckState = true;

  tempOperator = 1;

  ft = new SPCSObject;

  formData = new FormData();

  select2BindModel = [];

  fReview = [{ Name: "", Expression: "" }];

  showFormulasDetail = false;

  listAllFormula = [];

  formulaDisabled = [];

  formulaDisabled2 = [];

  constructor(public dialog: MatDialog, private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) { }

  ngOnInit(): void {
    this.ft.deleteTest = [];
    this.formulaDisabled.push(false);

    this.route.queryParamMap.subscribe(params => {
      // this.Hash = params.get('Hash');
      // if (this.Hash == null || this.Hash == "") {
      //   this.router.navigate([""]);
      // }

      this.initDocumentToCreate();
      this.getAllFormulaElements();

    });
  }

  ngAfterViewInit() {
    $(document).on('keyup', function (e) {
      if ($('.spcs.active').length) {
        let state = $('.spcs.active').data('state');
        if (e.which == 27 /* KEYCODE ESC */) {
          $('.spcs.active').removeClass('active');
        } else if (e.which == 187 /* KEYCODE '+' */) {
          state.additional.operator = 1;
        } else if (e.which == 189 /* KEYCODE '-' */) {
          state.additional.operator = 2;
        } else if (e.which == 56 /* KEYCODE '*' */) {
          state.additional.operator = 3;
        } else if (e.which == 191 /* KEYCODE '/' */) {
          state.additional.operator = 4;
        } else {
          // console.log(e.which);
        }
        $('.spcs.active').attr('data-operator', state.additional.operator);
      }
    })
  }

  ngAfterViewChecked() {
    this.showTooltip();
  }

  activeSelect2(index) {
    this.deactiveAllSelect2();
    console.log(this.formulaDisabled)
    this.formulaDisabled[index] = false;
  }

  deactiveAllSelect2() {
    for (let i = 0; i < this.formulaDisabled.length; i++) {
      this.formulaDisabled[i] = true;
    }
  }

  activeSelect2Detail(index) {
    console.log('test');
    this.deactiveAllSelect2Detail();
    console.log(this.formulaDisabled2)
    this.formulaDisabled2[index] = false;
  }

  deactiveAllSelect2Detail() {
    for (let i = 0; i < this.formulaDisabled2.length; i++) {
      this.formulaDisabled2[i] = true;
    }
  }

  checkToShowFormulaDetail(showFormulasDetail) {
    if (showFormulasDetail == false) {

      console.log("selectedList3");
      console.log(this.selectedList3);
      if (this.selectedList2.length == 0) {
        this.showFormulasDetail = !this.showFormulasDetail;
        this.formulaDisabled[0] = true;
        for (let i = 0; i < this.selectedList3.length; i++) {
          const el = this.selectedList3[i];
          this.formulaDisabled2.push(true);
          this.selectedList.push(this.select2BindModel[el.id]);
        }
      }
      else {
        Swal.fire("Cảnh Báo", "Hãy xác nhận hết các công thức để có thể sử dụng tính năng này", "warning");
      }

    }
    else {
      this.checkToCloseFormulaDetail();
    }
  }

  checkToCloseFormulaDetail() {

    this.createFormula2();
    this.formulaDisabled2 = [];
    this.showFormulasDetail = !this.showFormulasDetail;
    this.formulaDisabled[0] = false;
  }

  convertDate(str) {
    var date = new Date(str),
      mnth = ("0" + (date.getMonth() + 1)).slice(-2),
      day = ("0" + date.getDate()).slice(-2);
    return [date.getFullYear(), mnth, day].join("-");
  }


  validateStep1(stepper: MatStepper) {
    if (this.documentToCreate.Code == "" || this.documentToCreate.Code == null) {
      Swal.fire("Cảnh Báo", "Chưa nhập Số Hiệu cho Quyết Định", "warning");
      return;
    }
    if (this.documentToCreate.file == null) {
      Swal.fire("Cảnh Báo", "Chưa tải lên file Quyết Định", "warning");
      return;
    }
    if (this.documentToCreate.CloseDay == "") {
      Swal.fire("Cảnh Báo", "Chưa nhập Ngày Tính Lương cho Quyết Định", "warning");
      return;
    }
    if (this.documentToCreate.Deadline == "") {
      Swal.fire("Cảnh Báo", "Chưa nhập Hạn Tính Lương cho Quyết Định", "warning");
      return;
    }
    if (this.documentToCreate.CloseDay < 1 || this.documentToCreate.CloseDay > 28) {
      Swal.fire("Cảnh Báo", "Ngày Tính Lương phải từ 1 đến 28", "warning");
      return;
    }
    if (this.documentToCreate.Deadline < 1 || this.documentToCreate.Deadline > 28) {
      Swal.fire("Cảnh Báo", "Ngày Tính Lương phải từ 1 đến 28", "warning");
      return;
    }
    this.goForward(stepper);
  }

  triggerEventStep(event) {
    // console.log(event);

    this.formulaToCreate.Name = ("Thực nhận của " + this.documentToCreate.Code);

    if (event.selectedIndex == 2) {
      this.createSalaryFormula(null);
      this.listTest = [];
      this.listResultTest = [];
    }
    if (event.selectedIndex == 3) {
      // call api for fReview
      this.api.showFormula(this.formulaToCreate).subscribe(
        res => {
          this.fReview = []
          for (let i = res[0].length - 1; i >= 0; i--) {
            const el = res[0][i];
            this.fReview.push(el);
          }

          console.log(this.fReview);
        },
        err => {

        }
      )
    }
  }

  openCreateTestFieldsModal(testFields: TemplateRef<any>) {
    let config = {
      class: "modal-lg"
    }
    this.modalRef = this.modalService.show(testFields, config);
  }

  addListTest() {
    let temp = JSON.parse(JSON.stringify(this.listField));
    this.listTest.push(temp);
  }

  deleteListTest(index) {
    this.listTest.splice(index, 1);
  }

  goBack(stepper: MatStepper) {
    stepper.previous();
  }

  goForward(stepper: MatStepper) {
    stepper.selected.completed = true;
    stepper.next();
  }

  onFileChange(files: any) {
    this.documentToCreate.file = files.item(0);
  }

  initDocumentToCreate() {
    this.documentToCreate.Code = "";
    this.documentToCreate.SignDate = new Date();
    this.documentToCreate.ApplyDate = new Date();
    this.documentToCreate.CloseDay = 5;
    this.documentToCreate.Deadline = 10;
    this.documentToCreate.Description = "";
    this.documentToCreate.file = null;
  }

  getAllFormulaElements() {
    this.api.getAllFormulaElements()
      .subscribe(res => {
        this.combinedComponents = res;
        this.formulaComponents = [];

        for (let i = 0; i < this.combinedComponents.length; i++) {
          const el = this.combinedComponents[i];
          el.operator = 1;

          let check = this.formulaComponents.filter((e) => {
            let temp1 = el.Id + "-" + el.Name + "-" + el.Type;
            let temp2 = e.id;
            if (temp2 == null) temp2 = "~!can't duplicate thing!~";
            // ID
            if (temp1 == temp2) {
              return el;
            }
          })
          if (check.length == 0) {
            this.formulaComponents.push(
              {
                id: el.Id + "-" + el.Name + "-" + el.Type,
                text: el.Name,
                additional: el,
              }
            );
          }
        }

        console.log(this.formulaComponents);
      }, err => {
        console.log(err);
      });
  }

  showTooltip() {

    let length = this.selectedList2.length - 1;
    let temp = document.getElementById("tooltip" + (length));
    let more = 0;
    if (this.selectedList2.length == 1) {
      // first need more offset
      more = 42;
    }
    let offset;
    if (this.selectedList2[length] != null && this.selectedList2[length].htmltag != null) {
      offset = this.selectedList2[length].htmltag.offsetLeft + 75 + more;
    }
    if (temp != null) {
      temp.style.visibility = "visible";
      temp.style.opacity = "1";
      temp.style.top = (45 + 48 * (length)) + "px";
      temp.style.left = offset + "px";
    }
  }

  options(value, type) {
    var vm = this;
    var selectedComponents = [];
    var formula;

    // formula là để lấy công thức đang được mở
    // selectedComponents là các phần tử trong công thức đó nếu có
    if (type == 1 && vm.selectedList2[(value - 1)] != null) {
      selectedComponents = this.select2BindModel[vm.selectedList2[(value - 1)].id] || [];
      formula = vm.selectedList2[(value - 1)];
    }
    else if (type == 2 && vm.selectedList3[(value - 1)] != null) {
      selectedComponents = this.select2BindModel[vm.selectedList3[(value - 1)].id] || [];
      formula = vm.selectedList3[(value - 1)];

    }

    return {
      width: '1200',
      multiple: true,
      tags: true,
      allowduplicate: true,
      createTag: function (e) {
        return undefined;
      },
      onUnselect: function (data, selection) {
        if (vm.selectedList2.length > 0 && data.text == formula.text) {
          vm.selectedList2.splice(-1, 1);
          vm.selectedList.splice(-1, 1);
        }
        for (let i = 0; i < vm.selectedList3.length; i++) {
          var el = vm.selectedList3[i];
          if (el.text == data.text) {
            vm.selectedList3.splice(i, 1);
            vm.selectedList.splice(i + 1, 1);
          }
        }
        // if (vm.selectedList3.length > 0 && data.text == formula.text) {
        //   vm.selectedList3.splice(-1, 1);
        //   vm.selectedList.splice(-1, 1);
        // }
      },
      myChanged: function (sl) {
        // console.log("myChanged");
        vm.selectedList[value] = sl;

      },
      selectedList: [],
      index: value,
      ajax: {
        transport: function (params, success, failure) {
          let data = Object.assign([], vm.formulaComponents || []);
          let data2 = Object.assign([], vm.formulaComponents || []);
          let temp = new SPCSObject();

          if (params.data._type == 'query' && params.data.q) {
            // process operator
            let query = params.data.q;
            let firstLetter = query.substr(0, 1);
            if (vm.util.isOperator(firstLetter)) {
              temp.operator = vm.util.numOperator(firstLetter);
              query = query.substr(1);
            }
            else {
              temp.operator = 1;
              firstLetter = " ";
            }

            // console.log("data2-1");
            // console.log(data2);

            // search
            data = data.filter(a => a.text.indexOf(query) != -1);
            data2 = data2.filter(a => a.additional.Name == query);

            // console.log("data2-2");
            // console.log(data2);

            // chỉnh dấu cho các phần tử
            for (let i = 0; i < data.length; i++) {
              const el = data[i];
              if (vm.util.isOperator(el.text.substr(0, 1))) {
                el.text = el.text.substr(1);
              }
              el.text = firstLetter + el.text;
              // console.log(el);
            }

            // create new const
            if (!isNaN(query)) {
              let obj = {
                id: query
                , text: `Tạo mới hằng số "${query}"`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data = [obj];
            }
            else if (vm.util.equalsIgnoreCase(params.data.q, "Min(")) {
              let tempMin = {
                id: query
                , text: `Tạo mới Min(...)`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data.push(tempMin);
            }
            else if (vm.util.equalsIgnoreCase(params.data.q, "Max(")) {
              let tempMax = {
                id: query
                , text: `Tạo mới Max(...)`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data.push(tempMax);
            }
            else if (vm.util.equalsIgnoreCase(params.data.q, "Avg(")) {
              let tempAvg = {
                id: query
                , text: `Tạo mới Avg(...)`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data.push(tempAvg);
            }
            else if (vm.util.equalsIgnoreCase(params.data.q, "Rng(")) {
              let tempRng = {
                id: query
                , text: `Tạo mới Rng(...)`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data.push(tempRng);
            }
            else if (vm.util.equalsIgnoreCase(params.data.q, "(")) {
              let tempRng = {
                id: query
                , text: `Tạo mới (...)`
                , additional: JSON.parse(JSON.stringify(temp)) // just copy value
              }
              data.push(tempRng);
            } else {
              // flow create new component
              let cmp = [
                `đầu lương định kỳ `,
                `đầu lương `,// field
                `bảng tra `,// refTable
                `công thức `,// expression
              ]
              // compare search to name of all system
              if (data2.length == 0) {
                // check if query is not a number -> new component else -> const
                if (isNaN(query)) {
                  // create new component
                  // field or refTable or formula
                  for (let i = 0; i < cmp.length; i++) {
                    let obj = {
                      id: query
                      , text: `Tạo mới ${cmp[i]} "${query}"`
                      , additional: JSON.parse(JSON.stringify(temp)) // just copy value
                    }
                    data.push(obj);
                  }
                }
                else {
                  // create new const
                  let obj = {
                    id: query
                    , text: `Tạo mới hằng số "${query}"`
                    , additional: JSON.parse(JSON.stringify(temp)) // just copy value
                  }
                  data.push(obj);
                }
              }
            }
          }
          success({
            results: data.map((d, i) => ({ index: i, id: d.id, text: d.text, additional: d.additional }))
          })
        }
      },
      templateResult: function (state) {
        if (!state.id) {
          return state.text;
        }

        let temp = Object.assign({}, state);
        if (vm.util.isOperator(temp.text.substr(0, 1))) {
          temp.text = state.text.substr(1);
        }

        if (state.additional.Type == null) {
          // show dropdown
          return $(
            '<span>' + ' <i class="fa fa-plus-circle"></i> ' + temp.text + '</span>'
          );

        } else {

          let url = "assets/images/";
          switch (state.additional.Type) {
            case 1:
              url += "field.png";
              break;
            case 2:
              url += "refTable.png";
              break;
            case 3:
              url += "formula.png";
              break;

            default:
              break;
          }
          return $(
            '<span><img width="14" height="14" src="' + url + '" class="img-flag" /> ' + temp.text + '</span>'
          ).on('click', function (e) {
            e.stopPropagation();

          });
        }
      },
      templateSelection: function (state) {
        let result;

        if (state.additional.isMapped == null) {
          state.additional.isMapped = false;
        }

        // track operator
        if (vm.util.isOperator(state.text.substr(0, 1))) {
          state.additional.operator = vm.util.numOperator(state.text.substr(0, 1));
          state.text = state.text.substr(1);
        }

        if (state.text.includes("Tạo mới hằng số ")) {
          state.text = state.id;
        }

        // xét trường hợp component là cũ hay đang được tạo mới để lấy text
        var tempText;
        if (state.additional.Id == null) {
          tempText = state.id;
        }
        else {
          tempText = state.text;
        }

        // xét aggreegation để trả class css định nghĩa cho hợp lý ( dấu ',' và operator )
        if (formula != null && formula.additional.FormulaTypeVM.FormulaType != 1 && formula.additional.FormulaTypeVM.FormulaType != 6) {
          // css chỉ có dấu phẩy
          result = $('<div>', {
            'class': 'spcs-aggregation'
            , 'style': 'width: auto;'
            , 'text': tempText
            , 'data-operator': '0'
          }).data('state', state)
        }
        else {
          // css có 4 operator
          result = $('<div>', {
            'class': 'spcs'
            , 'style': 'width: auto;'
            , 'text': tempText
            , 'data-operator': state.additional.operator
          }).data('state', state).on('click', function (e) {
            e.stopPropagation();
            // process CSS
            $('.spcs.active').removeClass('active');
            $(this).addClass('active');
          }).on('dblclick', function (e) {
            e.stopPropagation();
            // process CSS
            $('.spcs.active').removeClass('active');
          });
        }

        if (state.additional.Id == null) { // tạo mới

          if (vm.select2BindModel[state.id] != null) {
            result.on('dblclick', function (e) {
              e.stopPropagation();
              var tempModel = vm.select2BindModel[state.id] || [];

              // problem
              if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(tempModel) == -1) {
                // nếu chưa tồn tại thì khởi tạo
                if (state.additional.FormulaTypeVM == null) {
                  state.additional.FormulaTypeVM = new SPCSObject;
                  // formula expression
                  state.additional.FormulaTypeVM.FormulaType = 1;
                }

                if (type == 1) {
                  // first way select 2
                  vm.selectedList2.push(state);

                  // validate which select2 is enable
                  vm.formulaDisabled.push(false);
                  vm.activeSelect2(vm.selectedList2.length);
                }
                else if (type == 2) {
                  // second way select 2
                  vm.selectedList3.push(state);
                }

                vm.selectedList.push(tempModel);

              }
            })
          }
          if (state.text.includes("Tạo mới đầu lương định kỳ ")
            || (state.additional.Type == 1 && state.additional.FieldTypeVM.DataType == 'payroll')) {
            // state.text = state.id;

            // trạng thái khi tạo mới field
            if (state.text.includes("Tạo mới đầu lương định kỳ ")) {
              state.additional.Type = -1;
              state.additional.Name = state.id;

              if (vm.formulaComponents.filter(a => a.id == state.id).length == 0) {
                vm.formulaComponents.push({
                  id: state.id,
                  text: state.id,
                  additional: state.additional,
                });
              }
            }

            // trang thái khi xài lại field tạo mới
            result.on('dblclick', function (e) {
              e.stopPropagation();
              vm.openFieldModal('payroll', state);
            })
          }
          if ((!state.text.includes("Tạo mới đầu lương định kỳ ") && state.text.includes("Tạo mới đầu lương "))
            || (state.additional.Type == 1 && state.additional.FieldTypeVM.DataType == 'string')) {
            // state.text = state.id;

            // trạng thái khi tạo mới field
            if (state.text.includes("Tạo mới đầu lương ")) {
              state.additional.Type = -1;
              state.additional.Name = state.id;

              if (vm.formulaComponents.filter(a => a.id == state.id).length == 0) {
                vm.formulaComponents.push({
                  id: state.id,
                  text: state.id,
                  additional: state.additional,
                });
              }
            }

            // trang thái khi xài lại field tạo mới
            result.on('dblclick', function (e) {
              e.stopPropagation();
              vm.openFieldModal('number', state);
            })
          }
          if (state.text.includes("Tạo mới bảng tra ") || (state.additional.Type == 2)) {
            // state.text = state.id;

            // trạng thái khi tạo mới refTable tạo mới
            if (state.text.includes("Tạo mới bảng tra ")) {
              state.additional.Type = -1;
              state.additional.Name = state.id;

              if (vm.formulaComponents.filter(a => a.id == state.id).length == 0) {
                vm.formulaComponents.push({
                  id: state.id,
                  text: state.id,
                  additional: state.additional,
                });
              }
            }

            // trang thái khi xài lại refTable tạo mới
            result.on('dblclick', function (e) {
              e.stopPropagation();
              vm.openReftableModal(state);
            })
          }
          if (state.text.includes("Tạo mới công thức ")) {
            result.on('dblclick', function (e) {
              e.stopPropagation();
              var tempModel = vm.select2BindModel[state.id] || [];

              if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(tempModel) == -1) {
                // nếu chưa tồn tại thì khởi tạo
                if (state.additional.FormulaTypeVM == null) {
                  state.additional.FormulaTypeVM = new SPCSObject;
                  // formula expression
                  state.additional.FormulaTypeVM.FormulaType = 1;
                }

                if (type == 1) {
                  // first way select 2
                  vm.selectedList2.push(state);
                  // validate which select2 is enable
                  vm.formulaDisabled.push(false);
                  vm.activeSelect2(vm.selectedList2.length);
                }
                else if (type == 2) {
                  // second way select 2
                  vm.selectedList3.push(state);
                }
                vm.selectedList.push(tempModel);


              }
            })
          }
          switch (state.text) {
            case `Tạo mới (...)`:
              // agreegation range
              result.on('dblclick', function (e) {
                e.stopPropagation();
                var tempModel = vm.select2BindModel[state.id] || [];

                if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(state) == -1) {
                  // nếu chưa tồn tại thì khởi tạo
                  if (state.additional.FormulaTypeVM == null) {
                    state.additional.FormulaTypeVM = new SPCSObject;
                    // formula expression
                    state.additional.FormulaTypeVM.FormulaType = 6;
                  }

                  if (type == 1) {
                    // first way select 2
                    vm.selectedList2.push(state);
                    // validate which select2 is enable
                    vm.formulaDisabled.push(false);
                    vm.activeSelect2(vm.selectedList2.length);
                  }
                  else if (type == 2) {
                    // second way select 2
                    vm.selectedList3.push(state);
                  }

                  vm.selectedList.push(tempModel);


                }
              })
            case `Tạo mới Min(...)`:
              // // aggregation min
              result.on('dblclick', function (e) {
                e.stopPropagation();
                var tempModel = vm.select2BindModel[state.id] || [];

                if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(state) == -1) {
                  // nếu chưa tồn tại thì khởi tạo
                  if (state.additional.FormulaTypeVM == null) {
                    state.additional.FormulaTypeVM = new SPCSObject;
                    // formula expression
                    state.additional.FormulaTypeVM.FormulaType = 2;
                  }

                  if (type == 1) {
                    // first way select 2
                    vm.selectedList2.push(state);
                    // validate which select2 is enable
                    vm.formulaDisabled.push(false);
                    vm.activeSelect2(vm.selectedList2.length);
                  }
                  else if (type == 2) {
                    // second way select 2
                    vm.selectedList3.push(state);
                  }
                  vm.selectedList.push(tempModel);


                }
              })
            case `Tạo mới Max(...)`:
              // // aggregation max
              result.on('dblclick', function (e) {
                e.stopPropagation();
                var tempModel = vm.select2BindModel[state.id] || [];

                if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(tempModel) == -1) {
                  // nếu chưa tồn tại thì khởi tạo
                  if (state.additional.FormulaTypeVM == null) {
                    state.additional.FormulaTypeVM = new SPCSObject;
                    // formula expression
                    state.additional.FormulaTypeVM.FormulaType = 3;
                  }

                  if (type == 1) {
                    // first way select 2
                    vm.selectedList2.push(state);
                    // validate which select2 is enable
                    vm.formulaDisabled.push(false);
                    vm.activeSelect2(vm.selectedList2.length);
                  }
                  else if (type == 2) {
                    // second way select 2
                    vm.selectedList3.push(state);
                  }

                  vm.selectedList.push(tempModel);

                }
              })
            case `Tạo mới Avg(...)`:
              // aggregation average
              result.on('dblclick', function (e) {
                e.stopPropagation();
                var tempModel = vm.select2BindModel[state.id] || [];

                if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(tempModel) == -1) {
                  // nếu chưa tồn tại thì khởi tạo
                  if (state.additional.FormulaTypeVM == null) {
                    state.additional.FormulaTypeVM = new SPCSObject;
                    // formula expression
                    state.additional.FormulaTypeVM.FormulaType = 4;
                  }

                  if (type == 1) {
                    // first way select 2
                    vm.selectedList2.push(state);
                    // validate which select2 is enable
                    vm.formulaDisabled.push(false);
                    vm.activeSelect2(vm.selectedList2.length);
                  }
                  else if (type == 2) {
                    // second way select 2
                    vm.selectedList3.push(state);
                  }

                  vm.selectedList.push(tempModel);

                }
              })
            case `Tạo mới Rng(...)`:
              // agreegation range
              result.on('dblclick', function (e) {
                e.stopPropagation();
                var tempModel = vm.select2BindModel[state.id] || [];
                if (vm.selectedList2.indexOf(state) == -1 && vm.selectedList.indexOf(tempModel)) {
                  // nếu chưa tồn tại thì khởi tạo
                  if (state.additional.FormulaTypeVM == null) {
                    state.additional.FormulaTypeVM = new SPCSObject;
                    // formula expression
                    state.additional.FormulaTypeVM.FormulaType = 5;
                  }

                  if (type == 1) {
                    // first way select 2
                    vm.selectedList2.push(state);
                    // validate which select2 is enable
                    vm.formulaDisabled.push(false);
                    vm.activeSelect2(vm.selectedList2.length);
                  }
                  else if (type == 2) {
                    // second way select 2
                    vm.selectedList3.push(state);
                  }
                  vm.selectedList.push(tempModel);

                }
              })
            default:
              break;
          }

          state.htmltag = result[0];
          // console.log(result);
          return result;
        } else {
          let color = "color: ";
          switch (state.additional.Type) { // đã có
            case 1:
              color += "blue;";
              break;
            case 2:
              color += "green;";
              break;
            case 3:
              color += "red;";
              break;
            default:
              break;
          }
          state.htmltag = result[0];
          return result;
        }
      },
      selections: {
        data: function (data) {
          // console.log("current");
          // console.log(selected);
          return selectedComponents;
        }
        , select: function (data) {
          // if (selected.indexOf(data) == -1) {
          //   selected.push(data);
          // }
        }
        , unselect: function (data) {
          // selected = selected.filter(a => a != data);
        }
      }

    };
  }

  openFieldModal(dt, fEL) {
    const initialState = {
      vm: this,
      dataType: dt,
      formulaEL: fEL,
    };

    this.modalRef = this.modalService.show(
      ModalFieldComponent,
      {
        initialState: initialState,
        // ignoreBackdropClick: true
      }
    );

  }

  openReftableModal(fEL) {
    const initialState = {
      vm: this,
      formulaEL: fEL
    };

    this.modalRef = this.modalService.show(
      ModalRefTableComponent, { initialState: initialState, class: "modal-lg modal-khoinkt" }
    );

  }

  createFormula2() {
    console.log("create formula 2");

    console.log(this.selectedList);
    // validate select2
    for (let i = this.selectedList.length - 1; i > 0; i--) {
      var el = this.selectedList[i];
      if (el.length == 0) {
        Swal.fire("Cảnh Báo", "Không được để trống công thức", "warning");
        return;
      }
    }

    for (let i = this.selectedList.length - 1; i > 0; i--) {
      var el = this.selectedList[i];
      var el3 = this.selectedList3[i - 1];
      // lưu model trong additional
      var k = el3;
      k.additional.Type = 3;
      var temp = new SPCSObject;
      var name = "";
      // khởi tạo
      temp.FormulaType = k.additional.FormulaTypeVM.FormulaType; // khởi tạo type
      temp.Name = el3.id;
      temp.IsSalaryFormula = false;
      temp.Description = "formula";
      temp.DocId = -1;
      if (k.additional.FormulaTypeVM.FormulaDetailNCVMs != null) {
        k.additional.FormulaTypeVM.FormulaDetailNCVMs = [];
        temp.FormulaDetailNCVMs = k.additional.FormulaTypeVM.FormulaDetailNCVMs
      }
      else {
        temp.FormulaDetailNCVMs = [];
      }

      // đọc các phần tử từ select2
      for (let i = 0; i < el.length; i++) {
        const tempEL = el[i];
        let tempName = tempEL.id as String;
        if (i > 0) {
          // xóa id đặc biệt trước khi gắn vào
          if ((tempName).includes("-")) {
            name += ", " + tempName.split("-")[1];
          }
          else {
            name += ", " + tempName;
          }
        }
        else {
          if ((tempName).includes("-")) {
            name += tempName.split("-")[1];
          }
          else {
            name += tempName;
          }
        }

        let component = new SPCSObject;
        if (tempEL.additional.Type == null) {
          // constant sẽ ko khởi tạo type
          component.Type = 4;
          component.Operator = tempEL.additional.operator;
          component.ConstantTypeVM = {
            Value: Number(tempEL.text)
          }
        }
        else {
          component.Type = tempEL.additional.Type;
          component.Operator = tempEL.additional.operator;
          if (tempEL.additional.Id != null) {
            component.Id = tempEL.id;
            component.FDTypeVM = {
              Id: tempEL.additional.Id,
              Value: Number(tempEL.additional.Name)
            }
            if (isNaN(component.FDTypeVM.Value)) component.FDTypeVM.Value = 0;
          }
          else {
            switch (component.Type) {
              case 1:
                component.FieldTypeVM = tempEL.additional.FieldTypeVM;

                break;
              case 2:
                component.RefTableTypeVM = tempEL.additional.RefTableTypeVM;

                break;
              case 3:
                component.FormulaTypeVM = tempEL.additional.FormulaTypeVM;

                break;

              default:
                break;
            }
          }
        }
        component.Ordinal = i + 1; // chị Hằng ko xài
        temp.FormulaDetailNCVMs.push(component);
      }

      // sửa thông tin trên selectedList -> formulaEL của mảng cũ -> select2 chọn sẽ có
      console.log("formula")

      switch (temp.FormulaType) {
        case 6:
          temp.Name = "(" + name + ")";
          temp.FormulaType = 1;
          break;
        case 2:
          temp.Name = "Min(" + name + ")";
          break;
        case 3:
          temp.Name = "Max(" + name + ")";
          break;
        case 4:
          temp.Name = "Avg(" + name + ")";
          break;
        case 5:
          temp.Name = "Rng(" + name + ")";
          break;
        default:
          break;
      }
      k.additional.FormulaTypeVM = temp;
      el3.id = temp.Name;
      // this.selectedList2[this.selectedList2.length - 1].text = temp.Name;
      // el3.htmltag.innerHTML = temp.Name;

      if (this.formulaComponents.filter(a => a.id == temp.Name).length == 0) {
        this.formulaComponents.push(
          {
            id: temp.Name,
            text: temp.Name,
            additional: k.additional,
          }
        );

      }

      this.select2BindModel[temp.Name] = el;

      console.log("ref formula 2");
      console.log(el3);
      console.log(el);
      // console.log(this.select2BindModel);

      this.selectedList.splice(-1, 1);

    }
  }

  createFormula() {
    console.log("create formula");

    console.log("ref formula");
    console.log(this.selectedList2[this.selectedList2.length - 1]);
    console.log(this.selectedList);
    console.log(this.select2BindModel);

    // validate select2
    if (this.selectedList[this.selectedList.length - 1].length > 0) {
      // lưu model trong additional
      var k = this.selectedList2[this.selectedList2.length - 1];
      k.additional.Type = 3;
      var temp = new SPCSObject;
      var name = "";
      // khởi tạo
      temp.FormulaType = k.additional.FormulaTypeVM.FormulaType; // khởi tạo type
      temp.Name = this.selectedList2[this.selectedList2.length - 1].id;
      temp.IsSalaryFormula = false;
      temp.Description = "formula";
      temp.DocId = -1;
      temp.FormulaDetailNCVMs = [];

      // đọc các phần tử từ select2
      for (let i = 0; i < this.selectedList[this.selectedList.length - 1].length; i++) {
        const el = this.selectedList[this.selectedList.length - 1][i];
        let tempName = el.id as String;
        let seperator;
        if (temp.FormulaType == 6) {
          seperator = " " + this.util.charOperator(el.additional.operator) + " ";
        }
        else {
          seperator = ", ";
        }
        if (i > 0) {
          // xóa id đặc biệt trước khi gắn vào
          if ((tempName).includes("-")) {
            name += seperator + tempName.split("-")[1];
          }
          else {
            name += seperator + tempName;
          }
        }
        else {
          if ((tempName).includes("-")) {
            name += tempName.split("-")[1];
          }
          else {
            name += tempName;
          }
        }

        let component = new SPCSObject;
        if (el.additional.Type == null) {
          // constant sẽ ko khởi tạo type
          component.Type = 4;
          component.Operator = el.additional.operator;
          component.ConstantTypeVM = {
            Value: Number(el.text)
          }
        }
        else {
          component.Type = el.additional.Type;
          component.Operator = el.additional.operator;
          if (el.additional.Id != null) {
            component.Id = el.id;
            component.FDTypeVM = {
              Id: el.additional.Id,
              Value: Number(el.additional.Name)
            }
            if (isNaN(component.FDTypeVM.Value)) component.FDTypeVM.Value = 0;
          }
          else {
            switch (component.Type) {
              case 1:
                component.FieldTypeVM = el.additional.FieldTypeVM;

                break;
              case 2:
                component.RefTableTypeVM = el.additional.RefTableTypeVM;

                break;
              case 3:
                component.FormulaTypeVM = el.additional.FormulaTypeVM;

                break;

              default:
                break;
            }
          }
        }
        component.Ordinal = i + 1; // chị Hằng ko xài
        temp.FormulaDetailNCVMs.push(component);
      }

      // sửa thông tin trên selectedList -> formulaEL của mảng cũ -> select2 chọn sẽ có
      console.log("formula")

      switch (temp.FormulaType) {
        case 6:
          temp.Name = "(" + name + ")";
          temp.FormulaType = 1;
          break;
        case 2:
          temp.Name = "Min(" + name + ")";
          break;
        case 3:
          temp.Name = "Max(" + name + ")";
          break;
        case 4:
          temp.Name = "Avg(" + name + ")";
          break;
        case 5:
          temp.Name = "Rng(" + name + ")";
          break;
        default:
          break;
      }
      k.additional.FormulaTypeVM = temp;
      this.selectedList2[this.selectedList2.length - 1].id = temp.Name;
      // this.selectedList2[this.selectedList2.length - 1].text = temp.Name;
      this.selectedList2[this.selectedList2.length - 1].htmltag.innerHTML = temp.Name;

      // var tempComponent = this.selectedList2.pop();
      if (this.formulaComponents.filter(a => a.id == temp.Name).length == 0) {
        this.formulaComponents.push(
          {
            id: temp.Name,
            text: temp.Name,
            additional: k.additional,
          }
        );

        this.selectedList3.push(this.selectedList2[this.selectedList2.length - 1]);
      }

      this.select2BindModel[temp.Name] = this.selectedList[this.selectedList.length - 1];

      this.selectedList2.splice(-1, 1);
      this.selectedList.splice(-1, 1);

      // active select2 parent
      this.formulaDisabled.slice(-1, 1);
      this.activeSelect2(this.selectedList2.length);
    }
    else {
      Swal.fire("Cảnh Báo", "Không được bỏ trống công thức", "warning");
    }
  }

  createSalaryFormula(stepper: MatStepper) {
    console.log("create salary formula")
    console.log(this.selectedList);

    if (this.showFormulasDetail == true) {
      this.checkToShowFormulaDetail(this.showFormulasDetail);
    }

    // validate select2
    if (this.selectedList.length > 1) {
      Swal.fire("Cảnh Báo", "Hãy hoàn thành tất cả công thức", "warning");
    }
    else if (this.selectedList[this.selectedList.length - 1].length > 0) {

      // truyen data
      this.formulaToCreate.FormulaType = 1; // init type
      this.formulaToCreate.Name = ("Thực nhận của " + this.documentToCreate.Code);
      this.formulaToCreate.IsSalaryFormula = true;
      this.formulaToCreate.Description = "formula";
      this.formulaToCreate.DocId = this.documentToCreate.Id || -1;
      this.formulaToCreate.FormulaDetailNCVMs = [];

      this.documentToCreate.FormulaReview = { Name: "", Expression: "" };
      this.documentToCreate.FormulaReview.Name = this.formulaToCreate.Name;
      this.documentToCreate.FormulaReview.Expression += this.formulaToCreate.Name;
      for (let i = 0; i < this.selectedList[this.selectedList.length - 1].length; i++) {
        const el = this.selectedList[this.selectedList.length - 1][i];

        // ghép formula để review
        if (i > 0) {
          this.documentToCreate.FormulaReview.Expression += " " + this.util.charOperator(el.additional.operator) + " " + el.id;
        }
        else {
          this.documentToCreate.FormulaReview.Expression += " = " + el.id;
        }


        let component = new SPCSObject;
        if (el.additional.Type == -1) {

        }
        if (el.additional.Type == null) {
          // constant sẽ ko khởi tạo type
          component.Type = 4;
          component.Operator = el.additional.operator;
          component.ConstantTypeVM = {
            Value: Number(el.text)
          }
        }
        else {
          component.Type = el.additional.Type;
          component.Operator = el.additional.operator;
          if (el.additional.Id != null) {
            component.FDTypeVM = {
              Id: el.additional.Id,
              Value: Number(el.additional.Name)
            }
            if (isNaN(component.FDTypeVM.Value)) component.FDTypeVM.Value = 0;
          }
          else {
            switch (component.Type) {
              case 1:
                component.FieldTypeVM = el.additional.FieldTypeVM;

                break;
              case 2:
                component.RefTableTypeVM = el.additional.RefTableTypeVM;

                break;
              case 3:

                component.FormulaTypeVM = el.additional.FormulaTypeVM;

                break;

              default:
                break;
            }
          }
        }
        component.Name = el.additional.Name;
        component.Ordinal = i + 1; // chị Hằng ko xài
        this.formulaToCreate.FormulaDetailNCVMs.push(component);
      }

      console.log("formula to create")
      console.log(this.formulaToCreate);
      this.cache = this.formulaToCreate;

      this.api.getFieldsCheckFormula(this.formulaToCreate).subscribe(
        res => {
          this.listField = res;
          this.formulaToCreate.FieldsandValues = this.listField;
          this.listResultTest = [];
          if (stepper != null) {
            this.goForward(stepper);
          }

          this.formulaToCreate.FieldsandValuesTest = [];
          this.formulaToCreate.FieldsandValuesTest.push(this.listField);

          // call api for fReview
          this.api.showFormula(this.formulaToCreate).subscribe(
            res => {
              this.fReview = []
              for (let i = res[0].length - 1; i >= 0; i--) {
                const el = res[0][i];
                this.fReview.push(el);
              }

              console.log(this.fReview);
            },
            err => {

            }
          )
        },
        err => {
          Swal.fire("Cảnh Báo", "Bạn chưa định nghĩa các thành phần sau:" + "<b>" + err.error + "</b>", "warning");
        }
      );
    }
    else {
      Swal.fire("Cảnh Báo", "Không được bỏ trống công thức", "warning");
    }

  }

  checkFormula() {
    // kiểm thử công thức cùng số liệu
    this.formulaToCreate.FieldsandValuesTest = this.listTest;
    console.log(this.formulaToCreate);
    this.api.checkFormula(this.formulaToCreate).subscribe(
      res => {
        this.listResultTest = [];
        for (let k = 0; k < res.length; k++) {
          const el = res[k];
          let tempList = [];
          for (let i = 0; i < el.length; i++) {
            const element = el[i];
            // console.log(element)
            if (element.Type == 2 || element.Type == 3) {
              // số thập phân 2 chữ số cuối
              tempList.push(element);
            }
          }
          this.listResultTest.push(tempList);
        }
        console.log(this.listResultTest);
        this.btnCheckState = false;
      },
      err => {
        console.log(err);
      }
    );
  }

  complete() {
    console.log(this.documentToCreate);
    console.log(this.formulaToCreate);
    this.documentToCreate.Formula = this.formulaToCreate;
    this.documentToCreateNewModel = new FormData();
    this.documentToCreateNewModel.append("viewModel", JSON.stringify(this.documentToCreate));
    this.documentToCreateNewModel.append("file", this.documentToCreate.file);
    // console.log(this.documentToCreateNewModel.get("file"));
    this.api.createFormula(this.documentToCreateNewModel)
      .subscribe(
        res => {
          this.documentAfterCreate = res;
          Swal.fire("Thành Công", "Tạo quyết định thành công", "success").then((value) => {
            location.href = 'document/detail?documentId=' + JSON.stringify(this.documentAfterCreate.Id);
          });

        },
        err => {
          console.log(err);
          if (err.status == 409) {
            this.documentAfterCreate = err.error.documentAfterCreate;
            const initialState = {
              vm: this,
              model: err.error,
            };

            this.modalRef = this.modalService.show(
              ModalActiveDocComponent,
              {
                initialState: initialState,
                ignoreBackdropClick: true,
                class: "modal-sm"
              }
            );
          }
          else {
            Swal.fire("Cảnh Báo", err.error, "warning");
          }
        }
      );
  }

}


@Component({
  selector: 'modal-active-doc',
  template: `
  <div class="modal-header">
    <h5 class="modal-title">Thay Thế Quyết Định Hiện Hành ?</h5>
  </div>
  <div class="modal-footer">
    <button type="button" class="btn btn-danger" (click)="activeDoc(false)">
        <i class="fa fa-times"></i>
        Hủy</button>
    <button type="button" class="btn btn-success" (click)="activeDoc(true)">
        <i class="fa fa-check-square-o"></i>
        Xác nhận</button>
  </div>
    `
})

export class ModalActiveDocComponent implements OnInit {

  vm: DocumentCreateComponent;

  model: any;

  constructor(public fieldModalRef: BsModalRef, private api: ApiService,) { }

  ngOnInit() {

  }

  // toDocDetail() {
  //   location.href = 'document/detail?document=' + JSON.stringify(this.vm.documentAfterCreate);
  // }

  activeDoc(isChange) {
    this.api.activeDoc(this.model, isChange).subscribe(
      res => {
        alert("Đổi Quyết Định Hiện Hành Thành Công");
        location.href = 'document/detail?documentId=' + JSON.stringify(this.vm.documentAfterCreate.Id);
      },
      err => {
        location.href = 'document/detail?documentId=' + JSON.stringify(this.vm.documentAfterCreate.Id);
      }
    )
  }
}

@Component({
  selector: 'modal-field',
  template: `
  <div class="modal-header">
    <h5 class="modal-title">Tạo Đầu Lương Mới</h5>
    <button type="button" class="close" (click)="fieldModalRef.hide()" aria-label="Close">
        <span aria-hidden="true">&times;</span>
    </button>
  </div>
  <div class="modal-body">

    <div class="form-group">
        <label for="input-1">Tên viết tắt</label>
        <input class="form-control" type="text" id="input-1" [(ngModel)]="fieldToCreate.Name" required>
    </div>
    <div class="form-group">
        <label for="input-2">Tên đầy đủ</label>
        <input class="form-control" type="text" id="input-2" [(ngModel)]="fieldToCreate.LongName" required>
    </div>
    <div class="form-group">
        <label for="input-3">Kiểu dữ liệu</label>
        <select class="form-control" id="input-3" [(ngModel)]="fieldToCreate.DataType">
            <option value="number">Số</option>
            <option value="string">Chuỗi</option>
        </select>
    </div>
    <div class="form-group">
        <label for="input-5">mô tả</label>
        <input class="form-control" type="text" [(ngModel)]="fieldToCreate.Description" required>
    </div>
    <ng-container *ngIf="fieldToCreate.DataType != 'payroll'">
    <div class="form-group">
      <div class="icheck-material-white">
        <input type="checkbox" id="input-4" [(ngModel)]="fieldToCreate.IsMonthlySalaryComponent" checked>
        <label for="input-4">Nạp thông tin mỗi tháng</label>
      </div>
    </div>
    </ng-container>
  </div>
  <div class="modal-footer">
    <!-- <button type="button" class="btn btn-danger" (click)="fieldModalRef.hide()">
        <i class="fa fa-times"></i>
        Hủy</button> -->
    <button type="button" class="btn btn-success" (click)="createField()">
        <i class="fa fa-check-square-o"></i>
        Xác nhận</button>
  </div>
    `
})

export class ModalFieldComponent implements OnInit {

  vm: DocumentCreateComponent;

  fieldToCreate = new SPCSObject;

  formulaEL: any;

  dataType: any;

  constructor(public fieldModalRef: BsModalRef, private api: ApiService,) { }

  ngOnInit() {
    console.log(this.formulaEL);
    if (this.formulaEL.additional.FieldTypeVM != null) {
      this.fieldToCreate = this.formulaEL.additional.FieldTypeVM;
    }
    else {
      this.fieldToCreate = {
        Name: "",
        LongName: "",
        DataType: this.dataType,
        Description: "",
        IsMonthlySalaryComponent: true,
      };
    }
    this.fieldToCreate.Name = this.formulaEL.id;
    console.log(this.fieldToCreate);
  }

  createField() {
    console.log(this.fieldToCreate);


    var tempComp = this.vm.formulaComponents.filter(a => a.id == this.fieldToCreate.Name)

    this.formulaEL.id = this.fieldToCreate.Name;
    // dòng này để bỏ các prefix trong select2 khiến nó thành component đã đc lưu dù chưa xuống databse
    this.formulaEL.text = this.fieldToCreate.Name;

    this.formulaEL.htmltag.innerHTML = this.fieldToCreate.Name;
    this.formulaEL.additional.Type = 1;
    this.formulaEL.additional.FieldTypeVM = this.fieldToCreate;
    this.fieldModalRef.hide();

    // có phần tử nào có tên ... trong formulacomponents thì mới thay đổi
    if (tempComp.length > 0) {

      tempComp[0] = {
        id: this.fieldToCreate.Name,
        text: this.fieldToCreate.Name,
        additional: this.formulaEL.additional,
      };
    }
    else {
      Swal.fire("Lỗi", "Có lỗi xảy ra ở tạo field", "error");
    }
  }
}

@Component({
  selector: 'modal-refTable',
  template: `

          <div class="modal-header" style="background-color: rgba(255,255,255,.2);">
              <h5 class="modal-title" style="font-size: 18px;">TẠO BẢNG TRA</h5>
              <!-- <button type="button" class="close" (click)="refTableModalRef.hide()" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
              </button> -->
          </div>
          <div class="modal-body" style="float: left; width: 100%; background-color: rgba(255,255,255,.2);">
              <div style="float:left; width: 30%;">
                  <div class="form-group">
                      <label for="input-1">Tên bảng tra</label>
                      <input class="form-control" type="text" id="input-1" [(ngModel)]="refTableToCreate.Name"
                          required>
                  </div>
                  <div class="form-group">
                      <label>Mô tả</label>
                      <input [(ngModel)]="refTableToCreate.Description" class="form-control" type="text"
                          placeholder="Mô tả">
                  </div>
                  <div class="form-group">
                      <label>Đối tượng so sánh</label>
                      <select #scc (change)="setRefTableSource(scc.value)" class="form-control"
                          style="width: 290px;">
                          <ng-container *ngFor="let cc of formulaComponents; index as indexId">
                              <option value="{{cc.id}}">{{cc.text}}</option>
                          </ng-container>
                      </select>
                  </div>
                  <div class="form-group">
                      <label>Kiểu trả về</label>
                      <select #rt (change)="setRefTableReturnType(rt.value)" class="form-control"
                          style="width: 290px;" [disabled]="refTableToCreate.ReferenceTableDetailCreateVMs.length > 0">
                          <option value="4">Dữ liệu</option>
                          <option value="3">Công thức</option>
                          <!-- <option value="1">Đầu lương</option> -->
                      </select>
                  </div>
                  <div class="form-group">
                      <label>Kiểu so sánh</label>
                      <select #cpt id="cpt" (change)="setRefTableCompareType(cpt.value)" class="form-control"
                          style="width: 290px;" [disabled]="stateDisableCompareType">
                          <option value="1">So sánh bằng</option>
                          <option value="2">So sánh khoảng</option>
                      </select>
                  </div>
              </div>
              <div style="float:left; width: 66%; margin-left:4%;">
                  <p>BẢNG THAM CHIẾU&nbsp;
                      <i (click)="openCreatePairModal(createPair)" class="fa fa-plus-circle"
                          style="color: white; font-size: 1.5em; cursor: pointer"></i>
                  </p>
                  <table class="table table-bordered">
                      <colgroup>
                          <col style="width: 44%;">
                          <col style="width: 44%;">
                          <col style="width: 12%;">
                      </colgroup>
                      <tr>
                          <th>GIÁ TRỊ</th>
                          <th>TRẢ VỀ</th>
                          <!-- <th>KIỂU</th> -->
                          <th></th>
                      </tr>
                      <ng-container
                          *ngFor="let ref of refTableToCreate.ReferenceTableDetailCreateVMs; index as indexId">
                          <ng-container>
                              <tr>
                                  <td>
                                      {{ref.Key}}
                                  </td>
                                  <td>
                                      {{ref.Value}}
                                  </td>
                                  <!-- <td>
                                      {{detailReturnType(ref.ReturnType)}}
                                  </td> -->
                                  <td>
                                      <i class="fa fa-trash fa-2x" style="color: red;"
                                          (click)="deleteRefTableDetail(indexId)"></i>
                                  </td>
                              </tr>
                          </ng-container>
                      </ng-container>
                      <ng-container *ngIf="refTableToCreate.ReferenceTableDetailCreateVMs?.length == 0">
                          <td colspan="4">Chưa có dữ liệu</td>
                      </ng-container>
                  </table>
              </div>
          </div>
          <div class="modal-footer" style="background-color: rgba(255,255,255,.2);">
              <button type="button" class="btn btn-danger" (click)="refTableModalRef.hide()">
                  <i class="fa fa-times"></i>
                  Hủy</button>
              <button type="button" class="btn btn-success" (click)="createRefTable()">
                  <i class="fa fa-check-square-o"></i>
                  Xác nhận</button>
          </div>
  <ng-template #createPair>
      <div class="modal-header">
          <h5 class="modal-title">Thêm Dữ Liệu Bảng Tra</h5>
          <button type="button" class="close" (click)="tempModalRef.hide()" aria-label="Close">
              <span aria-hidden="true">&times;</span>
          </button>
      </div>
      <div class="modal-body">
          <div class="form-group">
              <label for="input-1">Giá Trị</label>
              <input [(ngModel)]="refTableDetail.Key" class="form-control" type="text" required>
          </div>
          <div class="form-group">
              <label for="input-1">Trả Về</label>
              <input [(ngModel)]="refTableDetail.Value" class="form-control" type="text" required>
          </div>
          <!-- 
          <div class="form-group">
          <label>Kiểu trả về</label>
          <select #detailrt (change)="setRefTableDetailReturnType(detailrt.value)" class="form-control"
              style="">
              <option value="data">Dữ liệu</option>
              <option value="field">Đầu lương</option>
              <option value="formula">Công thức</option>
          </select>
          </div>
          -->
      </div>
      <div class="modal-footer">
          <button type="button" class="btn btn-danger" (click)="tempModalRef.hide()">
              <i class="fa fa-times"></i>
              Hủy</button>
          <button type="button" class="btn btn-success" (click)="addRefTableDetail();tempModalRef.hide();">
              <i class="fa fa-check-square-o"></i>
              Xác nhận</button>
      </div>
  </ng-template>
    `
})

export class ModalRefTableComponent implements OnInit {

  vm: DocumentCreateComponent;

  formulaEL: any;

  tempModalRef: BsModalRef;

  refTableDetail = new SPCSObject;

  refTableToCreate = new SPCSObject;

  formulaComponents: any;

  listAllFields = [];

  stateDisableCompareType = true;

  constructor(public refTableModalRef: BsModalRef, private modalService: BsModalService, private api: ApiService, private util: UtilityService) { }

  ngOnInit() {
    this.formulaComponents = JSON.parse(JSON.stringify(this.vm.formulaComponents));

    this.api.getAllFields().subscribe(
      res => {
        this.listAllFields = res;
      },
      err => {
        Swal.fire("Cảnh Báo", "Không lấy được danh sách đầu lương từ server, hãy kiểm tra kết nối", "warning");
      }
    )

    for (let i = 0; i < this.formulaComponents.length; i++) {
      var el = this.formulaComponents[i];
      if (this.util.isOperator(el.text[0])) {
        el.text = (el.text as String).substr(1);
      }
      // xét những trường nào mới hoặc refTable thì bỏ ra
      if (el.id.split("-").length == 1 || el.id.split("-")[2] == "2") {
        this.formulaComponents.splice(i, 1);
        i--;
      }
    }

    if (this.formulaEL.additional.RefTableTypeVM != null) {
      this.refTableToCreate = this.formulaEL.additional.RefTableTypeVM;
    }
    else {
      this.refTableToCreate.ReferenceTableDetailCreateVMs = [];
      this.refTableToCreate.Name = this.formulaEL.id;
      this.refTableToCreate.ReturnType = "4";
      this.refTableToCreate.Description = "";
      if (this.vm.formulaComponents.length > 0) {
        this.refTableToCreate.SourceType = this.vm.formulaComponents[0].additional.Type;
        this.refTableToCreate.SourceName = this.vm.formulaComponents[0].additional.Name;
      }
      else {
        this.refTableToCreate.SourceType = "";
        this.refTableToCreate.SourceName = "";
      }

      this.refTableToCreate.CompareType = 1;
    }

  }

  detailReturnType(value) {
    switch (value) {
      case "data":
        return "Dữ liệu";
      // case "field":
      //   return "Đầu lương";
      case "formula":
        return "Công thức";
      default:
        return value;
    }
  }

  createRefTable() {
    console.log(this.refTableToCreate);
    this.formulaEL.id = this.refTableToCreate.Name;
    this.formulaEL.text = this.refTableToCreate.Name;
    this.formulaEL.htmltag.innerHTML = this.refTableToCreate.Name;
    this.formulaEL.additional.Type = 2;
    this.formulaEL.additional.RefTableTypeVM = this.refTableToCreate;

    if (this.vm.formulaComponents.filter(a => a.id == this.refTableToCreate.Name).length == 0) {
      this.vm.formulaComponents.push(
        {
          id: this.refTableToCreate.Name,
          text: this.refTableToCreate.Name,
          additional: this.formulaEL.additional,
        }
      );
    }

    this.refTableModalRef.hide();
  }

  openCreatePairModal(createPair: TemplateRef<any>) {
    this.refTableDetail.ReturnType = "data";
    let config = {
      class: "modal-sm"
    }
    this.tempModalRef = this.modalService.show(createPair, config);

  }

  setRefTableDetailReturnType(value) {
    this.refTableDetail.ReturnType = value;
  }

  addRefTableDetail() {
    console.log(this.refTableDetail);
    this.refTableToCreate.ReferenceTableDetailCreateVMs.push(this.refTableDetail);
    if (this.refTableToCreate.ReturnType == "3") {
      // making
      let temp = new SPCSObject;

      // khởi tạo type
      let FormulaTypeVM = new SPCSObject
      FormulaTypeVM.FormulaType = 1;

      FormulaTypeVM.Name = this.refTableDetail.Value;
      FormulaTypeVM.IsSalaryFormula = false;
      FormulaTypeVM.Description = "formula";
      FormulaTypeVM.DocId = -1;
      FormulaTypeVM.FormulaDetailNCVMs = [];
      temp.Type = 3;
      temp.FormulaTypeVM = FormulaTypeVM;

      let temp2 = {
        id: this.refTableDetail.Value,
        text: this.refTableDetail.Value,
        additional: temp,
      }
      this.refTableDetail.FormulaTypeVM = FormulaTypeVM;
      this.vm.selectedList3.push(temp2);
    }
    else {

    }
    this.refTableDetail = {
      Key: "",
      Value: "",
      // ReturnType: "data"
    }
  }


  deleteRefTableDetail(index) {
    let value = this.refTableToCreate.ReferenceTableDetailCreateVMs[index].Value;
    let indexSL3;
    for (let i = 0; i < this.vm.selectedList3.length; i++) {
      const el = this.vm.selectedList3[i];
      if (el.id == value) {
        indexSL3 = i;
      }
    }
    this.vm.selectedList3.splice(indexSL3, 1);
    this.refTableToCreate.ReferenceTableDetailCreateVMs.splice(index, 1);
  }

  setRefTableSource(value) {
    // console.log(value);
    var temp = value.split("-");
    this.refTableToCreate.SourceType = Number(temp[2]);
    this.refTableToCreate.SourceValue = Number(temp[0]);
    if (this.refTableToCreate.SourceType == 3 ||
      (this.refTableToCreate.SourceType == 1 && this.listAllFields.filter(a => a.Id == this.refTableToCreate.SourceValue)[0].DataType == "number")) {
      this.stateDisableCompareType = false;
    }
    else {
      this.stateDisableCompareType = true;
      this.refTableToCreate.CompareType = "1";
      (document.getElementById("cpt") as HTMLSelectElement).value = "1";
    }
    console.log(this.refTableToCreate);
  }

  setRefTableReturnType(value) {
    this.refTableToCreate.ReturnType = value;
  }

  // chưa validate
  setRefTableCompareType(value) {
    this.refTableToCreate.CompareType = value
  }
}
