using System;
using System.Collections.Generic;
namespace app.Data.DataManipulation
{
    public class Join
    {
        public string JoinType{get;set;}
        public string TableToJoinOn{get;set;}
        public List<List<string>> leftTables;
        public List<List<string>> rightTables;
        public Join()
        {
            leftTables = new  List<List<string>>();
            rightTables = new  List<List<string>>();
        }
    }
}