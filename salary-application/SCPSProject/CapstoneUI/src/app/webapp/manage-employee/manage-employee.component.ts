import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { UtilityService } from 'src/app/util.service';

@Component({
  selector: 'app-manage-employee',
  templateUrl: './manage-employee.component.html',
  styleUrls: ['./manage-employee.component.css']
})
export class ManageEmployeeComponent implements OnInit {

  emps = []

  constructor(private api: ApiService, public util: UtilityService) { }

  ngOnInit(): void {
    this.api.getAllManageEmployee().subscribe(
      res => {
        this.emps = res;
      },
      err => {

      }
    )
  }

  setRole(value) {

  }

}
