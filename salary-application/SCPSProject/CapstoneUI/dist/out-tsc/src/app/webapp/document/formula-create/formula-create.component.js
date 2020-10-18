// import { Component, OnInit, TemplateRef, ChangeDetectorRef, Input } from '@angular/core';
// import { Select2OptionData } from 'ng-select2';
// import { PrimitiveService } from 'src/app/primitive.service';
// import { ApiService } from 'src/app/api.service';
// import { ActivatedRoute, Router } from '@angular/router';
// import { UtilityService } from 'src/app/util.service';
// import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
// import { SPCSObject } from 'src/app/class/SPCSObject';
// declare var $: any;
// @Component({
//   selector: 'app-formula-create',
//   templateUrl: './formula-create.component.html',
//   styleUrls: ['./formula-create.component.css']
// })
// export class FormulaCreateComponent implements OnInit {
//   @Input() DocumentId: any;
//   public formulaComponents: Array<Select2OptionData>;
//   public selectedList = [[]];
//   public selectedList2 = [];
//   combinedComponents: any;
//   formulaToCreate = new SPCSObject;
//   Hash: any;
//   UserID: any;
//   modalRef: BsModalRef;
//   documentDetail: any;
//   constructor(private changeDetection: ChangeDetectorRef, private modalService: BsModalService, private validator: PrimitiveService, private api: ApiService, private route: ActivatedRoute, public router: Router, public util: UtilityService) { }
//   ngOnInit() {
//     this.route.queryParamMap.subscribe(params => {
//       // this.Hash = params.get('Hash');
//       // if (this.Hash == null || this.Hash == "") {
//       //   this.router.navigate([""]);
//       // }
//     });
//     this.getAllFormulaElements();
//   }
//   getAllFormulaElements() {
//     this.api.getAllFormulaElements()
//       .subscribe(res => {
//         this.combinedComponents = res;
//         this.formulaComponents = [];
//         for (let i = 0; i < this.combinedComponents.length; i++) {
//           const el = this.combinedComponents[i];
//           el.operator = 1;
//           let check = this.formulaComponents.filter((e) => {
//             let temp1 = el.Id + "-" + el.Name + "-" + el.Type;
//             let temp2 = e.id;
//             if (temp2 == null) temp2 = "~!can't duplicate thing!~";
//             // ID
//             if (temp1 == temp2) {
//               return el;
//             }
//           })
//           if (check.length == 0) {
//             this.formulaComponents.push(
//               {
//                 id: el.Id + "-" + el.Name + "-" + el.Type,
//                 text: el.Name,
//                 additional: el,
//               }
//             );
//           }
//         }
//         console.log(this.formulaComponents);
//       }, err => {
//         console.log(err);
//       });
//   }
//   ngAfterViewChecked() {
//     let length = this.selectedList2.length - 1;
//     let temp = document.getElementById("tooltip" + (length));
//     let more = 0;
//     if (this.selectedList2.length == 1) {
//       more = 45;
//     }
//     let offset;
//     if (this.selectedList2[length] != null) {
//       offset = this.selectedList2[length].htmltag.offsetLeft + 315 + more;
//     }
//     if (temp != null) {
//       temp.style.visibility = "visible";
//       temp.style.opacity = "1";
//       temp.style.top = (95 + 48 * (length)) + "px";
//       temp.style.left = offset + "px";
//     }
//   }
//   getDocumentById(Id) {
//     this.api.getDocumentById(Id)
//       .subscribe(res => {
//         this.documentDetail = res;
//         console.log(res);
//       }, err => {
//         console.log(err);
//       });
//   }
//   options(value) {
//     // console.log(value);
//     var vm = this;
//     return {
//       width: '600',
//       multiple: true,
//       tags: true,
//       createTag: function (e) {
//         return undefined;
//       },
//       myChanged: function (e) {
//         console.log(e);
//         console.log(vm.selectedList);
//         vm.selectedList[value] = e;
//       },
//       selectedList: [],
//       index: value,
//       ajax: {
//         transport: function (params, success, failure) {
//           let data = vm.formulaComponents || [];
//           if (params.data._type == 'query' && params.data.q) {
//             let query = params.data.q;
//             data = data.filter(a => a.text.indexOf(query) != -1);
//             let temp = new SPCSObject();
//             if (vm.util.IsOperator(query.substr(0, 1))) {
//               temp.operator = vm.util.NumOperator(query.substr(0, 1));
//               query = query.substr(1);
//             }
//             else {
//               temp.operator = 1;
//             }
//             if (data.length == 0) {
//               let obj = {
//                 id: query
//                 , text: `${query}`
//                 , additional: temp
//                 , iscreate: true
//               }
//               data.push(obj);
//             }
//           }
//           success({
//             results: data.map((d, i) => ({ index: i, id: d.id, text: d.text, additional: d.additional }))
//           })
//         }
//       },
//       templateSelection: function (state) {
//         console.log("state");
//         console.log(state);
//         console.log("index");
//         console.log(vm.selectedList[value].indexOf(state));
//         if (state.additional == null) {
//           return $('<div>', {
//             'class': 'spcs'
//             , 'style': 'width: auto;'
//             , 'text': state.text
//             , 'data-operator': state.additional.operator
//           }).on('click', function (e) {
//             e.stopPropagation();
//             // process CSS
//             $('.spcs.active').removeClass('active');
//             $(this).addClass('active');
//             if (state.functionOpenMenu == null) {
//               state.functionOpenMenu = function ftMenu(e) {
//                 // e.preventDefault();
//                 e.stopPropagation();
//                 vm.openMenuModal(state, this);
//               }
//               this.addEventListener('click', state.functionOpenMenu);
//             }
//           });
//         } else {
//           return $('<div>', {
//             'class': 'spcs'
//             , 'style': 'width: auto;'
//             , 'text': state.text
//             , 'data-operator': state.additional.operator
//           }).on('click', function (e) {
//             e.stopPropagation();
//             // process CSS
//             $('.spcs.active').removeClass('active');
//             $(this).addClass('active');
//             // vm.openSymbolModal(formulaEL, this);
//             if (state.functionOpenMenu != null) {
//               this.removeEventListener('click', state.functionOpenMenu);
//             }
//           });
//         }
//       },
//       templateResult: function (state) {
//         if (!state.id) {
//           return state.text;
//         }
//         console.log("state");
//         console.log(state);
//         console.log("index");
//         console.log(vm.selectedList[value].indexOf(state));
//         if (state.additional.Id == null) {
//           var baseUrl = "#";
//           return $(
//             '<span><img src="' + baseUrl + '.png" class="img-flag" /> ' + 'Tạo mới "' + state.text + '"</span>'
//           ).on('click', function (e) {
//             e.stopPropagation();
//             // process CSS
//             // $('.spcs.active').removeClass('active');
//             // $(this).addClass('active');
//             // if (state.functionOpenMenu == null) {
//             //   state.functionOpenMenu = function ftMenu(e) {
//             //     // e.preventDefault();
//             //     e.stopPropagation();
//             //     vm.openMenuModal(state, this);
//             //   }
//             //   this.addEventListener('click', state.functionOpenMenu);
//             // }
//           });
//         } else {
//           return $(
//             '<span><img src="' + baseUrl + '.png" class="img-flag" /> ' + state.text + '</span>'
//           ).on('click', function (e) {
//             e.stopPropagation();
//             // process CSS
//             // $('.spcs.active').removeClass('active');
//             // $(this).addClass('active');
//             // if (state.functionOpenMenu != null) {
//             //   this.removeEventListener('click', state.functionOpenMenu);
//             // }
//           });
//         }
//       },
//     };
//   }
//   triggerClick() {
//   }
//   openMenuModal(formulaEL, tag) {
//     console.log(formulaEL);
//     const initialState = {
//       vm: this,
//       formulaEL: formulaEL,
//       tag: tag,
//     };
//     this.modalRef = this.modalService.show(
//       ModalMenuComponent, { initialState }
//     );
//   }
//   openFieldModal(fEL) {
//     const initialState = {
//       vm: this,
//       formulaEL: fEL
//     };
//     this.modalRef = this.modalService.show(
//       ModalFieldComponent, { initialState: initialState }
//     );
//   }
//   openReftableModal(fEL) {
//     const initialState = {
//       vm: this,
//       formulaEL: fEL
//     };
//     this.modalRef = this.modalService.show(
//       ModalRefTableComponent, { initialState: initialState, class: "modal-lg" }
//     );
//   }
//   createSalaryFormula() {
//     // truyen data
//     this.formulaToCreate.Name = this.documentDetail.Code;
//     this.formulaToCreate.IsSalaryFormula = true;
//     this.formulaToCreate.Description = "formula";
//     this.formulaToCreate.DocId = this.DocumentId;
//     this.formulaToCreate.FormulaDetailCreateVMs = [];
//     for (let i = 0; i < this.selectedList[this.selectedList.length - 1].length; i++) {
//       const el = this.selectedList[this.selectedList.length - 1][i];
//       let component = new SPCSObject;
//       component.Type = el.additional.Type;
//       component.Ordinal = i + 1;
//       component.Operator = el.additional.operator;
//       component.FDType = {
//         Id: el.additional.Id,
//         Value: Number(el.additional.Name)
//       }
//       if (isNaN(component.FDType.Value)) component.FDType.Value = 0;
//       this.formulaToCreate.FormulaDetailCreateVMs.push(component);
//     }
//     console.log(this.formulaToCreate);
//     this.api.createFormula(this.formulaToCreate)
//       .subscribe(
//         res => {
//           alert("Tạo công thức lương thành công");
//           location.href = 'document/detail?DocumentId=' + this.DocumentId;
//         },
//         err => {
//           // alert("tạo salary formula lỗi");
//         }
//       );
//   }
//   createFormula() {
//     // truyen data
//     this.formulaToCreate.Name = this.selectedList2[this.selectedList2.length - 1].text;
//     this.formulaToCreate.IsSalaryFormula = false;
//     this.formulaToCreate.Description = "formula";
//     this.formulaToCreate.DocId = -1;
//     this.formulaToCreate.FormulaDetailCreateVMs = [];
//     for (let i = 0; i < this.selectedList[this.selectedList.length - 1].length; i++) {
//       const el = this.selectedList[this.selectedList.length - 1][i];
//       let component = new SPCSObject;
//       if (el.additional == null) {
//         let innerhtml = el.id;
//         let operator = -1;
//         let value = Number(innerhtml.split(' ')[1]);
//         console.log("const = " + value);
//         switch (innerhtml.split(' ')[0]) {
//           case "+":
//             operator = 1;
//             break;
//           case "-":
//             operator = 2;
//             break;
//           case "*":
//             operator = 3;
//             break;
//           case "/":
//             operator = 4;
//             break;
//           default:
//             alert("hằng số không đúng định dạng")
//             break;
//         }
//         component = {
//           Type: 4,
//           Operator: operator,
//         }
//         component.Type = 4;
//         component.Operator = operator;
//         component.FDType = {
//           Id: 0,
//           Value: value
//         }
//       }
//       else {
//         component.Type = el.additional.Type;
//         component.Operator = el.additional.operator;
//         component.FDType = {
//           Id: el.additional.Id,
//           Value: Number(el.additional.Name)
//         }
//       }
//       component.Ordinal = i + 1;
//       if (isNaN(component.FDType.Value)) component.FDType.Value = 0;
//       this.formulaToCreate.FormulaDetailCreateVMs.push(component);
//     }
//     console.log(this.formulaToCreate);
//     // call api
//     this.api.createFormula(this.formulaToCreate)
//       .subscribe(
//         res => {
//           // this.getAllFormulaElements();
//           this.selectedList2[this.selectedList2.length - 1].additional = { Id: res.Id, Type: 3, Name: res.Name }
//           console.log("sau khi tạo formula");
//           console.log(this.selectedList2[this.selectedList2.length - 1]);
//           console.log(this.selectedList);
//           alert("Tạo công thức thành công");
//           this.selectedList2.splice(-1, 1);
//           this.selectedList.splice(-1, 1);
//         },
//         err => {
//           // alert("tạo salary formula lỗi");
//         }
//       );
//   }
// }
// @Component({
//   selector: 'modal-symbol',
//   template: `
//   <div class="modal-header">
//             <h5 class="modal-title">Tính Năng</h5>
//             <button type="button" class="close" (click)="menuModalRef.hide()" aria-label="Close">
//                 <span aria-hidden="true">&times;</span>
//             </button>
//         </div>
//         <div class="modal-body">
//             <div class="btn-group m-1" style="margin: 6% !important;">
//                 <button type="button" class="btn btn-success waves-effect waves-light" (click)="openFieldModal()">Tạo Đầu Lương</button>
//                 <button type="button" class="btn btn-info waves-effect waves-light" (click)="openRefModal()">Tạo Bảng Tra</button>
//                 <button type="button" class="btn btn-primary waves-effect waves-light" (click)="openFormulaModal()">Tạo Công Thức</button>
//             </div>
//         </div>
//     `
// })
// export class ModalMenuComponent implements OnInit {
//   vm: FormulaCreateComponent;
//   formulaEL: any;
//   tag: any;
//   constructor(public menuModalRef: BsModalRef) { }
//   ngOnInit() {
//     console.log("menu modal")
//     console.log(this.formulaEL);
//     console.log(this.tag);
//   }
//   openFieldModal() {
//     this.vm.openFieldModal(this.formulaEL);
//     this.menuModalRef.hide();
//   }
//   openRefModal() {
//     this.vm.openReftableModal(this.formulaEL);
//     this.menuModalRef.hide();
//   }
//   openFormulaModal() {
//     this.formulaEL.htmltag = this.tag;
//     this.vm.selectedList2.push(this.formulaEL);
//     this.menuModalRef.hide();
//     console.log(this.formulaEL);
//   }
// }
// @Component({
//   selector: 'modal-field',
//   template: `
//   <div class="modal-header">
//     <h5 class="modal-title">Tạo Đầu Lương Mới</h5>
//     <button type="button" class="close" (click)="fieldModalRef.hide()" aria-label="Close">
//         <span aria-hidden="true">&times;</span>
//     </button>
//   </div>
//   <div class="modal-body">
//     <div class="form-group">
//         <label for="input-1">Tên viết tắt</label>
//         <input class="form-control" type="text" id="input-1" [(ngModel)]="fieldToCreate.Name" required>
//     </div>
//     <div class="form-group">
//         <label for="input-2">Tên đầy đủ</label>
//         <input class="form-control" type="text" id="input-2" [(ngModel)]="fieldToCreate.LongName" required>
//     </div>
//     <div class="form-group">
//         <label for="input-3">Kiểu dữ liệu</label>
//         <select class="form-control" id="input-3" [(ngModel)]="fieldToCreate.DataType" >
//             <option value="number">Số</option>
//             <option value="string">Chuỗi</option>
//         </select>
//     </div>
//     <div class="form-group">
//         <label for="input-5">mô tả</label>
//         <input class="form-control" type="text" [(ngModel)]="fieldToCreate.Description" required>
//     </div>
//     <div class="form-group">
//       <div class="icheck-material-white">
//         <input type="checkbox" id="input-4" [(ngModel)]="fieldToCreate.IsMonthlySalaryComponent" checked>
//         <label for="input-4">Nạp thông tin mỗi tháng</label>
//       </div>
//     </div>
//   </div>
//   <div class="modal-footer">
//     <button type="button" class="btn btn-danger" (click)="fieldModalRef.hide()">
//         <i class="fa fa-times"></i>
//         Hủy</button>
//     <button type="button" class="btn btn-success" (click)="createField()">
//         <i class="fa fa-check-square-o"></i>
//         Xác nhận</button>
//   </div>
//     `
// })
// export class ModalFieldComponent implements OnInit {
//   vm: FormulaCreateComponent;
//   fieldToCreate = {
//     Name: "",
//     LongName: "",
//     DataType: "",
//     Description: "",
//     IsMonthlySalaryComponent: true,
//   };
//   formulaEL: any;
//   constructor(public fieldModalRef: BsModalRef, private api: ApiService, ) { }
//   ngOnInit() {
//     this.fieldToCreate.Name = this.formulaEL.text;
//   }
//   createField() {
//     console.log(this.fieldToCreate);
//     this.api.createField(this.fieldToCreate)
//       .subscribe(
//         res => {
//           // hold record được tạo mới
//           // this.vm.getAllFormulaElements();
//           this.formulaEL.additional = { Id: res.Id, Type: 1, Name: res.Name };
//           alert("Tạo đầu lương thành công")
//           this.vm.triggerClick();
//           this.fieldModalRef.hide();
//         },
//         err => {
//           console.log(err);
//         }
//       );
//   }
// }
// @Component({
//   selector: 'modal-refTable',
//   template: `
//           <div class="modal-header" style="background-color: rgba(255,255,255,.2);">
//               <h5 class="modal-title">Tạo Bảng Tra</h5>
//               <button type="button" class="close" (click)="refTableModalRef.hide()" aria-label="Close">
//                   <span aria-hidden="true">&times;</span>
//               </button>
//           </div>
//           <div class="modal-body" style="float: left; width: 100%; background-color: rgba(255,255,255,.2);">
//               <div style="float:left; width: 36%;">
//                   <div class="form-group">
//                       <label for="input-1">Tên bảng tra</label>
//                       <input class="form-control" type="text" id="input-1" [(ngModel)]="refTableToCreate.Name"
//                           required>
//                   </div>
//                   <div class="form-group">
//                       <label>Mô tả</label>
//                       <input [(ngModel)]="refTableToCreate.Description" class="form-control" type="text"
//                           placeholder="Mô tả">
//                   </div>
//                   <div class="form-group">
//                       <label>Đối tượng so sánh</label>
//                       <select #scc (change)="setRefTableSource(scc.value)" class="form-control"
//                           style="margin-left: 10px; width: 200px;">
//                           <ng-container *ngFor="let cc of vm.formulaComponents; index as indexId">
//                               <option value="{{cc.id}}">{{cc.text}}</option>
//                           </ng-container>
//                       </select>
//                   </div>
//                   <div class="form-group">
//                       <label>Kiểu trả về</label>
//                       <select #rt (change)="setRefTableReturnType(rt.value)" class="form-control"
//                           style="margin-left: 10px; width: 200px;">
//                           <option value="number">Số</option>
//                           <option value="string">Chuỗi</option>
//                       </select>
//                   </div>
//               </div>
//               <div style="float:left; width: 60%; margin-left:4%;">
//                   <p>Bảng tham chiếu
//                       <i (click)="openCreatePairModal(createPair)" class="fa fa-plus-circle fa-lg"
//                           style="color: white;"></i>
//                   </p>
//                   <table class="table table-bordered">
//                       <colgroup>
//                           <col style="width: 44%;">
//                           <col style="width: 44%;">
//                           <col style="width: 12%;">
//                       </colgroup>
//                       <tr>
//                           <th>GIÁ TRỊ</th>
//                           <th>TRẢ VỀ</th>
//                           <th></th>
//                       </tr>
//                       <ng-container
//                           *ngFor="let ref of refTableToCreate.ReferenceTableDetailCreateVMs; index as indexId">
//                           <ng-container>
//                               <tr>
//                                   <td>
//                                       {{ref.Key}}
//                                   </td>
//                                   <td>
//                                       {{ref.Value}}
//                                   </td>
//                                   <td>
//                                       <i class="fa fa-trash fa-2x" style="color: red;"
//                                           (click)="deleteRefTableDetail(indexId)"></i>
//                                   </td>
//                               </tr>
//                           </ng-container>
//                       </ng-container>
//                       <ng-container *ngIf="refTableToCreate.ReferenceTableDetailCreateVMs?.length == 0">
//                           <td colspan="3">Chưa có dữ liệu</td>
//                       </ng-container>
//                   </table>
//               </div>
//           </div>
//           <div class="modal-footer" style="background-color: rgba(255,255,255,.2);">
//               <button type="button" class="btn btn-danger" (click)="refTableModalRef.hide()">
//                   <i class="fa fa-times"></i>
//                   Hủy</button>
//               <button type="button" class="btn btn-success" (click)="createRefTable()">
//                   <i class="fa fa-check-square-o"></i>
//                   Xác nhận</button>
//           </div>
//   <ng-template #createPair>
//       <div class="modal-header">
//           <h5 class="modal-title">Tính Năng</h5>
//           <button type="button" class="close" (click)="tempModalRef.hide()" aria-label="Close">
//               <span aria-hidden="true">&times;</span>
//           </button>
//       </div>
//       <div class="modal-body">
//           <div class="form-group">
//               <label for="input-1">Giá Trị</label>
//               <input [(ngModel)]="refTableDetail.Key" class="form-control" type="text" required>
//           </div>
//           <div class="form-group">
//               <label for="input-1">Trả Về</label>
//               <input [(ngModel)]="refTableDetail.Value" class="form-control" type="text" required>
//           </div>
//       </div>
//       <div class="modal-footer">
//           <button type="button" class="btn btn-danger" (click)="tempModalRef.hide()">
//               <i class="fa fa-times"></i>
//               Hủy</button>
//           <button type="button" class="btn btn-success" (click)="addRefTableDetail();tempModalRef.hide();">
//               <i class="fa fa-check-square-o"></i>
//               Xác nhận</button>
//       </div>
//   </ng-template>
//     `
// })
// export class ModalRefTableComponent implements OnInit {
//   vm: FormulaCreateComponent;
//   formulaEL: any;
//   tempModalRef: BsModalRef;
//   refTableDetail = new SPCSObject;
//   refTableToCreate = new SPCSObject;
//   constructor(public refTableModalRef: BsModalRef, private modalService: BsModalService, private api: ApiService, ) { }
//   ngOnInit() {
//     this.refTableToCreate.ReferenceTableDetailCreateVMs = [];
//     this.refTableToCreate.Name = this.formulaEL.text;
//     this.refTableToCreate.ReturnType = "number";
//     this.refTableToCreate.Description = "";
//     if (this.vm.formulaComponents.length > 0) {
//       this.refTableToCreate.SourceType = this.vm.formulaComponents[0].additional.Type;
//       this.refTableToCreate.SourceValue = this.vm.formulaComponents[0].additional.Id;
//     }
//     else {
//       this.refTableToCreate.SourceType = "";
//       this.refTableToCreate.SourceValue = "";
//     }
//   }
//   createRefTable() {
//     console.log(this.refTableToCreate);
//     this.api.createReferenceTable(this.refTableToCreate)
//       .subscribe(
//         res => {
//           // this.vm.getAllFormulaElements();
//           this.formulaEL.additional = { Id: res.Id, Type: 2, Name: res.Name };
//           alert("Tạo bảng tra thành công")
//           this.vm.triggerClick();
//           this.refTableModalRef.hide();
//         },
//         err => {
//           console.log(err);
//         }
//       );
//   }
//   openCreatePairModal(createPair: TemplateRef<any>) {
//     this.tempModalRef = this.modalService.show(createPair);
//   }
//   addRefTableDetail() {
//     console.log(this.refTableDetail);
//     this.refTableToCreate.ReferenceTableDetailCreateVMs.push(this.refTableDetail);
//     this.refTableDetail = {
//       Key: "",
//       Value: "",
//     }
//   }
//   deleteRefTableDetail(index) {
//     this.refTableToCreate.ReferenceTableDetailCreateVMs.splice(index, 1);
//   }
//   setRefTableSource(value) {
//     // console.log(value);
//     var temp = value.split("-");
//     this.refTableToCreate.SourceType = Number(temp[0]);
//     this.refTableToCreate.SourceValue = Number(temp[1]);
//     console.log(this.refTableToCreate);
//   }
//   setRefTableReturnType(value) {
//     this.refTableToCreate.ReturnType = value;
//   }
// }
//# sourceMappingURL=formula-create.component.js.map