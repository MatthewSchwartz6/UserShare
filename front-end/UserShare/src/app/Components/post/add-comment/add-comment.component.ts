import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Comment } from '../../../Model/Comment';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit {
  @Input() postId: string;
  @Input() userId: string;
  @Input() commenter: string;
  @Output() commentOuput = new EventEmitter<Comment>();
  private commentContent: string;
  comment = new Comment();

  constructor() { }

  ngOnInit() {
  }
  onClick(): void {
    this.comment.userGuid = this.userId;
    this.comment.commenter = this.commenter;
    // the two above will be false on the profile page because the 
    //client.user on the profile page will be user whose profile youre at
    //and not the session user...
    //the session user is the one who comments and posts, those things should be set by the session
    //the guid should not be visible by local storage token that is BAD 
    this.comment.content = this.commentContent;
    this.comment.postGuid = this.postId;
    this.commentOuput.emit(this.comment);
    this.commentContent = "";
  }

}
