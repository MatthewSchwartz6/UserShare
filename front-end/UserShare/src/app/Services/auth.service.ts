import { Injectable } from '@angular/core';
import { User } from '../Model/User';
import { Observable, throwError, of } from 'rxjs';
import { map, retry, catchError } from 'rxjs/operators';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthStateModel } from '../Model/AuthStateModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl: string = '/api/UserClient/CheckMembership';
  constructor(private http: HttpClient ) { }

  SignIn(profileName: string, password:string)
    : Observable<AuthStateModel> {

    const body = {
      name: profileName,
      password: password
    };
    const httpOptions = {
      headers: new HttpHeaders({
        'responseType': 'text'
      })
    };
    return this.http.post<AuthStateModel>(this.loginUrl,body,httpOptions)
      .pipe(
        map((res: any) => { return res.body}),
        retry(5),
        catchError(this.handleError)
      );
  }
  Signout(): Observable<null> {
    return of(null);
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
