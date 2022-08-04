import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ResponseCode } from '../enums/responseCode';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

public loginForm = this.formBuilder.group({
  email:['',[Validators.email,Validators.required]],
  password:['',Validators.required]
})

  constructor(private formBuilder: FormBuilder,private userService:UserService,private router:Router) { }

  ngOnInit(): void {
  }


  onSubmit()
  {
    console.log("Login");
    let email=this.loginForm.controls["email"].value;
    let password=this.loginForm.controls["password"].value;
    // ! will ignore 
    this.userService.login(email!,password!).subscribe({
      next: (data:any)=>{      
      if(data.ResponseCode==ResponseCode.OK)
      {
        localStorage.setItem("userInfo",JSON.stringify(data.DataSet.token));
        this.router.navigate(["/usermanagement"]);
      }      
      console.log("response",data);
    }});
  
}
}
