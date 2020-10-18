import { __decorate } from "tslib";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
//Tự import thư viện bằng tay
import { AngularFireDatabaseModule } from '@angular/fire/database';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { AngularFireModule } from '@angular/fire';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material-module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelect2Module } from 'ng-select2';
// component
import { ContainerComponent } from './container/container.component';
import { FormulaComponent } from './webapp/formula/formula.component';
import { PassDialogComponent } from './components/pass-dialog/pass-dialog.component';
import { FailDialogComponent } from './components/fail-dialog/fail-dialog.component';
import { WarnDialogComponent } from './components/warn-dialog/warn-dialog.component';
import { DocumentComponent } from './webapp/document/document.component';
import { LoginComponent } from './webapp/login/login.component';
import { PayrollComponent, ModalMonthlyComponent } from './webapp/payroll/payroll.component';
import { PayslipComponent } from './webapp/search/payslip.component';
import { DocumentDetailComponent } from './webapp/document-detail/document-detail.component';
import { ImportMaterialComponent } from './webapp/import-material/import-material.component';
import { DocumentEditorModule } from '@txtextcontrol/tx-ng-document-editor';
import { TestpageComponent } from './webapp/testpage/testpage.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { AngularFirestoreModule } from 'angularfire2/firestore';
import { PayslipTemplateComponent } from './webapp/payslip-template/payslip-template.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ManagePayslipComponent } from './webapp/manage-payslip/manage-payslip.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { DocumentCreateComponent, ModalFieldComponent, ModalRefTableComponent } from './webapp/document/document-create/document-create.component';
// import { FormulaCreateComponent } from './webapp/document/formula-create/formula-create.component';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Container1Component } from './container/container1/container1.component';
import { Container2Component } from './container/container2/container2.component';
import { Container3Component } from './container/container3/container3.component';
//import { PayrollDetailComponent } from './webapp/payroll-detail/payroll-detail.component';
let AppModule = class AppModule {
};
AppModule = __decorate([
    NgModule({
        declarations: [
            AppComponent,
            ContainerComponent,
            FormulaComponent,
            PassDialogComponent,
            FailDialogComponent,
            WarnDialogComponent,
            DocumentComponent,
            LoginComponent,
            PayrollComponent,
            PayslipComponent,
            DocumentDetailComponent,
            ImportMaterialComponent,
            TestpageComponent,
            ModalFieldComponent,
            ModalRefTableComponent,
            PayslipTemplateComponent,
            ManagePayslipComponent,
            DocumentCreateComponent,
            ModalMonthlyComponent,
            Container1Component,
            Container2Component,
            Container3Component,
        ],
        imports: [
            SweetAlert2Module.forRoot(),
            CKEditorModule,
            BrowserModule,
            AppRoutingModule,
            BrowserAnimationsModule,
            FormsModule,
            HttpClientModule,
            MaterialModule,
            AngularFireDatabaseModule,
            AngularFireAuthModule,
            AngularFireModule.initializeApp(environment.firebase),
            DocumentEditorModule,
            ReactiveFormsModule,
            NgbModule,
            MatDatepickerModule,
            AngularFirestoreModule,
            BsDatepickerModule.forRoot(),
            ModalModule.forRoot(),
            NgSelect2Module,
            CollapseModule.forRoot(),
            TabsModule.forRoot(),
        ],
        entryComponents: [
            PassDialogComponent,
            FailDialogComponent,
            WarnDialogComponent,
        ],
        providers: [],
        bootstrap: [AppComponent]
    })
], AppModule);
export { AppModule };
//# sourceMappingURL=app.module.js.map