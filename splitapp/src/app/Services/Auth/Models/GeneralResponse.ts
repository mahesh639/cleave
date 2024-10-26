export class GeneralResponse{
    Flag: boolean;
    Message: string;
       
    constructor(Flag: boolean, Message: string){
        this.Flag = Flag;
        this.Message = Message;
    }
}