import { Component, OnInit } from '@angular/core';
import { UsersessionService } from '../usersession.service'

@Component({
  selector: 'app-points',
  templateUrl: './points.component.html',
  styleUrls: ['./points.component.scss']
})
export class PointsComponent implements OnInit {

  constructor(private userSession: UsersessionService) { }

  ngOnInit() {
    this.refreshData();
  }

  addPoints(){
    this.userSession.addPoints().then(_=>{
      this.refreshData();
    });
  }

  reset(){
    this.userSession.reset().then(_=>{
      this.refreshData();
    });
  }

  _points = 0;
  _user = Object;

  refreshData(){
    this.userSession.getusers().then(data => {

      console.log(data);
      this._user = data.filter(el => {        
        return el.username === this.userSession._username;
      })[0];


      this._points = this._user['points'];

      if(!(this._points >0)){
        this._points = 0;
      }
    });    
  }
}
