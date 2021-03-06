import { Component, OnInit, Input } from '@angular/core';
import {PostcommentsService} from '../../../Services/postcomments.service';
import {Comment} from '../../../Model/Comment';
import {AmericanDateUtils} from '../../../Shared/DateUtils';
@Component({
  selector: 'app-postcomments',
  templateUrl: './postcomments.component.html',
  styleUrls: ['./postcomments.component.css']
})
export class PostcommentsComponent implements OnInit {

  @Input() postId: string;
  @Input() userId: string;
  @Input() commenter: string;
  comments: Comment[];

  constructor(private postCommentsService: PostcommentsService) { }

  ngOnInit() {
    this.getComments();
  }
  getComments(): void {
    this.postCommentsService.getPostComments(this.postId)
    .subscribe((comm: Comment[]) => {
      comm.forEach((comment)=>{
        comment.commentTime = AmericanDateUtils.formatDate(comment.commentTime);
      });
      this.comments = comm;
    });
  }
  onCommented(comment: Comment): void {
    this.postCommentsService.addComment(comment)
    .subscribe((comment) => {
      comment.commentTime = AmericanDateUtils.formatDate(comment.commentTime);
      this.comments.push(comment);
    });
  }
}
