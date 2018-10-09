using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using app.model;
using app.model.Dto;
using app.Data.DataOperations;
using app.Data.DataManipulation;
namespace app.repository
{
    public class UserClientRepository 
    {
        public UserClientRepository() 
        {

        }
        public UserClient GetUserClient(string id)
        {
            UserClient userClient = new UserClient();
            userClient.user = new UserRepository().GetSingle("userGuid",id);
            userClient.posts = new PostRepository().GetAllFilteredByGuid("userGuid",id);
            userClient.friends = new FriendRepository().GetAllFilteredByGuid("userGUID",id);
            userClient.subscriptions = new SubscriptionRepository().GetAllFilteredByGuid("followerGUID",id);
            foreach(var f in userClient.friends)
            {
                var posts = new PostRepository().GetAllFilteredByGuid("userGuid",f.friendGUID.ToString());
                userClient.posts.AddRange(posts);
            }
            userClient.posts = userClient.posts.OrderByDescending(o => o.creationDate).ToList();
            return userClient;
        }
        public List<string> GetAllProfileNames()
        {
            var uRepo = new UserRepository();
            var users = uRepo.GetAll();
            if (users.Count > 0)
            {
                return users.Select(x=> x.profileName).ToList();
            }
            return null;
        }
        public List<Comment> GetAllPostComments(string postId)
        {
            return new CommentRepository().GetAllFilteredByGuid("postGuid",postId);
        }
        public List<CommentLike> GetAllPostLikes(string id)
        {
            return new LikeRepository().GetAllFilteredByGuid("postGuid",id);
        }
        public List<CommentLike> GetAllCommentLikes(string id)
        {
            return new LikeRepository().GetAllFilteredByGuid("commentGuid",id);
        }
        public bool StorePassword(string pswd,string nm, string id)
        {
            var dataClient = new DataClient<User>();
            if (Guid.TryParse(id,out Guid g))
            {
                return dataClient.InsertPassword(pswd,nm,g);
            }
            else 
            {
                return false;
            }
        }
        public string Login(string pswd, string name)
        {
            var dataClient = new DataClient<User>();

            return dataClient.PassValidation(pswd,name);
            
        }
        

    }
}