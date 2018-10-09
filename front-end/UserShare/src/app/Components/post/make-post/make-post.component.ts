import { Component, Input, Output, EventEmitter} from '@angular/core';
import { Post } from '../../../Model/Post';
import { User } from '../../../Model/User';


@Component({
  selector: 'app-make-post',
  templateUrl: './make-post.component.html',
  styleUrls: ['./make-post.component.css']
})
export class MakePostComponent {
  post = new Post();
  @Input() user: User;
  @Output() posted = new EventEmitter<Post>();
  constructor() {

  }

  onSubmit() {
  this.SubmitPost();
  this.post = new Post();
  }
  SubmitPost() {
    this.post.userGuid = this.user.userGuid;
    this.post.posterName = this.user.profileName;
    this.post.postTitle = 'Rando title';
    this.post.postUrl = this.user.profileUrl
    this.posted.emit(this.post);
  }
}
