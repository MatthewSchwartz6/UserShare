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
    public class PostRepository : BaseRepository<Post>
    {
        
        public PostRepository()
        {
            SetTable("Post");
            this.guidColumnName = "postGuid";
        }
        
        // public bool Add(Post post)
        // {
        //     return dataClient.Insert(Insert.InsertItem(post));
        // }
        
        
        
    }
}