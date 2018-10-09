using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using app.repository;
using app.model.Dto;
using app.model;
namespace app.controller
{
    public class FriendController : BaseController<Friend>
    {
        private FriendRepository friendRepo;
        public FriendController()
        {
            friendRepo = new FriendRepository();
            this.friendRepo.SetFields("Friend","friendGUID");
            this.baseRepo.SetFields("Friend","friendGUID");
        }
        [HttpGet]
        [Route("/api/GetFriends")]
        public IActionResult GetFriends(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var friends = friendRepo.GetAllFilteredByGuid("userGUID",id);
            return Ok(friends);
        }

        [HttpGet]
        [Route("/api/GetFriendDetails")]
        public IActionResult GetFriendDetails(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var friends = friendRepo.GetAllFilteredByGuid("userGUID",id);
            var userRepo = new UserRepository();
            List<User> friendUsers = new List<User>();
            User friendDetails;
            foreach (Friend friend in friends)
            {
                friendDetails = null;
                friendDetails = userRepo.GetSingle("userGuid",friend.friendGUID.ToString());
                friendUsers.Add(friendDetails); 
                             
            }
            if (friendUsers != null)
                return Ok(friendUsers);
            
            return Ok("no friends founds");
        }
        [HttpPost]
        [Route("AddFull[controller]")]
        public IActionResult AddFullFriend( [FromBody] Friend friend)
        {
            if (friend == null)
            {
                return BadRequest("Invalid submition");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // if(!friendRepo.Add(friend))
            // {
            //     return new StatusCodeResult(500);
            // }
            return Ok(friend);
        }
        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdateFriend(string id,[FromBody]JsonPatchDocument<Friend> jsonPatch)
        {
            Friend toPatch = friendRepo.GetSingle(friendRepo.guidColumnName,id);
            jsonPatch.ApplyTo(toPatch,ModelState);
            TryValidateModel(ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = new {
                patchedUser = toPatch,
                patchOperations = jsonPatch
            };
            if(jsonPatch.Operations.Count > 1)
            {
                if(!friendRepo.Update(toPatch,this.friendRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!friendRepo.Update(column,value,this.friendRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
                            /*
            How to send patch from body
            [
                {
                "op": "replace",
                "path": "/<property name>",
                "value": "<property value>"
                },
                {
                "op": "replace",
                "path": "/<property name>",
                "value": "<property value>"
                },
            ]
             */
            return Ok(model);
        }
    }
}