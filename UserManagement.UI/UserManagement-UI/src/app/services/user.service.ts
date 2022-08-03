import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../models/responseModel';
import { map } from 'rxjs/operators';
import { ResponseCode } from '../enums/responseCode';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class UserService {

private readonly baseURL:string=environment.baseApiUrl;

  constructor(private httpClient:HttpClient) { }

  public login(email:string,password:string)
  {
    const body={
      Email:email,
      Password:password
    }
    return this.httpClient.post<ResponseModel>(this.baseURL+"/api/User/login-user",body);
  }

  public register(fullname:string,email:string,password:string)
  {
    const body={
      fullName:fullname,
      Email:email,
      Password:password
    }
    return this.httpClient.post<ResponseModel>(this.baseURL+"/api/User/register-user",body);
  }

  public GetAllUser()
  {
    let userList=new Array<User>();
    const headers=new HttpHeaders({
      'Authorization':`Bearer ${JSON.parse(JSON.stringify(localStorage.getItem("userInfo")))?.token}`
    });
    return this.httpClient.get<ResponseModel>(this.baseURL+"/api/User/get-all-users",{headers:headers}).pipe(map(res=>{
      if(res.ResponseCode==ResponseCode.OK)
      {
        let userList=new Array<User>();
        if(res.DataSet)
        {
          res.DataSet.map((x:User)=>{
            userList.push(new User(x.fullName,x.email,x.userName))
          })
        }
        return userList;
      }
      return userList;
    }
    
    ));
  }

}
