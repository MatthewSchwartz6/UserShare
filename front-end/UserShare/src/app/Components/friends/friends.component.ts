import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/friend.service';
import { Store } from '@ngxs/store';
import { Friend } from '../../Model/Friend';
import { User } from '../../Model/User';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  private guid: string = this.store.selectSnapshot(state => state.auth.guid);
  private friends$: Observable<User[]>;
  constructor(private store: Store, private friendService: FriendService) { }

  ngOnInit() {
    this.getFriends();
  }
  getFriends() {
    this.friends$ = this.friendService.getFriendDetails(this.guid);
  }
}
