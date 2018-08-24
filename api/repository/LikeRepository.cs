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
    public class LikeRepository : BaseRepository<CommentLike>
    {

        public LikeRepository()
        {
            SetTable("CommentLike");
        }
        // public bool Add(Like post)
        // {
        //     return dataClient.Insert(Insert.InsertItem(post));
        // }
        
    }
}