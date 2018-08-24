using System;
namespace app.model
{
    public class Message
    {
        public int Id {get;set;}
        public Guid messageGUID{get;set;}
        public string messageContent {get;set;}
        public DateTime messageDate {get;set;}
        public DateTime messageTime {get;set;}
        public Guid userSentGUID {get;set;}
        public Guid userReceivedGUID {get;set;}
        public Guid friendSentGUID {get;set;}
        public Guid friendReceivedGUID {get;set;} 
        public Guid conversationGUID {get;set;}

    }
}