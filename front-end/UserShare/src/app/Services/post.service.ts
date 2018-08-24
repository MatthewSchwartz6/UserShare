import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserClient} from '../Model/UserClient';
import { Post } from '../Model/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {


  private UserPostURL = '/api/Post';
  private singleUserPostsURL = '/api/GetSingleUserPosts';


  post: Post;
  constructor(private http: HttpClient) { 
  }


  getSingleUserPosts(id: string): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.singleUserPostsURL}?id=${id}`);
  }
  addPost(post: Post): Observable<Post> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };
    return this.http.post<Post>(this.UserPostURL, post, httpOptions);
  }

}
