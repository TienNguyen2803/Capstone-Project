import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

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

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public getApiUrl() {
    return apiUrl;
  }

  //#region Import Material

  downloadSalaryFull(listEmployee) {
    const apiUrlSalary = apiUrl + "Payroll/GetExcelTemplate?status=1";
    return this.http
      .post<any>(apiUrlSalary, listEmployee, httpOptions).pipe();
  }

  downloadMonthlyFull(listEmployee, listField) {
    const apiUrlMonthly = apiUrl + "Payroll/GetExcelTemplate?status=2";
    let temp = {
      "listEmp": listEmployee,
      "listField": listField
    }
    return this.http
      .post<any>(apiUrlMonthly, listEmployee, httpOptions).pipe();
  }

  downloadPayroll(id) {
    const apiUrlPayroll = apiUrl + "Payroll/DownloadPayroll?payrollId=" + id;
    return this.http
      .get<any>(apiUrlPayroll, httpOptions).pipe();
  }

  //#endregion

  //#region Login - Logout

  login(code, password) {
    const apiUrlLogin = apiUrl + "Account/login";
    let temp = {
      "code": code,
      "password": password
    }
    return this.http
      .post<any>(apiUrlLogin, temp, httpOptions).pipe();
  }

  //#endregion

  //#region Formula

  getAllFormulas() {
    const apiUrlGetFormula = apiUrl + `Formula/formulas`;
    return this.http
      .get<any>(apiUrlGetFormula, httpOptions).pipe();
  }

  getAllFields() {
    const apiUrlGetField = apiUrl + `Formula/fields`;
    return this.http
      .get<any>(apiUrlGetField, httpOptions).pipe();
  }

  getAllFormulaElements() {
    const apiUrlGetAllFormulaElements = apiUrl + `FormulaDetail/formula-create-elements`;
    return this.http
      .get<any>(apiUrlGetAllFormulaElements, httpOptions).pipe();
  }

  getFieldsCheckFormula(formula) {
    const apiUrlCreateFormula = apiUrl + `Formula/check-formula-field`;
    return this.http
      .post<any>(apiUrlCreateFormula, formula, httpOptions).pipe();
  }

  checkFormula(formula) {
    const apiUrlCreateFormula = apiUrl + `Formula/check-formula`;
    return this.http
      .post<any>(apiUrlCreateFormula, formula, httpOptions).pipe();
  }

  showFormula(formula) {
    const apiUrlCreateFormula = apiUrl + `Formula/show-formula`;
    return this.http
      .post<any>(apiUrlCreateFormula, formula, httpOptions).pipe();
  }

  createFormula(model) {
    const apiUrlCreateFormula = apiUrl + `Formula/formula`;
    return this.http
      .post<any>(apiUrlCreateFormula, model).pipe();
  }

  //#endregion

  //#region ReferenceTable

  getAllReferenceTable() {
    const apiUrlGetAllReferenceTable = apiUrl + `ReferenceTable/referencetables`;
    return this.http
      .get<any>(apiUrlGetAllReferenceTable, httpOptions).pipe();
  }

  createReferenceTable(refTable) {
    const apiUrlCreateReferenceTable = apiUrl + `ReferenceTable`;
    return this.http
      .post<any>(apiUrlCreateReferenceTable, refTable, httpOptions).pipe();
  }

  //#endregion

  //#region Field

  createField(field) {
    const apiUrlCreateField = apiUrl + `Field/field`;
    return this.http
      .post<any>(apiUrlCreateField, field, httpOptions).pipe();
  }

  //#endregion

  //#region Document

  getActiveDocument(m, y) {
    console.log(m, y);
    const apiUrlGetActiveDocument = apiUrl + `Document/active-document?m=${m}&y=${y}`;
    return this.http
      .get<any>(apiUrlGetActiveDocument, httpOptions).pipe();
  }

  getAllDocument() {
    const apiUrlGetAllDocument = apiUrl + `Document/documents`;
    return this.http
      .get<any>(apiUrlGetAllDocument, httpOptions).pipe();
  }

  getDocumentById(Id) {
    const apiUrlGetDocumentByID = apiUrl + `Document/document?id=${Id}`;
    return this.http
      .get<any>(apiUrlGetDocumentByID, httpOptions).pipe();
  }

  getImageURL(documentCode) {
    const apiUrlGetDocumentByID = apiUrl + `Document/ImageDocument?code=${documentCode}`;
    return this.http
      .get<any>(apiUrlGetDocumentByID, httpOptions).pipe();
  }

  createDocument(document) {
    const apiUrlCreateDocument = apiUrl + "Document/document";
    return this.http
      .post<any>(apiUrlCreateDocument, document).pipe();
  }

  updateDocument(document) {
    const apiUrlUpdateDocument = apiUrl + "Document/document";
    return this.http
      .put<any>(apiUrlUpdateDocument, document, httpOptions).pipe();
  }

  //#endregion

  //#region Template
  UploadTemplate(template) {
    const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplate`;
    return this.http
      .post<any>(apiUrlUploadTemplate, template).pipe();
  }
  UploadTemplateV2(template) {
    const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplateV2`;
    return this.http
      .post<any>(apiUrlUploadTemplate, template).pipe();
  }
  UploadTemplateV3(template) {
    const apiUrlUploadTemplate = apiUrl + `Mail/UploadTemplateV3`;
    return this.http
      .post<any>(apiUrlUploadTemplate, template).pipe();
  }
  UploadDocx(Base64) {
    const apiUrlUpload = apiUrl + `Mail/UpLoad`;
    return this.http
      .post<any>(apiUrlUpload, Base64).pipe();
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
  SendPayslip(template: string) {
    const apiUrlSendPayslip = apiUrl + "Mail/SendMailMerge?template=" + template;
    return this.http
      .get<string>(apiUrlSendPayslip).pipe();
  }
  GetDocumentActive() {
    const apiUrlGetDocumentActive = apiUrl + "Document/active-document";
    return this.http
      .get<any>(apiUrlGetDocumentActive).pipe();
  }
  SetStatus(template: string, code: string) {
    const apiUrlSetStatus = apiUrl + "Mail/SetStatus?fileName=" + template + "&code=" + code;
    return this.http
      .get<string>(apiUrlSetStatus).pipe();
  }
  IsDocumentFormularId(code: string) {
    const apiUrlIsDocumentFormularId = apiUrl + "Mail/PayrollDocument?code=" + code;
    return this.http
      .get(apiUrlIsDocumentFormularId, {
        responseType: 'text'
      }).pipe();
  }
  GetAllTemplate() {
    const apiUrlGetAllTemplate = apiUrl + "Mail/PaysplitTemplate";
    return this.http
      .get<any>(apiUrlGetAllTemplate).pipe();
  }
  TemplateDefault(template: string) {
    const apiUrlSendPayslip = apiUrl + "Mail/TemplateDefault?documentName=" + template;
    return this.http
      .get(apiUrlSendPayslip, {
        responseType: 'text'
      }).pipe();
  }
  GetHtmlString(filename: string) {
    const apiUrlGetHtmlString = apiUrl + "Mail/StringHtml?fileName=" + filename;
    return this.http
      .get(apiUrlGetHtmlString, {
        responseType: 'text'
      }).pipe();
  }
  ViewDataDemo(code: string, filename: string) {

    const apiUrlGetHtmlString = apiUrl + "Mail/Data-demo?Code=" + code + "&template=" + filename;
    console.log(apiUrlGetHtmlString)
    return this.http
      .get(apiUrlGetHtmlString, {
        responseType: 'text'
      }).pipe();
  }
  GetListNameDocument() {
    const apiUrlGetListNameDocument = apiUrl + "Mail/ListNameDocument";
    return this.http
      .get<string[]>(apiUrlGetListNameDocument).pipe();
  }
  GetListFields(code) {
    const apiUrlGetListFields = apiUrl + "Mail/ListFieldsMerge?code=" + code;
    return this.http
      .get<string[]>(apiUrlGetListFields).pipe();
  }
  GetListTemplate(DocumentName) {
    const apiUrlGetListTemplate = apiUrl + `Mail/ListTempalte?DocumentName=${DocumentName}`;
    return this.http
      .get<string[]>(apiUrlGetListTemplate).pipe();
  }
  //#endregion Template

  //#region Payroll

  getAllPayroll() {
    const apiUrlGetAllPayroll = apiUrl + `Payroll/payrolls`;
    return this.http
      .get<any>(apiUrlGetAllPayroll, httpOptions).pipe();
  }

  activeDoc(model, isChanged) {
    const apiUrlActiveDoc = apiUrl + `Payroll/payroll?m=${model.month}&y=${model.year}&docId=${model.documentAfterCreate.Id}&isChanged=${isChanged}`;
    return this.http
      .put<any>(apiUrlActiveDoc, httpOptions).pipe();
  }

  publishPayroll(id) {
    const apiUrlCreatePayroll = apiUrl + `Payroll/publish-payroll?id=${id}`;
    return this.http
      .put<any>(apiUrlCreatePayroll, httpOptions).pipe();
  }

  sendEmail() {
    const apiUrlGetAllPayroll = apiUrl + `Schedule/send-email-sch`;
    return this.http
      .get<any>(apiUrlGetAllPayroll, httpOptions).pipe();
  }

  // createPayroll(payroll) {
  //   const apiUrlCreatePayroll = apiUrl + `Payroll/payroll`;
  //   return this.http
  //     .post<any>(apiUrlCreatePayroll, payroll, httpOptions).pipe();
  // }
  //#endregion

  //#region Employee
  getPayslipEmployee(month, year, code) {
    const apiUrlGetPayslipEmployee = apiUrl + `Employee/payslip-employee?employeeId=${code}&month=${month}&year=${year}
    `;
    return this.http
      .get<any>(apiUrlGetPayslipEmployee, httpOptions).pipe();
  }

  getAllEmployee() {
    const apiUrlGetAllEmployee = apiUrl + `Employee/employees`;
    return this.http
      .get<any>(apiUrlGetAllEmployee, httpOptions).pipe();
  }

  getAllManageEmployee() {
    const apiUrlGetAllEmployee = apiUrl + `Employee/manage-employees`;
    return this.http
      .get<any>(apiUrlGetAllEmployee, httpOptions).pipe();
  }

  //#endregion

  //#region Payslip

  getPayslipByPayrollIdAndEmployeeId(employeeId, payrollId) {
    const apiUrlGetAllEmployee = apiUrl + `Payslip/payslip?employeeId=${employeeId}&payrollId=${payrollId}`;
    return this.http
      .get<any>(apiUrlGetAllEmployee, httpOptions).pipe();
  }

  //#endregion

  //#region SalaryComponent

  createSalaryComponents(salaryComponents) {
    const apiUrlCreate = apiUrl + `SalaryComponent/salarycomponents`;
    return this.http
      .post<any>(apiUrlCreate, salaryComponents, httpOptions).pipe();
  }

  createMonthlySalaryComponents(monthlySalaryComponents) {
    const apiUrlCreate = apiUrl + `SalaryComponent/monthlysalarycomponents`;
    return this.http
      .post<any>(apiUrlCreate, monthlySalaryComponents, httpOptions).pipe();
  }

  createPayrollComponents(payrollcomponents) {
    const apiUrlCreate = apiUrl + `SalaryComponent/payrollcomponents`;
    return this.http
      .post<any>(apiUrlCreate, payrollcomponents, httpOptions).pipe();
  }

  getEmpSalaryComponents() {
    const apiUrlCreate = apiUrl + `Payroll/EmpSalaryComponents`;
    return this.http
      .get<any>(apiUrlCreate).pipe();
  }

  //#endregion
}
