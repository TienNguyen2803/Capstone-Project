import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { PassDialogComponent } from './components/pass-dialog/pass-dialog.component';
import { FailDialogComponent } from './components/fail-dialog/fail-dialog.component';
import { WarnDialogComponent } from './components/warn-dialog/warn-dialog.component';
let UtilityService = class UtilityService {
    constructor(dialog) {
        this.dialog = dialog;
    }
    formatDateAPI(dateNeedToFormat) {
        var temp = new Date(dateNeedToFormat);
        const dd = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(temp);
        const mm = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(temp);
        const yyyy = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(temp);
        var result = "";
        result += yyyy + "-";
        result += mm + "-";
        result += dd;
        return result;
    }
    formatDateAPI2(dateNeedToFormat) {
        var temp = dateNeedToFormat;
        temp = temp.split(' / ')[2] + "-" + temp.split(' / ')[1] + "-" + temp.split(' / ')[0];
        console.log(temp);
        return temp;
    }
    formatDate(dateNeedToFormat) {
        var temp = new Date(dateNeedToFormat);
        const dd = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(temp);
        const mm = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(temp);
        const yyyy = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(temp);
        var result = "";
        result += dd + " / ";
        result += mm + " / ";
        result += yyyy;
        return result;
    }
    formatDateTime(dateNeedToFormat) {
        var result = "";
        var temp = new Date(dateNeedToFormat);
        var hh = temp.getHours();
        var MM = temp.getMinutes();
        var ss = temp.getSeconds();
        if (hh < 10) {
            result += '0' + hh + ':';
        }
        else {
            result += hh + ':';
        }
        if (MM < 10) {
            result += '0' + MM + ':';
        }
        else {
            result += MM + ':';
        }
        if (ss < 10) {
            result += '0' + ss + ' - ';
        }
        else {
            result += ss + ' - ';
        }
        result += this.formatDate(dateNeedToFormat);
        return result;
    }
    nonAccentVietnamese(str) {
        console.log(str);
        if (str != null && str != "") {
            str = str.toLowerCase();
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");
            // Some system encode vietnamese combining accent as individual utf-8 characters
            str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // Huyền sắc hỏi ngã nặng 
            str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // Â, Ê, Ă, Ơ, Ư
            return str;
        }
        else {
            return "~=~=~";
        }
    }
    showPassDialog(err_code, alt_value) {
        const dialogRef = this.dialog.open(PassDialogComponent, {
            width: '0px',
            height: '0px',
            panelClass: 'custom-dialog-container',
            data: { err_code: err_code, alt_value: alt_value }
        });
        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }
    showFailDialog(err_code, alt_value) {
        const dialogRef = this.dialog.open(FailDialogComponent, {
            width: '0px',
            height: '0px',
            panelClass: 'custom-dialog-container',
            data: { err_code: err_code, alt_value: alt_value }
        });
        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }
    showWarnDialog(err_code, alt_value) {
        const dialogRef = this.dialog.open(WarnDialogComponent, {
            width: '0px',
            height: '0px',
            panelClass: 'custom-dialog-container',
            data: { err_code: err_code, alt_value: alt_value }
        });
        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
        });
    }
    formulaShortType(key) {
        switch (key) {
            case 1:
                return "T";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "H";
            default:
                break;
        }
    }
    formulaLongType(key) {
        switch (key) {
            case 1:
                return "Thành phần";
            case 2:
                return "Bảng lương";
            case 3:
                return "Công thức";
            case 4:
                return "Hằng số";
            default:
                break;
        }
    }
    commaForBigNum(num) {
        num += "";
        // check field 
        if (Number(num) == null) {
            // with string return
            return num;
        }
        else {
            console.log(num);
            // with int return
            let result = "";
            let i = num.length - 3;
            while (true) {
                if (i < 1)
                    break;
                result = "," + num.substring(i, i + 3) + result;
                num = num.substring(0, i);
                i = i - 3;
            }
            result = num + result;
            return result;
        }
    }
    commaForBigNum2(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    isOperator(str) {
        switch (str) {
            case "+":
                return true;
            case "-":
                return true;
            case "*":
                return true;
            case "/":
                return true;
            default:
                return false;
        }
    }
    numOperator(str) {
        switch (str) {
            case "+":
                return 1;
            case "-":
                return 2;
            case "*":
                return 3;
            case "/":
                return 4;
            default:
                return false;
        }
    }
    charOperator(num) {
        switch (num) {
            case 1:
                return "+";
            case 2:
                return "-";
            case 3:
                return "*";
            case 4:
                return "/";
            default:
                return false;
        }
    }
    equalsIgnoreCase(a, b) {
        return typeof a === 'string' && typeof b === 'string'
            ? a.localeCompare(b, undefined, { sensitivity: 'accent' }) === 0
            : a === b;
    }
};
UtilityService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], UtilityService);
export { UtilityService };
//# sourceMappingURL=util.service.js.map