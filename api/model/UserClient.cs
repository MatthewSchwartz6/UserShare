using System;
using System.Collections.Generic;
using app.model.Dto;
namespace app.model
{
    public class UserClient
    {
        public User user {get;set;}
        public List<CommentLike> likes {get;set;}
        public List<Post> posts {get;set;}        
        public List<Comment> comments {get;set;}
        public List<Friend> friends {get;set;}
        public List<Subscription> subscriptions {get;set;}
        public List<Message> messages {get;set;}
        public List<Post> friendsPosts{get;set;}
        public UserClient()
        {
            likes = new List<CommentLike>();
            comments = new List<Comment>();
            friends = new List<Friend>();
            subscriptions = new List<Subscription>();
            messages = new List<Message>();
            posts = new List<Post>();
            friendsPosts = new List<Post>();
        }
    }
}