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
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository()
        {
            SetFields("User","userGuid");
        }
        public void SetFields(string table,string guid) 
        {
            this.dataClient.SetTable(table);
            this.guidColumnName = guid;
            base.SetFields(table,guid);
        }
        
    }
}