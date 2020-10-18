import { __decorate } from "tslib";
import { Component } from '@angular/core';
let LoginComponent = class LoginComponent {
    constructor(api) {
        this.api = api;
    }
    ngOnInit() {
    }
    login() {
        this.api.login(this.username, this.password).subscribe(res => {
            console.log(res);
            localStorage.setItem('token', res.access_token);
            localStorage.setItem('role', res.role);
            location.href = "document";
        }, err => {
            alert("Tài khoản hoặc mật khẩu chưa hợp lệ!");
        });
    }
};
LoginComponent = __decorate([
    Component({
        selector: 'app-login',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css']
    })
], LoginComponent);
export { LoginComponent };
//# sourceMappingURL=login.component.js.map