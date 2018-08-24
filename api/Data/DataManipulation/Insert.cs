using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using app.model;
namespace app.Data.DataManipulation
{
    public static class Insert<T>
    {
        public static string InsertItem(T t)
        {
            PropertyInfo [] properties = t.GetType().GetProperties();
            var columns = new List<string>();
            var values = new List<string>();
            Guid dummy = new Guid();
            string propString;
            int propInt;
            foreach(var property in properties)
            {
                if(property.GetValue(t) == null) 
                    property.SetValue(t,"null");
                else if(property.GetValue(t).GetType() == typeof(Guid)
                    && property.GetValue(t).Equals(dummy)) 
                    property.SetValue(t,Guid.NewGuid());
                else if (property.GetValue(t).GetType() == typeof(DateTime)) 
                    property.SetValue(t,DateTime.Now);
                
                columns.Add(property.Name);
                values.Add(property.GetValue(t).ToString());
            }
            string sql = "Insert into [" + t.GetType().Name + "] (";
            columns.ForEach((col) => sql += col + "," );
            sql = sql.Substring(0,sql.Length - 1);
            sql += ") VALUES (";
            values.ForEach((v) =>{
                 sql += Int32.TryParse(v,out propInt) ? propInt + "," : "'" + Escape_Apostrophes(v) + "'," ;
            });
            sql = sql.Substring(0,sql.Length - 1);
            sql += ")";
            return sql;

        }
        public static string Escape_Apostrophes(string value)
        {
          
            char apos = '\'';
            LinkedList<char> char_list = new LinkedList<char>(value);
            for (LinkedListNode<char> current_node = char_list.First; current_node != null; current_node = current_node.Next)
            {
                if (current_node.Value.Equals(apos))
                {
                    char_list.AddBefore(current_node, apos);
                }
            }
            string escaped_sql = string.Join("",char_list);
            return escaped_sql;
        
        }
        public static string InsertItem(User user)
        {
            string sql = $@"
            Insert INTO UserShare
            (
                firstName,
                lastName,
                profileName,
                profileUrl,
                avatarUrl,
                emailAddress,
                creationDate,
                age,
                country,
                stateProvidence,
                zip,
                streetAddress,
                phone,
                userGuid
            )
            VALUES
            (
                '{user.firstName}',
                '{user.lastName}',
                '{user.profileName}',
                '{user.profileUrl}',
                '{user.avatarUrl}',
                '{user.emailAddress}',
                '{user.creationDate}',
                {user.age},
                '{user.country}',
                '{user.stateProvidence}',
                {user.zip},
                '{user.streetAddress}',
                '{user.phone}',
                '{user.userGuid.ToString()}'
            )";
            
            return sql;
        }
        public static string InsertItem(Post post)
        {
            string sql = $@"
            INSERT INTO POST
            (            
                content,
                creationDate,
                creationTime,
                postUrl,
                postGuid,
                userGuid
            )
            VALUES
            (
                '{post.content}',
                '{post.creationDate}',
                '{post.creationTime}',
                '{post.postUrl}',
                '{post.postGuid.ToString()}',
                '{post.userGuid.ToString()}'
            )";
            return sql;
        }
        public static string InsertItem(Comment comment)
        {
            string sql = $@"
            INSERT INTO Comment
            (
                content,
                commentDate,
                commentTime,
                userGuid,
                commentUrl,
                commentGuid,
                postGuid
            )
            VALUES 
            (
                '{comment.content}',
                {comment.commentDate.Date},
                {comment.commentTime},
                '{comment.userGuid.ToString()}',
                '{comment.commentUrl}',
                '{comment.commentGuid.ToString()}',
                '{comment.postGuid.ToString()}'
            )
            ";
            return sql;
        }
        public static string InsertItem(Friend friend)
        {
            string sql =$@"
            INSERT INTO Friend
            (
                friendGUID,
                userGUID,
                profileUrl
            )
            VALUES 
            (
                '{friend.friendGUID.ToString()}',
                '{friend.userGUID.ToString()}',
                '{friend.profileUrl}'
            )
            
            ";
            return sql;
        }
        public static string InsertItem (CommentLike like)
        {
            string sql = $@"
            Insert INTO CommentLike
            (
                likeGUID,
                commentGUID,
                userGUID,
                friendGUID
            )
            VALUES 
            (
                '{like.likeGUID.ToString()}',
                '{like.commentGUID.ToString()}',
                '{like.userGUID.ToString()}',
                '{like.friendGUID.ToString()}'
                
            )";
            return sql;
        }
        public static string InsertItem(Message m)
        {
            string sql = $@"
            Insert INTO Message
            (
                messageGUID,
                messageContent,
                messageDate,
                messageTime,
                userSentGUID,
                userReceivedGUID,
                friendSentGUID,
                friendReceivedGUID,
                conversationGUID
            )
            Values
            (
                '{m.messageGUID.ToString()}',
                '{m.messageContent}',
                {m.messageDate.Date},
                {m.messageTime.TimeOfDay},
                '{m.userSentGUID.ToString()}',
                '{m.userReceivedGUID.ToString()}',
                '{m.friendSentGUID.ToString()}',
                '{m.friendReceivedGUID.ToString()}',
                '{m.conversationGUID.ToString()}'
            )";
            return sql;
        }
        public static string InsertConversation(Guid userInit, Guid userReceived)
        {
            DateTime dt = DateTime.Now;
            string sql = $@"
            Insert INTO Conversation
            (
                creationDate,
                creationTime,
                userInitiatedGUID,
                userReceivedGUID
            )
            VALUES
            (
                {dt.Date},
                {dt.TimeOfDay},
                '{userInit.ToString()}',
                '{userReceived.ToString()}'
            )";
            return sql;
        }
        public static string InsertItem(Subscription s)
        {
            DateTime dt = DateTime.Now;
            string sql = $@"
            INSERT INTO Subscription
            (
                subscriptionGUID,
                creationDate,
                creationTime,
                followerGUID,
                followedGUID
            )
            VALUES
            (
                '{s.subscriptionGUID.ToString()}',
                {dt.Date},
                {dt.TimeOfDay},
                '{s.followerGUID.ToString()}',
                '{s.followedGUID.ToString()}'
            )";
            return sql;
        }

    }
}