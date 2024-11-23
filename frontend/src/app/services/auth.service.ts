import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { BaseTVM, BaseVM } from '../models/baseVM';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  register(username: string, email: string, password: string): Observable<BaseTVM<string>> {
    return this.http.post<BaseTVM<string>>(`${this.apiUrl}/auth/register`, { username, email, password });
  }

  login(email: string, password: string): Observable<BaseTVM<string>> {
    return this.http.post(`${this.apiUrl}/auth/login`, { email, password }).pipe(
      tap((response: any) => {
        localStorage.setItem('token', response.data); // store JWT
      })
    );
  }

  logout(): Observable<BaseVM>  {
    return this.http.post(`${this.apiUrl}/auth/logout`,'').pipe(
      tap((response: any) => {
        localStorage.removeItem('token');
      })
    );
  }

  get isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}