import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/Services/Auth/auth.service';
import { LoginDto } from 'src/app/Services/Auth/Models/LoginDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  email: string = '';
  password: string = '';

  constructor(private authService: AuthService){

  }

  login(loginForm:NgForm) {
    console.log(loginForm.value.email)
    let loginDetails = new LoginDto(loginForm.value.email, loginForm.value.password);
    this.authService.loginUser(loginDetails).subscribe(data => {
      console.log(data);
    })
  }

}

