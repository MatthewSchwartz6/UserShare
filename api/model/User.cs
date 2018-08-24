using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace app.model
{
    public class User
    {
        public Guid userGuid {get;set;}
        public string firstName {get;set;}
        public string lastName {get;set;}
        public string profileName {get;set;}
        public string profileUrl{get;set;} = "";
        public string avatarUrl {get;set;} = "";
        public string emailAddress {get;set;} = "";
        public DateTime creationDate{get;set;} 
        public int age {get;set;} = 0;
        public string country {get;set; } ="";
        public string stateProvidence {get;set; } ="";
        public int zip {get;set;} = 0;
        public string streetAddress {get;set; } ="";
        public string phone {get;set; } ="";
        
    }
}