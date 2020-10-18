import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
let NotifyService = class NotifyService {
    constructor(db) {
        this.db = db;
        this.dbPath = '/admin';
        this.notifiesRef = this.db.list(this.dbPath);
    }
    updateNotify(key, value) {
        this.notifiesRef.update(key, value);
        return;
    }
    getNotifiesList() {
        this.notifiesRef = this.db.list(this.dbPath);
        return this.notifiesRef;
    }
};
NotifyService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], NotifyService);
export { NotifyService };
//# sourceMappingURL=notify.service.js.map