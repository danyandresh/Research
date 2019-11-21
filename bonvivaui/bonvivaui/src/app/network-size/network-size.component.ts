import { Component, OnInit } from '@angular/core';
import { UsersessionService } from '../usersession.service'

@Component({
  selector: 'app-network-size',
  templateUrl: './network-size.component.html',
  styleUrls: ['./network-size.component.scss']
})
export class NetworkSizeComponent implements OnInit {

  constructor(private userSession: UsersessionService) { }

  ngOnInit() {
    this.userSession.getusers().then(data => {
      data
        console.log(data);
        this._size = data.length;
      
    });

  }

  _size = 0;
}
