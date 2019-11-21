import { Component, OnInit } from '@angular/core';
import { UsersessionService } from '../usersession.service'
import { MatIconRegistry } from '@angular/material/icon';

@Component({
  selector: 'app-network-size',
  templateUrl: './network-size.component.html',
  styleUrls: ['./network-size.component.scss']
})
export class NetworkSizeComponent implements OnInit {

  constructor(private userSession: UsersessionService) { }

  ngOnInit() {

    this.refreshUsers();
  }

  refreshUsers() {
    this.userSession.getusers().then(data => {

      console.log(data);
      this._size = data.length;
      this._users = data.filter(el => {
        return true;
        //console.log(this.userSession._username);
        //return el.username !== this.userSession._username;
      });;
    });

  }

  _size = 0;
  _users = Object[0];
}
