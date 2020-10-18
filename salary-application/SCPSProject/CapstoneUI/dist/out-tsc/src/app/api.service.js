import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem("token")}`
    }),
};
// local to Azure
// let apiUrl = "http://spcs.azurewebsites.net/api/";
//let apiUrl = "http://capstonettestapp.azurewebsites.net/api/";
let apiUrl = "http://localhost:3911/api/";
// azure to azure or local to local
// let apiUrl = "api/";
let ApiService = class ApiService {
    constructor(http) {
        this.http = http;
    }
    getApiUrl() {
        return apiUrl;
    }
    //#region Import Material
    downloadSalaryFull(listEmployee) {
        const apiUrlSalary = apiUrl + "Payroll/GetExcelTemplate?status=1";
        return this.http
            .post(apiUrlSalary, listEmployee, httpOptions).pipe();
    }
    downloadMonthlyFull(listEmployee, listField) {
        const apiUrlMonthly = apiUrl + "Payroll/GetExcelTemplate?status=2";
        let temp = {
            "listEmp": listEmployee,
            "listField": listField
        };
        return this.http
            .post(apiUrlMonthly, listEmployee, httpOptions).pipe();
    }
    downloadPayroll(id) {
        const apiUrlPayroll = apiUrl + "Payroll/DownloadPayroll?payrollId=" + id;
        return this.http
            .get(apiUrlPayroll, httpOptions).pipe();
    }
    //#endregion
    //#region Login - Logout
    login(code, password) {
        const apiUrlLogin = apiUrl + "Account/login";
        let temp = {
            "code": code,
            "password": password
        };
        return this.http
            .post(apiUrlLogin, temp, httpOptions).pipe();
    }
    //#endregion
    //#region Formula
    getAllFormulas() {
        const apiUrlGetFormula = apiUrl + `Formula/formulas`;
        return this.http
            .get(apiUrlGetFormula, httpOptions).pipe();
    }
    getAllFormulaElements() {
        const apiUrlGetAllFormulaElements = apiUrl + `FormulaDetail/formula-create-elements`;
        return this.http
            .get(apiUrlGetAllFormulaElements, httpOptions).pipe();
    }
    getFieldsCheckFormula(formula) {
        const apiUrlCreateFormula = apiUrl + `Formula/check-formula-field`;
        return this.http
            .post(apiUrlCreateFormula, formula, httpOptions).pipe();
    }
    checkFormula(formula) {
        const apiUrlCreateFormula = apiUrl + `Formula/check-formula`;
        return this.http
            .post(apiUrlCreateFormula, formula, httpOptions).pipe();
    }
    showFormula(formula) {
        const apiUrlCreateFormula = apiUrl + `Formula/show-formula`;
        return this.http
            .post(apiUrlCreateFormula, formula, httpOptions).pipe();
    }
    createFormula(model) {
        const apiUrlCreateFormula = apiUrl + `Formula/formula`;
        return this.http
            .post(apiUrlCreateFormula, model).pipe();
    }
    //#endregion
    //#region ReferenceTable
    getAllReferenceTable() {
        const apiUrlGetAllReferenceTable = apiUrl + `ReferenceTable/referencetables`;
        return this.http
            .get(apiUrlGetAllReferenceTable, httpOptions).pipe();
    }
    createReferenceTable(refTable) {
        const apiUrlCreateReferenceTable = apiUrl + `ReferenceTable`;
        return this.http
            .post(apiUrlCreateReferenceTable, refTable, httpOptions).pipe();
    }
    //#endregion
    //#region Field
    createField(field) {
        const apiUrlCreateField = apiUrl + `Field/field`;
        return this.http
            .post(apiUrlCreateField, field, httpOptions).pipe();
    }
    //#endregion
    //#region Document
    getActiveDocument(m, y) {
        console.log(m, y);
        const apiUrlGetActiveDocument = apiUrl + `Document/active-document?m=${m}&y=${y}`;
        return this.http
            .get(apiUrlGetActiveDocument, httpOptions).pipe();
    }
    getAllDocument() {
        const apiUrlGetAllDocument = apiUrl + `Document/documents`;
        return this.http
            .get(apiUrlGetAllDocument, httpOptions).pipe();
    }
    getDocumentById(Id) {
        const apiUrlGetDocumentByID = apiUrl + `Document/document?id=${Id}`;
        return this.http
            .get(apiUrlGetDocumentByID, httpOptions).pipe();
    }
    getImageURL(documentCode) {
        const apiUrlGetDocumentByID = apiUrl + `Document/ImageDocument?code=${documentCode}`;
        return this.http
            .get(apiUrlGetDocumentByID, httpOptions).pipe();
    }
    createDocument(document) {
        const apiUrlCreateDocument = apiUrl + "Document/document";
        return this.http
            .post(apiUrlCreateDocument, document).pipe();
    }
    updateDocument(document) {
        const apiUrlUpdateDocument = apiUrl + "Document/document";
        return this.http
            .put(apiUrlUpdateDocument, document, httpOptions).pipe();
    }
    //#endregion
    //#region Template
    UploadTemplate(template) {
        const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplate`;
        return this.http
            .post(apiUrlUploadTemplate, template).pipe();
    }
    UploadTemplateV2(template) {
        const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplateV2`;
        return this.http
            .post(apiUrlUploadTemplate, template).pipe();
    }
    UploadTemplateV3(template) {
        const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplateV3`;
        return this.http
            .post(apiUrlUploadTemplate, template).pipe();
    }
    UploadDocx(Base64) {
        const apiUrlUpload = apiUrl + `Mail/UpLoad`;
        return this.http
            .post(apiUrlUpload, Base64).pipe();
    }
    DownLoadField(documentName) {
        const apiUrlDownLoadField = apiUrl + `Mail/Export?code=${documentName}`;
        return this.http.post(apiUrlDownLoadField, documentName, {
            responseType: 'blob'
        }).pipe();
    }
    LoadDocumetTemplate(documentName) {
        const apiUrlLoadDocumetTemplate = apiUrl + `Mail/File?fileName=${documentName}`;
        return this.http.post(apiUrlLoadDocumetTemplate, documentName, {
            responseType: 'blob'
        }).pipe();
    }
    SendPayslip(template) {
        const apiUrlSendPayslip = apiUrl + "Mail/SendMailMerge?template=" + template;
        return this.http
            .get(apiUrlSendPayslip).pipe();
    }
    GetDocumentActive() {
        const apiUrlGetDocumentActive = apiUrl + "Document/active-document";
        return this.http
            .get(apiUrlGetDocumentActive).pipe();
    }
    SetStatus(template, code) {
        const apiUrlSetStatus = apiUrl + "Mail/SetStatus?fileName=" + template + "&code=" + code;
        return this.http
            .get(apiUrlSetStatus).pipe();
    }
    IsDocumentFormularId(code) {
        const apiUrlIsDocumentFormularId = apiUrl + "Mail/PayrollDocument?code=" + code;
        return this.http
            .get(apiUrlIsDocumentFormularId, {
            responseType: 'text'
        }).pipe();
    }
    GetAllTemplate() {
        const apiUrlGetAllTemplate = apiUrl + "Mail/PaysplitTemplate";
        return this.http
            .get(apiUrlGetAllTemplate).pipe();
    }
    TemplateDefault(template) {
        const apiUrlSendPayslip = apiUrl + "Mail/TemplateDefault?documentName=" + template;
        return this.http
            .get(apiUrlSendPayslip, {
            responseType: 'text'
        }).pipe();
    }
    GetHtmlString(filename) {
        const apiUrlGetHtmlString = apiUrl + "Mail/StringHtml?fileName=" + filename;
        return this.http
            .get(apiUrlGetHtmlString, {
            responseType: 'text'
        }).pipe();
    }
    ViewDataDemo(code, filename) {
        const apiUrlGetHtmlString = apiUrl + "Mail/Data-demo?Code=" + code + "&template=" + filename;
        console.log(apiUrlGetHtmlString);
        return this.http
            .get(apiUrlGetHtmlString, {
            responseType: 'text'
        }).pipe();
    }
    GetListNameDocument() {
        const apiUrlGetListNameDocument = apiUrl + "Mail/ListNameDocument";
        return this.http
            .get(apiUrlGetListNameDocument).pipe();
    }
    GetListFields(code) {
        const apiUrlGetListFields = apiUrl + "Mail/ListFieldsMerge?code=" + code;
        return this.http
            .get(apiUrlGetListFields).pipe();
    }
    GetListTemplate(DocumentName) {
        const apiUrlGetListTemplate = apiUrl + `Mail/ListTempalte?DocumentName=${DocumentName}`;
        return this.http
            .get(apiUrlGetListTemplate).pipe();
    }
    //#endregion Template
    //#region Payroll
    getAllPayroll() {
        const apiUrlGetAllPayroll = apiUrl + `Payroll/payrolls`;
        return this.http
            .get(apiUrlGetAllPayroll, httpOptions).pipe();
    }
    activeDoc(model, isChanged) {
        const apiUrlActiveDoc = apiUrl + `Payroll/payroll?m=${model.month}&y=${model.year}&docId=${model.documentAfterCreate.Id}&isChanged=${isChanged}`;
        return this.http
            .put(apiUrlActiveDoc, httpOptions).pipe();
    }
    publishPayroll(id) {
        const apiUrlCreatePayroll = apiUrl + `Payroll/publish-payroll?id=${id}`;
        return this.http
            .put(apiUrlCreatePayroll, httpOptions).pipe();
    }
    sendEmail() {
        const apiUrlGetAllPayroll = apiUrl + `Schedule/send-email-sch`;
        return this.http
            .get(apiUrlGetAllPayroll, httpOptions).pipe();
    }
    // createPayroll(payroll) {
    //   const apiUrlCreatePayroll = apiUrl + `Payroll/payroll`;
    //   return this.http
    //     .post<any>(apiUrlCreatePayroll, payroll, httpOptions).pipe();
    // }
    //#endregion
    //#region Employee
    getAllEmployee() {
        const apiUrlGetAllEmployee = apiUrl + `Employee/employees`;
        return this.http
            .get(apiUrlGetAllEmployee, httpOptions).pipe();
    }
    //#endregion
    //#region Payslip
    getPayslipByPayrollIdAndEmployeeId(employeeId, payrollId) {
        const apiUrlGetAllEmployee = apiUrl + `Payslip/payslip?employeeId=${employeeId}&payrollId=${payrollId}`;
        return this.http
            .get(apiUrlGetAllEmployee, httpOptions).pipe();
    }
    //#endregion
    //#region SalaryComponent
    createSalaryComponents(salaryComponents) {
        const apiUrlCreate = apiUrl + `SalaryComponent/salarycomponents`;
        return this.http
            .post(apiUrlCreate, salaryComponents, httpOptions).pipe();
    }
    createMonthlySalaryComponents(monthlySalaryComponents) {
        const apiUrlCreate = apiUrl + `SalaryComponent/monthlysalarycomponents`;
        return this.http
            .post(apiUrlCreate, monthlySalaryComponents, httpOptions).pipe();
    }
    createPayrollComponents(payrollcomponents) {
        const apiUrlCreate = apiUrl + `SalaryComponent/payrollcomponents`;
        return this.http
            .post(apiUrlCreate, payrollcomponents, httpOptions).pipe();
    }
    getEmpSalaryComponents() {
        const apiUrlCreate = apiUrl + `Payroll/EmpSalaryComponents`;
        return this.http
            .get(apiUrlCreate).pipe();
    }
};
ApiService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], ApiService);
export { ApiService };
//# sourceMappingURL=api.service.js.map