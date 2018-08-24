using System;
using System.Collections.Generic;

using app.model;
using app.Data.DataManipulation;
namespace app.repository
{
    public class FriendRepository : BaseRepository<Friend>
    {
        public FriendRepository()
        {
            SetTable("Friend");
        }
        // public bool Add(Friend friend)
        // {
        //     return dataClient.Insert(Insert.InsertItem(friend));
        // }
    }
}