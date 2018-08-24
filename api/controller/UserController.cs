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
    //[Route("/UserCustom/")]
    
    public class UserController : BaseController<User>
    {
        public UserRepository userRepo;
        public UserController() : base()
        {
            this.userRepo = new UserRepository();
            this.baseRepo.SetFields("User","userGuid");
        }
        


        [HttpGet]
        [Route("/api/GetUsers")]
        public IActionResult GetAllFromSearchQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }
            var users = userRepo.GetAll();
            users = users.Where(u => u.profileName.ToLower().Contains(query.ToLower()) 
                                || u.firstName.ToLower().Contains(query.ToLower()) 
                                || u.lastName.ToLower().Contains(query.ToLower()))
                        .OrderBy(o => o.profileName).ToList();
            if (users.Count > 0)
                return Ok(users);
            else 
                return Ok();
        }
        [HttpGet]
        [Route("/api/GetUser")]
        public IActionResult GetSingleUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var user = userRepo.GetSingle("userGuid",id);
            if (user != null)
                return Ok(user);
            else
                return Ok("No user found.");
        }
        
        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdateUser(string id,[FromBody]JsonPatchDocument<User> jsonPatch)
        {
            User toPatch = userRepo.GetSingle(userRepo.guidColumnName,id);
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
                if(!baseRepo.Update(toPatch,this.userRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!userRepo.Update(column,value,this.userRepo.guidColumnName,id))
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