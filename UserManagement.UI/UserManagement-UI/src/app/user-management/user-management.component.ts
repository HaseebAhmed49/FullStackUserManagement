import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {

  constructor(private userServcice:UserService) { }

  // life cycle event
  ngOnInit(): void {
    this.GetAllUser();
  }

  GetAllUser()
  {
    this.userServcice.GetAllUser().subscribe((data:any)=>{

    })
  }
}
