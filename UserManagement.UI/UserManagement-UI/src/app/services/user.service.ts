import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

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
    return this.httpClient.post(this.baseURL+"/api/User/login-user",body);
  }

  public register(fullname:string,email:string,password:string)
  {
    const body={
      fullName:fullname,
      Email:email,
      Password:password
    }
    return this.httpClient.post(this.baseURL+"/api/User/register-user",body);
  }

  public GetAllUser()
  {
    const headers=new HttpHeaders({
      'Authorization':`Bearer ${JSON.parse(JSON.stringify(localStorage.getItem("userInfo")))?.token}`
    });
    return this.httpClient.get(this.baseURL+"/api/User/get-all-users",{headers:headers});
  }

}
