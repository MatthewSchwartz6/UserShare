--create database UserShare

USE UserShare

CREATE TABLE Post 
(
    postId int primary key Identity(1,1) NOT NULL,
    postUrl varchar(256),
    postGuid uniqueidentifier,
    posterName varchar(256),
    postTitle varchar(256),
    content VARCHAR(MAX),
    creationDate DateTime, 
    creationTime DateTime,
    userGuid uniqueidentifier
)

CREATE TABLE Comment 
(
    commentId int PRIMARY key IDENTITY(1,1) NOT NULL,
    content VARCHAR(max) NOT NULL,
    commentDate DateTime,
    commentTime DateTime,
    userGUID UNIQUEIDENTIFIER NOT NULL,
    commentUrl varchar(256),
    commentGUID UNIQUEIDENTIFIER NOT NULL,
    postGuid UNIQUEIDENTIFIER,
    commenter varchar(256)
)


CREATE TABLE Friend 
(
    friendId int PRIMARY KEY IDENTITY(1,1) NOT NULL,
    friendGUID UNIQUEIDENTIFIER NOT NULL,
    userGUID UNIQUEIDENTIFIER NOT NULL,
    profileUrl varchar(256) NOT NULL
)
CREATE TABLE CommentLike 
(
    likeId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    likeGUID UNIQUEIDENTIFIER NOT NULL,
    commentGUID UNIQUEIDENTIFIER ,
    userGUID UNIQUEIDENTIFIER ,
    friendGUID UNIQUEIDENTIFIER, 
    postGuid UNIQUEIDENTIFIER,
    commenter varchar(256)
)
CREATE TABLE Message 
(
    messageID int primary key IDENTITY(1,1) not null,
    messageGUID UNIQUEIDENTIFIER not null,
    messageContent varchar(max) not null,
    messageDate DATETIME,
    messageTime DATETIME,
    userSentGUID UNIQUEIDENTIFIER not null,
    userReceivedGUID UNIQUEIDENTIFIER not null,
    friendSentToGUID UNIQUEIDENTIFIER,
    friendReceivedGUID UNIQUEIDENTIFIER,
    conversationGUID UNIQUEIDENTIFIER 
)

CREATE TABLE Conversation
(
    conversationID int PRIMARY key IDENTITY(1,1) not null,
    creationDate DATETIME,
    userInitiatedGUID UNIQUEIDENTIFIER,
    userReceivedGUID UNIQUEIDENTIFIER
)

CREATE TABLE Subscription 
(
    subscriptionId int PRIMARY KEY IDENTITY(1,1) not null,
    subscriptionGUID UNIQUEIDENTIFIER,
    creationDate DATETIME, 
    followerGUID UNIQUEIDENTIFIER,
    followedGUID UNIQUEIDENTIFIER
)

CREATE TABLE [dbo].[User]
(
    userId int  PRIMARY KEY Identity(1,1) NOT NULL, 
    userGuid UNIQUEIDENTIFIER,
    firstName varchar(256) NOT NULL,
    lastName varchar(256) NOT NULL, 
    profileName varchar(256) NOT NULL,
    profileUrl varchar(256) ,
    avatarUrl varchar(256),
    emailAddress varchar(256),
    creationDate DateTime,
    age int,
    country varchar(256),
    stateProvidence varchar(256),
    zip int,
    streetAddress varchar(256),
    phone varchar(256)
)

CREATE TABLE Membership
(
    memberID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
    memberGUID UNIQUEIDENTIFIER NOT NULL,
    salt UNIQUEIDENTIFIER NOT NULL,
    pass varbinary(max),
    userGUID UNIQUEIDENTIFIER NOT NULL,
    name VARCHAR(max)
)

