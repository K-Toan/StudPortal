import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Account } from '../models/account';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  public register(account: Account): Observable<any> {
    return this.http.post<any>('http://localhost:5250/api/Auth/register', account);
  }

  public login(account: Account): Observable<string> {
    return this.http.post('http://localhost:5250/api/Auth/login', account, {responseType: "text"});
  }
}
