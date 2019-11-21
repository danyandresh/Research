import { Component, OnInit } from '@angular/core';
import { UsersessionService } from '../usersession.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private userSession: UsersessionService) { }

  ngOnInit() {
  }

  login(username: String) {
    this.userSession.login(username);
  }

}
