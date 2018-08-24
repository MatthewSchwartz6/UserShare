import { Injectable } from '@angular/core';
import { User } from '../../Model/User';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { map, retry, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private loginURL: string = '/api/UserClient/CheckMembership';
  private signupURL: string = '/api/UserClient/Membership';
  constructor(private httpClient: HttpClient) { }

  public SignIn(user: User) {

    const body = {
      name: user.profileName,
      password: user.password
    };
    const httpOptions = {
      headers: new HttpHeaders({
        'responseType': 'text'
      })
    };
    return this.httpClient.post<any>(this.loginURL,body,httpOptions)
      .pipe(
        retry(5),
        catchError(this.handleError)
      );
  }

  public SignUp(user: User) {

    const httpOptions = {
      headers: new HttpHeaders({
        'responseType': 'text'
      })
    };
    return this.httpClient.post<any>(this.signupURL,user,httpOptions)
    .pipe(
      retry(5),
      catchError(this.handleError)
    );
  }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }

}
