export class UserDto{
    UserName: string;
    EmailAddress: string;
    Password: string;
    ConfirmPassword: string;

    constructor(UserName: string, EmailAddress: string, Password: string, ConfirmPassword: string){
        this.UserName = UserName;
        this.EmailAddress = EmailAddress;
        this.Password = Password;
        this.ConfirmPassword = ConfirmPassword;
    }
}