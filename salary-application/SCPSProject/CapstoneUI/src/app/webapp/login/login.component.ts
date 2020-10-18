import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  username: any;
  password: any;

  constructor(private api: ApiService) { }

  ngOnInit(): void {

    if (localStorage.getItem('token') != null && localStorage.getItem('user') != null) {
      var user = JSON.parse(localStorage.getItem('user'));

      if (user.RoleName == "accountant" || user.RoleName == "chief") {
        location.href = "document";
      }
      else if (user.RoleName == "employee") {
        location.href = "payslip-employee";
      }
      else if (user.RoleName == "admin") {
        location.href = "manage-employee";
      }

    }
  }

  login() {
    this.api.login(this.username, this.password).subscribe(
      res => {
        console.log(res);

        localStorage.removeItem('token');
        localStorage.removeItem('user');

        localStorage.setItem('token', res.access_token);
        localStorage.setItem('user', JSON.stringify(res.user));

        let user = res.user;
        if (user.RoleName == "accountant" || user.RoleName == "chief") {
          location.href = "document";
        }
        else if (user.RoleName == "employee") {
          location.href = "payslip-employee";
        }
        else if (user.RoleName == "admin") {
          location.href = "document";
        }

      },
      err => {
        Swal.fire("Cảnh Báo", "Tài khoản hoặc mật khẩu không hợp lệ", "warning");
      }
    )
  }

}
