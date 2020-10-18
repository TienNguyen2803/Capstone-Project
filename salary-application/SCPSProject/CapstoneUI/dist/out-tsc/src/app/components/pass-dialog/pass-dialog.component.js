import { __decorate, __param } from "tslib";
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
let PassDialogComponent = class PassDialogComponent {
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
PassDialogComponent = __decorate([
    Component({
        selector: 'app-pass-dialog',
        templateUrl: './pass-dialog.component.html',
        styleUrls: ['./pass-dialog.component.css']
    }),
    __param(1, Inject(MAT_DIALOG_DATA))
], PassDialogComponent);
export { PassDialogComponent };
//# sourceMappingURL=pass-dialog.component.js.map