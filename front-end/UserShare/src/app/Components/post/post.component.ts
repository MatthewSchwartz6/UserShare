import { Component, OnInit, Input } from '@angular/core';
import {Post} from '../../Model/Post';
import {UserClient} from '../../Model/UserClient';
import {User} from '../../Model/User';
import {PostService} from '../../Services/post.service';
import { UserService } from '../../Services/user.service';
import { Store } from '@ngxs/store';
import { UserClientService } from '../../Services/user-client.service';
import {AmericanDateUtils} from '../../Shared/DateUtils';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  client: UserClient;
  currentUser: User;
  @Input() profileClient: UserClient;
  constructor(private store: Store,
              private userClientService: UserClientService,
              private postService: PostService,
              private userService: UserService) { }

  ngOnInit() {
    if(this.profileClient)
    {
      this.client = this.profileClient;
      setTimeout(() => {}, 100);
    }
    else
    {
      this.getPosts();
    }
    this.userService.getCurrentUser(this.store.selectSnapshot(state=> state.auth.guid)).subscribe(cUser => { this.currentUser = cUser});
  }
  getPosts(): void {
     this.userClientService.getUserClient()
      .subscribe((uc: UserClient) => {
        this.client = uc;  
        this.formatDates();  
      });

   }
   getUserName(): string {
     return this.client.user.profileName;
   }
   onPosted(post: Post): void {
      this.postService.addPost(post)
      .subscribe((p: Post) => {
        p.creationDate = AmericanDateUtils.formatDate(p.creationDate);
        this.client.posts.push(p);
      });
   }
   formatDates(): void {
     this.client.posts.forEach((p,idx)=>{
       p.creationDate = AmericanDateUtils.formatDate(p.creationDate) ;
     });
   }
}
