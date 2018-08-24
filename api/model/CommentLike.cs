using System;
namespace app.model
{
    public class CommentLike
    {

     public Guid likeGUID {get;set;} 
     public Guid commentGUID {get;set;}
     public Guid userGUID {get;set;}
     public Guid friendGUID {get;set;}  
     public Guid postGuid {get;set;}
    }
}