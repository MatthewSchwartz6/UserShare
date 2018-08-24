import { Injectable } from '@angular/core';
import { User } from '../Model/User';
import { Observable, of, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseSearchUrl: string = '/api/GetUsers';
  finalSearchUrl: string;
  baseGetUserUrl: string = '/api/GetUser'
  query: string;
  currentUser: User;
  public searchQuery = new Subject<string>();

  constructor(private http: HttpClient) { }

  public setSearchQuery(query: string): void {
    this.finalSearchUrl = `${this.baseSearchUrl}?query=${query}`;
    this.searchQuery.next(this.finalSearchUrl);
  }
  public getSearchQuery(): string {
    return this.query;
  }
  public getSearchResults(): Observable<User[]> {
    if (!this.finalSearchUrl)
    {
      return of([]);
    } 

    return this.http.get<User[]>(this.finalSearchUrl); 
  }
  public getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.baseGetUserUrl}?id=${id}`);
  }
  public getCurrentUser(id: string): Observable<User>{
    return this.currentUser
    ? of(this.currentUser)
    : this.http.get<User>(`${this.baseGetUserUrl}?id=${id}`)
      .pipe(tap(user => { this.currentUser = user }));
  }
}
