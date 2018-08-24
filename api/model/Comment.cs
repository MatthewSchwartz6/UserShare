using System;

namespace app.model
{
    public class Comment
    {
        public string content {get;set;}
        public string commenter {get;set;}
        public DateTime commentDate {get;set;}
        public DateTime commentTime {get;set;}
        public string commentUrl {get;set;}
        public Guid commentGuid {get;set;}
        public Guid userGuid {get;set;}

        public Guid postGuid {get;set;}
       
    }
}