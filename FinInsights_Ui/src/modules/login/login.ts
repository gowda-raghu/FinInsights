import { Component } from '@angular/core';
import { Restservice } from '../../services/restservice/restservice';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AlertService } from '../../common/alert/alert.service';
import { PathService } from '../../common/services/path.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, RouterOutlet],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  token: any;
  users: any;
  id: Number = -1;
  email: string = '';
  password: string = '';
  regemail: string = '';
  regpassword: string = '';
  rememberMe: boolean = false;
  showPassword: boolean = false;
  loading: boolean = false;
  errorMessage: string = '';
  isRegisterMode: boolean = false;
  isOtpGenerated: boolean = false;
  // Register fields
  otp: any = null;
  name: string = '';
  confirmPassword: string = '';
  constructor(private _restService: Restservice, private pathservice: PathService, private cd: ChangeDetectorRef, private router: Router, private alertService: AlertService) {
    // this._restService.getUsers().subscribe(s => {
    //   this.users = s;
    //   console.log(this.users);

    // });
    let user: any = localStorage.getItem('user');
    user = JSON.parse(user);
    console.log(user)
    if (user) {
      this.rememberMe = true;
      this.email = user.username;
      this.password = user.password;
    }

  }

  ngOninit() {
    this.pathservice.updatePath(this.router.url);
    this.cd.detectChanges();
  }

  getUserById() {
    this._restService.getUserById(this.id).subscribe((s: any) => {
      console.log(s);

      this.users = s;
      this.cd.detectChanges()
    })
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onRegister() {
    this.errorMessage = '';

    if (this.regpassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      this.alertService.show(this.errorMessage);
      return;
    }

    let newUserDetails: any = {
      Username: this.name,
      Email: this.regemail,
      Password: this.regpassword,
      Otp: this.otp
    }

    this._restService.AddUser(newUserDetails).subscribe({
      next: (s: any) => {
        if (s) {
          if (s.isOtpGenerated) {
            this.isOtpGenerated = true;
            this.cd.detectChanges();
          }
          if (s.isRegistered) {
            this.otp = null;
            this.isOtpGenerated = false;
            this.email = this.regemail;
            this.isRegisterMode = false;
            this.rememberMe = true;
            this.Remember();
            this.rememberMe = false;
            this.cd.detectChanges()
            // Call API here
            let currentUser = {
              name: this.name,
              email: this.regemail,
              password: this.regpassword
            }
            console.log(currentUser);
            localStorage.setItem('currentUser', JSON.stringify(currentUser));
            this.alertService.show("User Added Successfully!");
          }
          if (!s.isRegistered && !s.isOtpGenerated) {
            this.alertService.show(s?.message);
            this.cd.detectChanges();

          }
        }
      },
      error: (err: any) => {
        this.alertService.show("Unable to add user.\n" + err.error);
        this.cd.detectChanges();

      }
    })

  }

  onLogin() {
    this.loading = true;
    //this.token = null;
    this.errorMessage = '';
    console.log(this.password);
    this._restService.login(
      {
        "username": this.email,
        "password": this.password
      }).subscribe({
        next: (s: any) => {
          console.log(s);
          this.token = s;
          console.log(this.token)

          sessionStorage.setItem('token', s["token"])
          localStorage.setItem('token', s["token"])

          if (this.token) {
            //alert('Login successful');
            //this.loading = false;
            this.alertService.show("Login SuccessFull!\nToken Valid");

            localStorage.setItem('currentUser', JSON.stringify(s["user"]));

            this.cd.detectChanges();
            this.router.navigate(['/home']).then(res => {
              console.log('navigation result:', res);
              this.pathservice.updatePath(this.router.url);
              this.cd.detectChanges();
            }); console.log("home")

          }
          else {
            this.errorMessage = 'Invalid credentials';
            this.alertService.show(this.errorMessage);
          }

        },
        error: (err: any) => {
          this.alertService.show(err.error);
        }
      })
  };

  Remember() {
    if (this.rememberMe) {
      let user = JSON.stringify({
        username: this.email,
        password: this.password
      });
      console.log(user);
      localStorage.setItem('user', user);
    } else {
      localStorage.removeItem('user');
    }
  }

}

