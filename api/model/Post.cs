using System;
namespace app.model
{
    public class Post
    {
        public Guid postGuid {get;set;}
        public string content {get;set;}
        public Guid userGuid {get;set;}
        public DateTime creationTime {get;set;}
        public DateTime creationDate {get;set;}
        public string postUrl {get;set;}
        public string postTitle {get;set;}
        public string posterName {get;set;}
        
    }
}