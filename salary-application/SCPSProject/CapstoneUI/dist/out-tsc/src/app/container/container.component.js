import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
let ContainerComponent = class ContainerComponent {
    constructor(notifyService, db, router, api, util) {
        this.notifyService = notifyService;
        this.db = db;
        this.router = router;
        this.api = api;
        this.util = util;
        this.notis = [];
        this.searchCollapse = false;
    }
    ngOnInit() {
        if (localStorage.getItem('role') == null || localStorage.getItem('token') == null) {
            location.href = '';
        }
        this.role = localStorage.getItem('role');
        // init notify firebase
        this.db.list('/admin').snapshotChanges().subscribe(res => {
            this.notis = [];
            for (let i = 0; i < res.length; i++) {
                const item = res[i];
                let a = item.payload.toJSON();
                // if (this.notis.length != 0) {
                //   for (let i = 0; i < this.notis.length; i++) {
                //     let el = this.notis[i];
                //     if (el.key == item.key) {
                //       el.splice(i, 1);
                //     }
                //   }
                // }
                // a['key'] = item.key;
                // this.noti = a as Notify;
                // this.notis.push(this.noti);
                // this.notis = this.notis.reverse();
                a['key'] = item.key;
                this.noti = a;
                if (!this.noti.isReading) {
                    this.notis.push(this.noti);
                }
                this.notis = this.notis.reverse();
            }
            console.log(this.notis);
            for (let i = 0; i < this.notis.length; i++) {
                const el = this.notis[i];
                switch (el.status) {
                    case 1:
                        el.message = `Bảng lương tháng ` + el.month + ` / ` + el.year + ` vừa được tạo`;
                        break;
                    case 2:
                        el.message = `Chưa tính lương tháng ` + el.month + `, hãy publish bảng lương tháng ` + el.month + `/` + el.year;
                        break;
                    default:
                        break;
                }
            }
        });
        // init menu
        if (this.h1_title == "m1" || this.h1_title == "m3") {
            this.searchCollapse = true;
        }
    }
    updateActive(noti) {
        this.notifyService
            .updateNotify(noti.key, { isReading: true })
            .catch(err => console.log(err));
    }
    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        location.href = '';
    }
    clearData() {
        this.newName = "";
        this.oldPassword = "";
        this.newPassword = "";
        this.repeatPassword = "";
    }
};
__decorate([
    Input()
], ContainerComponent.prototype, "h1_title", void 0);
__decorate([
    Input()
], ContainerComponent.prototype, "UserID", void 0);
__decorate([
    Input()
], ContainerComponent.prototype, "Hash", void 0);
__decorate([
    Input()
], ContainerComponent.prototype, "notify", void 0);
ContainerComponent = __decorate([
    Component({
        selector: 'app-container',
        templateUrl: './container.component.html',
        styleUrls: ['./container.component.css']
    })
], ContainerComponent);
export { ContainerComponent };
//# sourceMappingURL=container.component.js.map