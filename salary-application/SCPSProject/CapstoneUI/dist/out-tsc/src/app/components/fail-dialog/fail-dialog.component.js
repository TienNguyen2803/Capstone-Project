import { __decorate, __param } from "tslib";
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
let FailDialogComponent = class FailDialogComponent {
    constructor(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    ngOnInit() {
    }
    onNoClick() {
        this.dialogRef.close();
    }
};
FailDialogComponent = __decorate([
    Component({
        selector: 'app-fail-dialog',
        templateUrl: './fail-dialog.component.html',
        styleUrls: ['./fail-dialog.component.css']
    }),
    __param(1, Inject(MAT_DIALOG_DATA))
], FailDialogComponent);
export { FailDialogComponent };
//# sourceMappingURL=fail-dialog.component.js.map