import { __decorate } from "tslib";
import { Component, ViewChild } from '@angular/core';
import { SPCSObject } from 'src/app/class/SPCSObject';
import { MAT_STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
let DocumentCreateComponent = class DocumentCreateComponent {
    constructor(dialog, modalService, validator, api, route, router, util) {
        this.dialog = dialog;
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
        this.thisDate = new Date();
        this.selectedList = [[]];
        this.selectedList2 = [];
        this.cache = new SPCSObject;
        this.formulaToCreate = new SPCSObject;
        this.documentToCreateNewModel = new FormData;
        this.documentToCreate = new SPCSObject;
        this.documentAfterCreate = new SPCSObject;
        this.isLinearStepper = true;
        this.checkStepperArr = [false, false, false, false];
        // listField for test
        this.listTest = [];
        this.listField = [];
        this.listResultTest = [];
        this.btnCheckState = true;
        this.tempOperator = 1;
        this.ft = new SPCSObject;
        this.formData = new FormData();
        this.select2BindModel = [];
        this.fReview = [{ Name: "", Expression: "" }];
    }
    ngOnInit() {
        this.ft.deleteTest = [];
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
                // console.log(state);
                if (e.which == 13) {
                    $('.spcs.active').removeClass('active');
                }
                else if (e.which == 187) {
                    state.additional.operator = 1;
                }
                else if (e.which == 189) {
                    state.additional.operator = 2;
                }
                else if (e.which == 56) {
                    state.additional.operator = 3;
                }
                else if (e.which == 191) {
                    state.additional.operator = 4;
                }
                else {
                    // console.log(e.which);
                }
                $('.spcs.active').attr('data-operator', state.additional.operator);
            }
        });
    }
    ngAfterViewChecked() {
        this.showTooltip();
    }
    convertDate(str) {
        var date = new Date(str), mnth = ("0" + (date.getMonth() + 1)).slice(-2), day = ("0" + date.getDate()).slice(-2);
        return [date.getFullYear(), mnth, day].join("-");
    }
    validateStep1(stepper) {
        if (this.documentToCreate.Code == "" || this.documentToCreate.Code == null) {
            alert("Chưa nhập Số Hiệu cho Quyết Định");
            return;
        }
        if (this.documentToCreate.file == null) {
            alert("Chưa tải lên file Quyết Định");
            return;
        }
        if (this.documentToCreate.CloseDay == "") {
            alert("Chưa nhập Ngày Tính Lương cho Quyết Định");
            return;
        }
        if (this.documentToCreate.Deadline == "") {
            alert("Chưa nhập Ngày Tính Lương cho Quyết Định");
            return;
        }
        if (this.documentToCreate.CloseDay < 1 || this.documentToCreate.CloseDay > 28) {
            alert("Chưa nhập Ngày Tính Lương cho Quyết Định");
            return;
        }
        if (this.documentToCreate.Deadline < 1 || this.documentToCreate.Deadline > 28) {
            alert("Chưa nhập Ngày Tính Lương cho Quyết Định");
            return;
        }
        console.log(this.convertDate("Thu Jun 09 2011 00:00:00 GMT+0530 (India Standard Time)"));
        console.log(this.documentToCreate);
        this.goForward(stepper);
    }
    triggerEventStep(event) {
        console.log(event);
        if (event.selectedIndex == 2) {
            this.createSalaryFormula(null);
            this.listTest = [];
            this.listResultTest = [];
        }
    }
    openCreateTestFieldsModal(testFields) {
        let config = {
            class: "modal-lg"
        };
        this.modalRef = this.modalService.show(testFields, config);
    }
    addListTest() {
        let temp = JSON.parse(JSON.stringify(this.listField));
        console.log(temp);
        this.listTest.push(temp);
        // for (let i = 0; i < this.listField.length; i++) {
        //   const el = this.listField[i];
        //   el.Value = "";
        // }
    }
    deleteListTest(index) {
        console.log("delete " + index);
        this.listTest.splice(index, 1);
    }
    goBack(stepper) {
        stepper.previous();
    }
    goForward(stepper) {
        stepper.selected.completed = true;
        stepper.next();
    }
    onFileChange(files) {
        this.documentToCreate.file = files.item(0);
        console.log(files.item(0));
        console.log(files[0]);
        console.log(files);
        // this.documentToCreateNewModel.append("file", this.documentToCreate.file);
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
                    if (temp2 == null)
                        temp2 = "~!can't duplicate thing!~";
                    // ID
                    if (temp1 == temp2) {
                        return el;
                    }
                });
                if (check.length == 0) {
                    this.formulaComponents.push({
                        id: el.Id + "-" + el.Name + "-" + el.Type,
                        text: el.Name,
                        additional: el,
                    });
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
            more = 45;
        }
        let offset;
        if (this.selectedList2[length] != null && this.selectedList2[length].htmltag != null) {
            offset = this.selectedList2[length].htmltag.offsetLeft + 65 + more;
        }
        if (temp != null) {
            temp.style.visibility = "visible";
            temp.style.opacity = "1";
            temp.style.top = (45 + 48 * (length)) + "px";
            temp.style.left = offset + "px";
        }
    }
    options(value) {
        var vm = this;
        var selected = [];
        // vm.selectedList[value] = [];
        if (vm.selectedList2[(value - 1)] != null) {
            selected = this.select2BindModel[vm.selectedList2[value - 1].id] || [];
        }
        return {
            width: '750',
            multiple: true,
            tags: true,
            allowduplicate: true,
            createTag: function (e) {
                return undefined;
            },
            onUnselect: function (data, selection) {
                if (vm.selectedList2.length > 0 && data.text == vm.selectedList2[vm.selectedList2.length - 1].text) {
                    vm.selectedList2.splice(-1, 1);
                    vm.selectedList.splice(-1, 1);
                }
            },
            myChanged: function (sl) {
                console.log("myChanged");
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
                        console.log("data2-1");
                        console.log(data2);
                        // search
                        data = data.filter(a => a.text.indexOf(query) != -1);
                        data2 = data2.filter(a => a.additional.Name == query);
                        console.log("data2-2");
                        console.log(data2);
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
                                id: query,
                                text: `Tạo mới hằng số "${query}"`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data = [obj];
                        }
                        else if (vm.util.equalsIgnoreCase(params.data.q, "Min(")) {
                            let tempMin = {
                                id: query,
                                text: `Tạo mới Min(...)`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data.push(tempMin);
                        }
                        else if (vm.util.equalsIgnoreCase(params.data.q, "Max(")) {
                            let tempMax = {
                                id: query,
                                text: `Tạo mới Max(...)`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data.push(tempMax);
                        }
                        else if (vm.util.equalsIgnoreCase(params.data.q, "Avg(")) {
                            let tempAvg = {
                                id: query,
                                text: `Tạo mới Avg(...)`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data.push(tempAvg);
                        }
                        else if (vm.util.equalsIgnoreCase(params.data.q, "Rng(")) {
                            let tempRng = {
                                id: query,
                                text: `Tạo mới Rng(...)`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data.push(tempRng);
                        }
                        else if (vm.util.equalsIgnoreCase(params.data.q, "(")) {
                            let tempRng = {
                                id: query,
                                text: `Tạo mới (...)`,
                                additional: JSON.parse(JSON.stringify(temp)) // just copy value
                            };
                            data.push(tempRng);
                        }
                        else {
                            // flow create new component
                            let cmp = [
                                `đầu lương định kỳ `,
                                `đầu lương `,
                                `bảng tra `,
                                `công thức `,
                            ];
                            // compare search to name of all system
                            if (data2.length == 0) {
                                // check if query is not a number -> new component else -> const
                                if (isNaN(query)) {
                                    // create new component
                                    // field or refTable or formula
                                    for (let i = 0; i < cmp.length; i++) {
                                        let obj = {
                                            id: query,
                                            text: `Tạo mới ${cmp[i]} "${query}"`,
                                            additional: JSON.parse(JSON.stringify(temp)) // just copy value
                                        };
                                        data.push(obj);
                                    }
                                }
                                else {
                                    // create new const
                                    let obj = {
                                        id: query,
                                        text: `Tạo mới hằng số "${query}"`,
                                        additional: JSON.parse(JSON.stringify(temp)) // just copy value
                                    };
                                    data.push(obj);
                                }
                            }
                        }
                    }
                    success({
                        results: data.map((d, i) => ({ index: i, id: d.id, text: d.text, additional: d.additional }))
                    });
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
                    return $('<span>' + ' <i class="fa fa-plus-circle"></i> ' + temp.text + '</span>');
                }
                else {
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
                    return $('<span><img width="14" height="14" src="' + url + '" class="img-flag" /> ' + temp.text + '</span>').on('click', function (e) {
                        e.stopPropagation();
                    });
                }
            },
            templateSelection: function (state) {
                let result;
                console.log("state!!!!!!!!!!!!!!!!!!!!!!!");
                console.log(state);
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
                if (state.additional.Id == null) { // tạo mới
                    result = $('<div>', {
                        'class': 'spcs',
                        'style': 'width: auto;',
                        'text': state.id,
                        'data-operator': state.additional.operator
                    }).data('state', state).on('click', function (e) {
                        e.stopPropagation();
                        // process CSS
                        $('.spcs.active').removeClass('active');
                        $(this).addClass('active');
                    });
                    if (vm.select2BindModel[state.id] != null) {
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
                                vm.selectedList2.push(state);
                                vm.selectedList.push(tempModel);
                                // alert("Tạo mới công thức");
                                console.log(vm.selectedList);
                                console.log(vm.selectedList2);
                            }
                        });
                    }
                    if (state.text.includes("Tạo mới đầu lương định kỳ ")) {
                        console.log("problem!!!!");
                        // state.text = state.id;
                        state.additional.Type = -1;
                        state.additional.Name = state.id;
                        result.on('dblclick', function (e) {
                            e.stopPropagation();
                            vm.openFieldModal('payroll', state);
                        });
                    }
                    if (!state.text.includes("Tạo mới đầu lương định kỳ ") && state.text.includes("Tạo mới đầu lương ")) {
                        // state.text = state.id;
                        state.additional.Type = -1;
                        state.additional.Name = state.id;
                        result.on('dblclick', function (e) {
                            e.stopPropagation();
                            vm.openFieldModal('string', state);
                        });
                    }
                    if (state.text.includes("Tạo mới bảng tra ")) {
                        // state.text = state.id;
                        state.additional.Type = -1;
                        state.additional.Name = state.id;
                        result.on('dblclick', function (e) {
                            e.stopPropagation();
                            vm.openReftableModal(state);
                        });
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
                                vm.selectedList2.push(state);
                                vm.selectedList.push(tempModel);
                                // alert("Tạo mới công thức");
                                console.log(vm.selectedList);
                                console.log(vm.selectedList2);
                            }
                        });
                    }
                    switch (state.text) {
                        case `Tạo mới (...)`:
                            // agreegation range
                            result.on('dblclick', function (e) {
                                e.stopPropagation();
                                if (vm.selectedList2.indexOf(state) == -1) {
                                    // nếu chưa tồn tại thì khởi tạo
                                    if (state.additional.FormulaTypeVM == null) {
                                        state.additional.FormulaTypeVM = new SPCSObject;
                                        // formula expression
                                        state.additional.FormulaTypeVM.FormulaType = 6;
                                    }
                                    vm.selectedList2.push(state);
                                    var temp = vm.select2BindModel[state.id] || [];
                                    vm.selectedList.push(temp);
                                    console.log(vm.selectedList);
                                    console.log(vm.selectedList2);
                                }
                            });
                        case `Tạo mới Min(...)`:
                            // // aggregation min
                            result.on('dblclick', function (e) {
                                e.stopPropagation();
                                if (vm.selectedList2.indexOf(state) == -1) {
                                    // nếu chưa tồn tại thì khởi tạo
                                    if (state.additional.FormulaTypeVM == null) {
                                        state.additional.FormulaTypeVM = new SPCSObject;
                                        // formula expression
                                        state.additional.FormulaTypeVM.FormulaType = 2;
                                    }
                                    vm.selectedList2.push(state);
                                    var temp = vm.select2BindModel[state.id] || [];
                                    vm.selectedList.push(temp);
                                    console.log(vm.selectedList);
                                    console.log(vm.selectedList2);
                                }
                            });
                        case `Tạo mới Max(...)`:
                            // // aggregation max
                            result.on('dblclick', function (e) {
                                e.stopPropagation();
                                if (vm.selectedList2.indexOf(state) == -1) {
                                    // nếu chưa tồn tại thì khởi tạo
                                    if (state.additional.FormulaTypeVM == null) {
                                        state.additional.FormulaTypeVM = new SPCSObject;
                                        // formula expression
                                        state.additional.FormulaTypeVM.FormulaType = 3;
                                    }
                                    vm.selectedList2.push(state);
                                    var temp = vm.select2BindModel[state.id] || [];
                                    vm.selectedList.push(temp);
                                    console.log(vm.selectedList);
                                    console.log(vm.selectedList2);
                                }
                            });
                        case `Tạo mới Avg(...)`:
                            // aggregation average
                            result.on('dblclick', function (e) {
                                e.stopPropagation();
                                if (vm.selectedList2.indexOf(state) == -1) {
                                    // nếu chưa tồn tại thì khởi tạo
                                    if (state.additional.FormulaTypeVM == null) {
                                        state.additional.FormulaTypeVM = new SPCSObject;
                                        // formula expression
                                        state.additional.FormulaTypeVM.FormulaType = 4;
                                    }
                                    vm.selectedList2.push(state);
                                    var temp = vm.select2BindModel[state.id] || [];
                                    vm.selectedList.push(temp);
                                    console.log(vm.selectedList);
                                    console.log(vm.selectedList2);
                                }
                            });
                        case `Tạo mới Rng(...)`:
                            // agreegation range
                            result.on('dblclick', function (e) {
                                e.stopPropagation();
                                if (vm.selectedList2.indexOf(state) == -1) {
                                    // nếu chưa tồn tại thì khởi tạo
                                    if (state.additional.FormulaTypeVM == null) {
                                        state.additional.FormulaTypeVM = new SPCSObject;
                                        // formula expression
                                        state.additional.FormulaTypeVM.FormulaType = 5;
                                    }
                                    vm.selectedList2.push(state);
                                    var temp = vm.select2BindModel[state.id] || [];
                                    vm.selectedList.push(temp);
                                    console.log(vm.selectedList);
                                    console.log(vm.selectedList2);
                                }
                            });
                        default:
                            break;
                    }
                    state.htmltag = result[0];
                    console.log(result);
                    return result;
                }
                else {
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
                    result = $('<div>', {
                        'class': 'spcs',
                        'style': 'width: auto; ' + color,
                        'text': state.text,
                        'data-operator': state.additional.operator
                    }).data('state', state).on('click', function (e) {
                        e.stopPropagation();
                        // process CSS
                        $('.spcs.active').removeClass('active');
                        $(this).addClass('active');
                    });
                    state.htmltag = result[0];
                    return result;
                }
            },
            selections: {
                data: function (data) {
                    // console.log("current");
                    // console.log(selected);
                    return selected;
                },
                select: function (data) {
                    // if (selected.indexOf(data) == -1) {
                    //   selected.push(data);
                    // }
                },
                unselect: function (data) {
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
        this.modalRef = this.modalService.show(ModalFieldComponent, {
            initialState: initialState,
        });
    }
    openReftableModal(fEL) {
        const initialState = {
            vm: this,
            formulaEL: fEL
        };
        this.modalRef = this.modalService.show(ModalRefTableComponent, { initialState: initialState, class: "modal-lg" });
    }
    createFormula() {
        console.log("create formula");
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
                let tempName = el.id;
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
                if (el.additional.Type == null) {
                    // constant sẽ ko khởi tạo type
                    component.Type = 4;
                    component.Operator = el.additional.operator;
                    component.ConstantTypeVM = {
                        Value: Number(el.text)
                    };
                }
                else {
                    component.Type = el.additional.Type;
                    component.Operator = el.additional.operator;
                    if (el.additional.Id != null) {
                        component.Id = el.id;
                        component.FDTypeVM = {
                            Id: el.additional.Id,
                            Value: Number(el.additional.Name)
                        };
                        if (isNaN(component.FDTypeVM.Value))
                            component.FDTypeVM.Value = 0;
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
            console.log("formula");
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
            if (this.formulaComponents.filter(a => a.id == temp.Name).length == 0) {
                this.formulaComponents.push({
                    id: temp.Name,
                    text: temp.Name,
                    additional: k.additional,
                });
            }
            this.select2BindModel[temp.Name] = this.selectedList[this.selectedList.length - 1];
            console.log("ref formula");
            console.log(this.selectedList2[this.selectedList2.length - 1]);
            console.log(this.selectedList);
            console.log(this.select2BindModel);
            this.selectedList2.splice(-1, 1);
            this.selectedList.splice(-1, 1);
        }
        else {
            alert("Không được bỏ trống công thức");
        }
    }
    createSalaryFormula(stepper) {
        // validate select2
        console.log(this.selectedList);
        if (this.selectedList.length > 1) {
            alert("Hãy hoàn thành tất cả công thức");
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
                console.log(el);
                if (el.additional.Type == -1) {
                }
                if (el.additional.Type == null) {
                    // constant sẽ ko khởi tạo type
                    component.Type = 4;
                    component.Operator = el.additional.operator;
                    component.ConstantTypeVM = {
                        Value: Number(el.text)
                    };
                }
                else {
                    component.Type = el.additional.Type;
                    component.Operator = el.additional.operator;
                    if (el.additional.Id != null) {
                        component.FDTypeVM = {
                            Id: el.additional.Id,
                            Value: Number(el.additional.Name)
                        };
                        if (isNaN(component.FDTypeVM.Value))
                            component.FDTypeVM.Value = 0;
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
            // // call api for fReview
            // this.fReview.push({ Name: "test", Expression: "test = testExp" });
            // this.fReview.push({ Name: "test2", Expression: "test2 = testExp2" });
            console.log(this.formulaToCreate);
            this.cache = this.formulaToCreate;
            this.api.getFieldsCheckFormula(this.formulaToCreate).subscribe(res => {
                this.listField = res;
                this.formulaToCreate.FieldsandValues = this.listField;
                this.listResultTest = [];
                if (stepper != null) {
                    this.goForward(stepper);
                }
                this.formulaToCreate.FieldsandValuesTest = [];
                this.formulaToCreate.FieldsandValuesTest.push(this.listField);
                // call api for fReview
                this.api.showFormula(this.formulaToCreate).subscribe(res => {
                    this.fReview = [];
                    // this.fReview.push(this.documentToCreate.FormulaReview);
                    for (let i = res[0].length - 1; i >= 0; i--) {
                        const el = res[0][i];
                        this.fReview.push(el);
                    }
                    // this.fReview = res[0];
                    console.log(this.fReview);
                }, err => {
                });
            }, err => {
                // alert("Không lấy được đầu lương từ máy chủ");
                alert(err.error);
            });
        }
        else {
            alert("Không được bỏ trống công thức");
        }
    }
    checkFormula() {
        this.formulaToCreate.FieldsandValuesTest = this.listTest;
        console.log(this.formulaToCreate);
        this.api.checkFormula(this.formulaToCreate).subscribe(res => {
            this.listResultTest = [];
            for (let k = 0; k < res.length; k++) {
                const el = res[k];
                let tempList = [];
                for (let i = 0; i < el.length; i++) {
                    const element = el[i];
                    // console.log(element)
                    if (element.Type == 2 || element.Type == 3) {
                        // số thập phân 2 chữ số cuối
                        element.Value = Math.round((Number(element.Value) + Number.EPSILON) * 100) / 100 + "";
                        tempList.push(element);
                    }
                }
                this.listResultTest.push(tempList);
            }
            console.log(this.listResultTest);
            this.btnCheckState = false;
        }, err => {
        });
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
            .subscribe(res => {
            this.documentAfterCreate = res;
            alert("Tạo quyết định thành công");
            location.href = 'document/detail?documentId=' + JSON.stringify(this.documentAfterCreate.Id);
        }, err => {
            console.log(err);
            if (err.status == 409) {
                this.documentAfterCreate = err.error.documentAfterCreate;
                const initialState = {
                    vm: this,
                    model: err.error,
                };
                this.modalRef = this.modalService.show(ModalActiveDocComponent, {
                    initialState: initialState,
                    ignoreBackdropClick: true,
                    class: "modal-sm"
                });
            }
            else {
                alert("Công thức lương có phần tử rỗng!");
            }
        });
    }
};
__decorate([
    ViewChild('createDocumentFlow', { static: false })
], DocumentCreateComponent.prototype, "createDocumentFlow", void 0);
DocumentCreateComponent = __decorate([
    Component({
        selector: 'app-document-create',
        templateUrl: './document-create.component.html',
        styleUrls: ['./document-create.component.css'],
        providers: [{
                provide: MAT_STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }
            }]
    })
], DocumentCreateComponent);
export { DocumentCreateComponent };
let ModalActiveDocComponent = class ModalActiveDocComponent {
    constructor(fieldModalRef, api) {
        this.fieldModalRef = fieldModalRef;
        this.api = api;
    }
    ngOnInit() {
    }
    // toDocDetail() {
    //   location.href = 'document/detail?document=' + JSON.stringify(this.vm.documentAfterCreate);
    // }
    activeDoc(isChange) {
        this.api.activeDoc(this.model, isChange).subscribe(res => {
            alert("Đổi Quyết Định Hiện Hành Thành Công");
            location.href = 'document/detail?documentId=' + JSON.stringify(this.vm.documentAfterCreate.Id);
        }, err => {
            location.href = 'document/detail?documentId=' + JSON.stringify(this.vm.documentAfterCreate.Id);
        });
    }
};
ModalActiveDocComponent = __decorate([
    Component({
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
], ModalActiveDocComponent);
export { ModalActiveDocComponent };
// export interface DialogData {
//   vm: any;
// }
// @Component({
//   selector: 'app-pass-dialog',
//   template: `<div id="ast-alt-2" class="ast-alert" style="display: block;">
//   <div class="ast-alertContent">
//       <div class="ast-alertHeader ast-theme-green">
//           <h3>Bạn Có Muốn Thay Thế Quyết Định Hiện Tại ?</h3>
//           <span onclick="document.getElementById('ast-alt-2').style.display='none'"
//               (click)="onNoClick()">&times;</span>
//       </div>
//       <div class="ast-alertBody ast-theme-l5">
//           <p>test</p>
//       </div>
//   </div>
// </div>`,
//   // styleUrls: ['./pass-dialog.component.css']
// })
// export class PassDialogComponent implements OnInit {
//   constructor(
//     public dialogRef: MatDialogRef<PassDialogComponent>,
//     @Inject(MAT_DIALOG_DATA) public data: DialogData) {
// }
// ngOnInit(): void {
// }
// onNoClick(): void {
//   this.dialogRef.close();
// }
// }
let ModalFieldComponent = class ModalFieldComponent {
    constructor(fieldModalRef, api) {
        this.fieldModalRef = fieldModalRef;
        this.api = api;
        this.fieldToCreate = new SPCSObject;
    }
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
        this.formulaEL.id = this.fieldToCreate.Name;
        // dòng này để bỏ các prefix trong select2 khiến nó thành component đã đc lưu dù chưa xuống databse
        this.formulaEL.text = this.fieldToCreate.Name;
        this.formulaEL.htmltag.innerHTML = this.fieldToCreate.Name;
        this.formulaEL.additional.Type = 1;
        this.formulaEL.additional.FieldTypeVM = this.fieldToCreate;
        this.fieldModalRef.hide();
        // chưa có phần tử nào có tên ... trong formulacomponents thì mới push
        if (this.vm.formulaComponents.filter(a => a.id == this.fieldToCreate.Name).length == 0) {
            this.vm.formulaComponents.push({
                id: this.fieldToCreate.Name,
                text: this.fieldToCreate.Name,
                additional: this.formulaEL.additional,
            });
        }
    }
};
ModalFieldComponent = __decorate([
    Component({
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
    <!--
    <div class="form-group">
        <label for="input-3">Kiểu dữ liệu</label>
        <select class="form-control" id="input-3" [(ngModel)]="fieldToCreate.DataType" >
            <option value="number">Số</option>
            <option value="string">Chuỗi</option>
        </select>
    </div>
    -->
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
], ModalFieldComponent);
export { ModalFieldComponent };
let ModalRefTableComponent = class ModalRefTableComponent {
    constructor(refTableModalRef, modalService, api, util) {
        this.refTableModalRef = refTableModalRef;
        this.modalService = modalService;
        this.api = api;
        this.util = util;
        this.refTableDetail = new SPCSObject;
        this.refTableToCreate = new SPCSObject;
    }
    ngOnInit() {
        this.formulaComponents = JSON.parse(JSON.stringify(this.vm.formulaComponents));
        for (let i = 0; i < this.formulaComponents.length; i++) {
            const el = this.formulaComponents[i];
            if (this.util.isOperator(el.text[0])) {
                el.text = el.text.substr(1);
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
    createRefTable() {
        console.log(this.refTableToCreate);
        // this.api.createReferenceTable(this.refTableToCreate)
        //   .subscribe(
        //     res => {
        //       // this.vm.getAllFormulaElements();
        //       this.formulaEL.additional = { Id: res.Id, Type: 2, Name: res.Name };
        //       alert("Tạo bảng tra thành công");
        //       this.refTableModalRef.hide();
        //     },
        //     err => {
        //       console.log(err);
        //     }
        //   );
        this.formulaEL.id = this.refTableToCreate.Name;
        // this.formulaEL.text = this.refTableToCreate.Name;
        this.formulaEL.htmltag.innerHTML = this.refTableToCreate.Name;
        this.formulaEL.additional.Type = 2;
        this.formulaEL.additional.RefTableTypeVM = this.refTableToCreate;
        this.vm.formulaComponents.push({
            id: this.refTableToCreate.Name,
            text: this.refTableToCreate.Name,
            additional: this.formulaEL.additional,
        });
        this.refTableModalRef.hide();
    }
    openCreatePairModal(createPair) {
        let config = {
            class: "modal-sm"
        };
        this.tempModalRef = this.modalService.show(createPair, config);
    }
    addRefTableDetail() {
        console.log(this.refTableDetail);
        this.refTableToCreate.ReferenceTableDetailCreateVMs.push(this.refTableDetail);
        this.refTableDetail = {
            Key: "",
            Value: "",
        };
    }
    deleteRefTableDetail(index) {
        this.refTableToCreate.ReferenceTableDetailCreateVMs.splice(index, 1);
    }
    setRefTableSource(value) {
        // console.log(value);
        var temp = value.F("-");
        this.refTableToCreate.SourceType = Number(temp[2]);
        this.refTableToCreate.SourceValue = Number(temp[0]);
        console.log(this.refTableToCreate);
    }
    setRefTableReturnType(value) {
        this.refTableToCreate.ReturnType = value;
    }
};
ModalRefTableComponent = __decorate([
    Component({
        selector: 'modal-refTable',
        template: `

          <div class="modal-header" style="background-color: rgba(255,255,255,.2);">
              <h5 class="modal-title">Tạo Bảng Tra</h5>
              <!-- <button type="button" class="close" (click)="refTableModalRef.hide()" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
              </button> -->
          </div>
          <div class="modal-body" style="float: left; width: 100%; background-color: rgba(255,255,255,.2);">
              <div style="float:left; width: 36%;">
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
                          style="margin-left: 10px; width: 200px;">
                          <ng-container *ngFor="let cc of formulaComponents; index as indexId">
                              <option value="{{cc.id}}">{{cc.text}}</option>
                          </ng-container>
                      </select>
                  </div>
                  <!-- <div class="form-group">
                      <label>Kiểu trả về</label>
                      <select #rt (change)="setRefTableReturnType(rt.value)" class="form-control"
                          style="margin-left: 10px; width: 200px;">
                          <option value="number">Số</option>
                          <option value="string">Chuỗi</option>
                      </select>
                  </div> -->
              </div>
              <div style="float:left; width: 60%; margin-left:4%;">
                  <p>Bảng tham chiếu
                      <i (click)="openCreatePairModal(createPair)" class="fa fa-plus-circle fa-lg"
                          style="color: white;"></i>
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
                                  <td>
                                      <i class="fa fa-trash fa-2x" style="color: red;"
                                          (click)="deleteRefTableDetail(indexId)"></i>
                                  </td>
                              </tr>
                          </ng-container>
                      </ng-container>
                      <ng-container *ngIf="refTableToCreate.ReferenceTableDetailCreateVMs?.length == 0">
                          <td colspan="3">Chưa có dữ liệu</td>
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
], ModalRefTableComponent);
export { ModalRefTableComponent };
//# sourceMappingURL=document-create.component.js.map