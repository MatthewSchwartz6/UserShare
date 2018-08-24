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
    public class SubscriptionRepository : BaseRepository<Subscription>
    {

        public SubscriptionRepository()
        {
            SetTable("Subscription");
        }
        // public bool Add(Subscription subscription)
        // {
        //     //get user from controller and insert into db            
        //     return  dataClient.Insert(Insert.InsertItem(subscription));
        // }
    }

}