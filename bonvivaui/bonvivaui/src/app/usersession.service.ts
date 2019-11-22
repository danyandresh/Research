import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, retry, catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UsersessionService {

  constructor(private router: Router, private http: HttpClient) { }

  _baseurl = 'http://52.169.255.164:4204/api/users/'
  public _username: String

  ensureLogin() {
    this._username = localStorage.getItem('user');
    if (!this._username && this.router.url !== '/login') {
      this.router.navigate(['/login']);
    }
  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  login(username: string) {
    this._username = username;
    console.log(username);

    var payload = JSON.stringify({ 'username': username });

    return this.http.post(this._baseurl, payload, this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      ).subscribe(data => {
        console.log(data);

        localStorage.setItem('user', this._username.toString());
        this.router.navigate(['/home']);
      });

  }

  async getusers(): Promise<any[]> {
    this.ensureLogin();

    return this.http.get<any[]>(this._baseurl, this.httpOptions)
      .pipe(
        map(item => item),
        retry(1),
        catchError(this.errorHandl)
      ).toPromise();

  }

  async addPoints(): Promise<any> {
    this.ensureLogin();

    var payload = JSON.stringify({ 'username': this._username });

    return this.http.put<any>(this._baseurl, payload, this.httpOptions)
      .pipe(
        map(item => item),
        retry(1),
        catchError(this.errorHandl)
      ).toPromise();

  }

  async reset(): Promise<any> {
    this.ensureLogin();

    return this.http.delete(this._baseurl, this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      ).toPromise();

  }

  logdata(data) {
    console.log(data);
  }

  // Error handling
  errorHandl(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
