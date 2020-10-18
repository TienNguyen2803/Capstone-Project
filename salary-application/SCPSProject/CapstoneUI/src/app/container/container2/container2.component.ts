import { Component, OnInit, Input, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/api.service';
import { UtilityService } from 'src/app/util.service';
import { AngularFireDatabase } from '@angular/fire/database';
import { Notify } from '../../class/notification/notify';
import { NotifyService } from '../../class/notification/notify.service';

@Component({
  selector: 'app-container2',
  templateUrl: './container2.component.html',
  styleUrls: ['./container2.component.css']
})
export class Container2Component implements OnInit {

  @Input() h1_title: string;
  @Input() UserID: string;
  @Input() Hash: string;
  @Input() notify: Notify;

  newName: any;
  oldPassword: any;
  newPassword: any;
  repeatPassword: any;

  notis: Notify[] = [];
  noti: Notify;

  searchCollapse = false;

  constructor(private notifyService: NotifyService, private db: AngularFireDatabase, public router: Router, public api: ApiService, public util: UtilityService) { }

  ngOnInit() {
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
        this.noti = a as Notify;
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
            el.message = `Chưa tạo bảng lương tháng ` + el.month + ` / ` + el.year;
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

  updateActive(noti: Notify) {
    this.notifyService
      .updateNotify(noti.key, { isReading: true })
      .catch(err => console.log(err));
  }

  logout() {
    window.location.replace('');
  }

  clearData() {
    this.newName = "";
    this.oldPassword = "";
    this.newPassword = "";
    this.repeatPassword = "";
  }

}
