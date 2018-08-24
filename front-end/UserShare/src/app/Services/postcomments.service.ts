import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Comment} from '../Model/Comment';
@Injectable({
  providedIn: 'root'
})
export class PostcommentsService {

  private url = '/api/UserClient/PostComments';
  private CommentURL = '/api/Comment';
  constructor(private http: HttpClient) { }

  getPostComments(id: string): Observable<Comment[]> {
    const params = new HttpParams({
      fromObject : {
        id : id,
      }
    });
    return this.http.get<Comment[]>(this.url, {params: params});
  }
  addComment(comment: Comment): Observable<Comment> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };
    return this.http.post<Comment>(this.CommentURL, comment, httpOptions);
  }
}
