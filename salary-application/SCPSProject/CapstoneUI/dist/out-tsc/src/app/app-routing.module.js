import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormulaComponent } from './webapp/formula/formula.component';
import { DocumentComponent } from './webapp/document/document.component';
import { PayrollComponent } from './webapp/payroll/payroll.component';
import { PayslipComponent } from './webapp/search/payslip.component';
import { DocumentDetailComponent } from './webapp/document-detail/document-detail.component';
import { ImportMaterialComponent } from './webapp/import-material/import-material.component';
import { LoginComponent } from './webapp/login/login.component';
import { TestpageComponent } from './webapp/testpage/testpage.component';
import { PayslipTemplateComponent } from './webapp/payslip-template/payslip-template.component';
import { ManagePayslipComponent } from './webapp/manage-payslip/manage-payslip.component';
import { DocumentCreateComponent } from './webapp/document/document-create/document-create.component';
//import { PayrollDetailComponent } from './webapp/payroll-detail/payroll-detail.component';
const routes = [
    // { path: 'webadmin', component: WebAdminComponent },
    { path: '', component: LoginComponent },
    { path: 'testpage', component: TestpageComponent },
    { path: 'formula', component: FormulaComponent },
    { path: 'formula/create', component: TestpageComponent },
    { path: 'document', component: DocumentComponent },
    { path: 'document/create', component: DocumentCreateComponent },
    { path: 'document/detail', component: DocumentDetailComponent },
    { path: 'payroll', component: PayrollComponent },
    //{ path: 'payroll/detail', component: PayrollDetailComponent },
    { path: 'payslip', component: PayslipComponent },
    { path: 'importmaterial', component: ImportMaterialComponent },
    { path: 'template', component: ManagePayslipComponent },
    { path: 'payslip-template', component: PayslipTemplateComponent },
];
let AppRoutingModule = class AppRoutingModule {
};
AppRoutingModule = __decorate([
    NgModule({
        imports: [RouterModule.forRoot(routes, {
                anchorScrolling: 'enabled',
            })],
        exports: [RouterModule]
    })
], AppRoutingModule);
export { AppRoutingModule };
//# sourceMappingURL=app-routing.module.js.map