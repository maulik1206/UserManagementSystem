import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { BaseTVM, BaseVM } from '../models/baseVM';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');
    if (token) {
      req = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`),
      });
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Client-side error: ${error.error.message}`;
        } else {
          // Server-side error
          if (error.status === 400) {
            errorMessage = 'Bad request. Please check the data.';
          } else if (error.status === 401) {
            errorMessage = 'Unauthorized. Please log in again.';
          } else if (error.status === 500) {
            errorMessage = 'Server error. Please try again later.';
          }

          // Custom error message based on response model
          if (error.error?.messages) {
            errorMessage = error.error.messages.map((msg: any) => msg.message).join(', ');
          }
        }

        // Optionally log the error or send it to an external logging service
        console.error(`Error Status: ${error.status}\nMessage: ${errorMessage}`);
        
        // Return the error message to be displayed
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
