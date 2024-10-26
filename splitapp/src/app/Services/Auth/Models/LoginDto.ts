export class LoginDto{
    Email: string;
    Password: String;

    constructor(Email: string, Password: string){
        this.Email = Email;
        this.Password = Password;
    }
}