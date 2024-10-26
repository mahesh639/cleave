import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDto } from './Models/UserDto';
import { GeneralResponse } from './Models/GeneralResponse';
import { LoginDto } from './Models/LoginDto';
import { LoginResponse } from './Models/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) { }

  private identityServiceBaseUrl = 'https://localhost:7215';
  private registerApi = this.identityServiceBaseUrl+'/register';
  private loginApi = this.identityServiceBaseUrl+'/login';

  
  registerUser(userDetails: UserDto){
    return this.httpClient.post<LoginResponse>(this.registerApi,userDetails);
  }

  loginUser(loginDto: LoginDto){
    return this.httpClient.post<GeneralResponse>(this.loginApi,loginDto);
  }

}
