import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { UserClient } from '../Model/UserClient';

@Injectable({
  providedIn: 'root'
})
export class UserClientService {

  private UserClientURL = '/api/UserClient';
  public userGuid: string;

  constructor(private http: HttpClient, private store:  Store) { 
    this.userGuid = this.store.selectSnapshot(state => state.auth.guid);
  }
  getUserClient(): Observable<UserClient> {
    const params = new HttpParams({
      fromObject : {
        id : this.userGuid,
      }
    });
    return this.http.get<UserClient>(this.UserClientURL, {params: params});
  }
}
