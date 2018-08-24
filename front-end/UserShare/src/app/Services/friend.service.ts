import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Model/User';
import { Friend } from '../Model/Friend';

@Injectable({
  providedIn: 'root'
})
export class FriendService {

  friendUrl: string = '/api/Friend';
  getFriendsUrl: string = '/api/GetFriends';
  getFriendDetailUrl: string = '/api/GetFriendDetails';
  friend:Friend = new Friend();
  constructor(private http: HttpClient) { }

  addFriend(userGUID: string, friendGUID: string): Observable<Friend> {
    //post request to friend controller
    //send my guid, and friend guid
    this.friend.userGUID = userGUID;
    this.friend.friendGUID = friendGUID;
    this.friend.profileUrl = `/profile/${friendGUID}`;
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };
    return this.http.post<Friend>(this.friendUrl,this.friend,httpOptions);
  }
  getFriends(userGUID: string): Observable<Friend[]> {
    return this.http.get<Friend[]>(`${this.getFriendsUrl}?id=${userGUID.toLowerCase()}`);
  }
  getFriendDetails(userGUID: string): Observable<User[]> {
    return this.http.get<User[]>(`${this.getFriendDetailUrl}?id=${userGUID.toLowerCase()}`);
  }
  // deleteFriend(): void {

  // }
}
