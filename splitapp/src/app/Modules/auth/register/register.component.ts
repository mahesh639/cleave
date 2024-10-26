import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/Services/Auth/auth.service';
import { LoginDto } from 'src/app/Services/Auth/Models/LoginDto';
import { UserDto } from 'src/app/Services/Auth/Models/UserDto';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  email: string = '';
  password: string = '';
  username: string = '';

  constructor(private authService: AuthService){

  }

  register(registerForm:NgForm) {
    console.log(registerForm.value.email)
    let userDetails = new UserDto(registerForm.value.username, registerForm.value.email, registerForm.value.password, registerForm.value.confirmPassword);
    this.authService.registerUser(userDetails).subscribe(data => {
      console.log(data);
    })
  }
}
