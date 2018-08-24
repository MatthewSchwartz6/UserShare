use UserShare
GO

insert into [User] (userGuid,firstName,lastName,profileName)
VALUES (NEWID(),'Matt','Schwartz','matfueX5000')
insert into [User] (userGuid,firstName,lastName,profileName)
VALUES (NEWID(),'John','Doe','John Doe')



use UserShare
GO


declare @id UNIQUEIDENTIFIER
declare @friendid UNIQUEIDENTIFIER
declare @commentid UNIQUEIDENTIFIER
declare @firstname VARCHAR(max)
declare @postid UNIQUEIDENTIFIER
declare @salt UNIQUEIDENTIFIER
set @salt = NEWID()

declare @pass nvarchar(max)
set @pass = 'TODO: ADD PASSWORD' + + Cast(@salt as nvarchar(max))

declare @h varbinary(max)
set @h = HASHBYTES('SHA2_512', @pass)

select @id=userGuid,@firstname=firstName from [User] where userId=1
select @friendid=userGuid from [User] where userId=2


insert into Post(postGuid,posterName,content,postTitle,creationDate,creationTime,postUrl,userGuid)
values(NEWID(),@firstname,'some demo content','some demo post title',GETDATE(),GETDATE(),'example.com',@id)

select @postid=postGuid from Post where postId=1

insert into Comment(content,commentDate,commentTime,userGUID,commentUrl,commentGUID,postGuid,commenter)
values ('WHOAH THAT WAS SO COOL DUDE',GETDATE(),GETDATE(),
@id,'EXAMPLE.COM',NEWID(),@postid,@firstname)

select @commentid=commentGuid from Comment where commentId=1

insert into Friend(friendGUID,userGUID,profileUrl) values (@friendid,@id,'example.com')

insert into CommentLike(likeGUID,userGUID,friendGUID,postGuid,commenter,commentGuid) values (NEWID(),@id,@friendid,@postid,@firstname,@commentid)

insert into [Conversation](creationDate,userInitiatedGUID,userReceivedGUID)
values (GETDATE(),@id, @friendid)

insert into [Message](messageGUID, messageContent, messageDate, messageTime, userSentGUID, userReceivedGUID)
values(NEWID(),'This is a message',GETDATE(),GETDATE(),@id,@friendid)

insert into Subscription(subscriptionGUID,creationDate,followedGUID, followerGUID)
values (NEWID(), GETDATE(), @friendid, @id)


insert into Membership(memberGUID,pass,salt,userGUID,name) values (NEWID(),@h,@salt,@id,'matfueX5000')

GO
use UserShare 


select * from [User]
select * from Membership
select * from Comment
select * from Post
select * from CommentLike
select * from Friend
select * from [Message]
select * from Subscription
select * from Conversation


