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
    public class MessageRepository : BaseRepository<Message>
    {
        public MessageRepository()
        {
            
        }
        // public bool Add(Message message)
        // {
        //     return dataClient.Insert(Insert.InsertItem(message));
        // }
    }
}