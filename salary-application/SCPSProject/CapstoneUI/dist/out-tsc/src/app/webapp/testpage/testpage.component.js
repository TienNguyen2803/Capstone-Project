import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
let TestpageComponent = class TestpageComponent {
    constructor(_formBuilder, changeDetection, modalService, validator, api, route, router, util) {
        this._formBuilder = _formBuilder;
        this.changeDetection = changeDetection;
        this.modalService = modalService;
        this.validator = validator;
        this.api = api;
        this.route = route;
        this.router = router;
        this.util = util;
    }
    ngOnInit() {
        this.firstFormGroup = this._formBuilder.group({
            firstCtrl: ['', Validators.required]
        });
        this.secondFormGroup = this._formBuilder.group({
            secondCtrl: ['', Validators.required]
        });
    }
};
TestpageComponent = __decorate([
    Component({
        selector: 'app-testpage',
        templateUrl: './testpage.component.html',
        styleUrls: ['./testpage.component.css']
    })
], TestpageComponent);
export { TestpageComponent };
//# sourceMappingURL=testpage.component.js.map