import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UsersessionService {

  constructor(private router: Router) { }

  _username: String

  login(username: String) {
    this._username = username;
    console.log(username);

    this.router.navigate(['/ns']);
  }
}
