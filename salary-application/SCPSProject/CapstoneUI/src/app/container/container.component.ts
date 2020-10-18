import { Component, OnInit, Input, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/api.service';
import { UtilityService } from 'src/app/util.service';
import { AngularFireDatabase } from '@angular/fire/database';
import { Notify } from '../class/notification/notify';
import { NotifyService } from '../class/notification/notify.service';

@Component({
  selector: 'app-container',
  templateUrl: './container.component.html',
  styleUrls: ['./container.component.css']
})
export class ContainerComponent implements OnInit {

  @Input() h1_title: string;
  @Input() notify: Notify;

  newName: any;
  oldPassword: any;
  newPassword: any;
  repeatPassword: any;

  notis: Notify[] = [];
  newNotis = 0;

  user: any;

  searchCollapse = false;

  constructor(private notifyService: NotifyService, private db: AngularFireDatabase, public router: Router, public api: ApiService, public util: UtilityService) { }

  ngOnInit() {

    if (localStorage.getItem('user') == null || localStorage.getItem('token') == null) {
      location.href = '';
    }

    this.user = JSON.parse(localStorage.getItem('user'));

    // init notify firebase
    this.db.list(`/${this.user.RoleName}`).snapshotChanges().subscribe(res => {
      this.newNotis = 0;
      this.notis = [];
      for (let i = res.length - 1; i >= 0; i--) {
        const item = res[i];
        let a = item.payload.toJSON();

        a['key'] = item.key;
        let noti = a as Notify;
        if (this.notis.length < 6) {
          this.notis.push(noti);
          if (noti.isReading == false) {
            this.newNotis++;
          }
        }
        
      }
      console.log(this.notis);

      for (let i = 0; i < this.notis.length; i++) {
        const el = this.notis[i];

        switch (el.status) {
          case 1:
            el.message = `Bạn nhận được phiếu lương tháng ` + el.month + `/` + el.year;
            break;
          case 2:
            el.message = `Bảng lương tháng ` + el.month + `/` + el.year + ` vừa được tạo`;
            break;
          case 3:
            el.message = `Bảng lương tháng ` + el.month + `/` + el.year + ` chưa được cập nhật dữ liệu, cần nhập ngay`;
            break;
          case 4:
            el.message = `Bảng lương tháng ` + el.month + `/` + el.year + ` đã được gửi đi`;
          case 5:
            el.message = `Quyết định ` + el.docCode + ` đã được áp dụng`;
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

  updateActive(noti: Notify) {
    this.notifyService
      .updateNotify(noti.key, { isReading: true })
      .catch(err => console.log(err));
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    location.href = '';
  }

  clearData() {
    this.newName = "";
    this.oldPassword = "";
    this.newPassword = "";
    this.repeatPassword = "";
  }

}
