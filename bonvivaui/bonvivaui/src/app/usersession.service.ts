import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersessionService {

  constructor() { }
  
  login() {
    return console.log('clicked');
  }
}
