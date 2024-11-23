import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BaseTVM, BaseVM } from '../models/baseVM';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUser(): Observable<BaseTVM<User>> {
    return this.http.get<BaseTVM<User>>(`${this.apiUrl}/me`);
  }

  updateUser(data: { username: string; email: string }): Observable<BaseVM> {
    return this.http.put<BaseVM>(`${this.apiUrl}/me`, data);
  }
}
