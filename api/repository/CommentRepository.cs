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
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository()
        {
            SetTable("Comment");
        }
        public List<Comment> GetAllFromFriend(string userGuid,string friendGuid)
        {
            dataClient.AddWhereClause("userGuid",userGuid);
            dataClient.AddWhereClause("friendGuid",friendGuid);
            return dataClient.GetAll();
        }
        // public bool Add(Comment comment)
        // {
        //     return dataClient.Insert(Insert.InsertItem(comment));
        // }
        
        public bool Edit(string newContent,string commentGuid)
        {
            return dataClient.UpdateOne("content",newContent,"commentGuid",commentGuid);
        }
        
    }
}