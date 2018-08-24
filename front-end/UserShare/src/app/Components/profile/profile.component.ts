import { Component, OnInit } from '@angular/core';
import { UserService } from '../../Services/user.service';
import { PostService } from '../../Services/post.service';
import { User } from '../../Model/User';
import { UserClient } from '../../Model/UserClient';
import { Post } from '../../Model/Post';
import { Router } from '@angular/router';
import { Friend } from '../../Model/Friend';
import { FriendService } from '../../Services/friend.service';
import { Store } from '@ngxs/store';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  userClient = new UserClient();
  canAddAsFriend: boolean = true;
  profileId : string;
  userId: string;
  friend: Friend;
  constructor(private store: Store,
        private userService: UserService, 
        private postService: PostService, 
        private friendService: FriendService,  
        private router: Router) { }

  ngOnInit() {
    this.profileId = this.router.url.substring('/profile/'.length);
    this.userId  = this.store.selectSnapshot(state=> state.auth.guid);
    
    this.getUserAndPosts();
    if (this.profileId.toLowerCase() == this.userId.toLowerCase())
    {
      this.canAddAsFriend = false;
    }

  }
  getUserAndPosts(): void {
    this.userService.getUser(this.profileId).subscribe(user => this.userClient.user = user);
    this.postService.getSingleUserPosts(this.profileId).subscribe(posts => this.userClient.posts = posts);
    this.friendService.getFriends(this.userId).subscribe((friends: Friend[]) => {
      this.userClient.friends = friends
      this.userClient.friends.forEach((friend) => {
        if (friend.friendGUID.toLowerCase() == this.profileId.toLowerCase())
        {
          this.canAddAsFriend = false;
        }
      });
    });
  }
  addFriend(): void {
    this.friendService.addFriend(this.userId,this.profileId).subscribe(f=> this.friend = f);
  }

}

