import { Injectable, OnInit } from '@angular/core';
import { AngularFireDatabase, AngularFireList, AngularFireObject } from '@angular/fire/database';
import { Notify } from './notify';


@Injectable({
  providedIn: 'root'
})
export class NotifyService implements OnInit {

  private dbPath = '/chief';

  notifiesRef: any;
  constructor(private db: AngularFireDatabase) {

    this.notifiesRef = this.db.list(this.dbPath);
  }

  ngOnInit() {

    var user = JSON.parse(localStorage.getItem('user'));
    this.dbPath = user.RoleName;
  }

  updateNotify(key: string, value: any): Promise<void> {
    this.notifiesRef.update(key, value)
    return;
  }

  getNotifiesList(): any {
    this.notifiesRef = this.db.list(this.dbPath);
    return this.notifiesRef;
  }

}