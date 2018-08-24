using System;
namespace app.model
{
    public class Subscription
    {

        public Guid subscriptionGUID {get;set;}
        public DateTime creationDate {get;set;}
        public DateTime creationTime {get;set;}
        public Guid followerGUID {get;set;}
        public Guid followedGUID {get;set;}
    }
}