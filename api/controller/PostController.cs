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
    //[Route("api/[controller]")]
    public class PostController : BaseController<Post>
    {
        private PostRepository postRepo;
        public PostController()
        {
            this.postRepo = new PostRepository();
            this.postRepo.SetFields("Post","postGuid");
            this.baseRepo.SetFields("Post","postGuid");
        }
        [HttpGet]
        [Route("/api/GetSingleUserPosts")]
        public IActionResult GetSingleUserPosts(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var posts = postRepo.GetAllFilteredByGuid("userGuid",id);
            if (posts != null)
            {
                return Ok(posts);
            }
            else
            {
                return Ok("No posts where found.");
            }
        }

        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdatePost(string id,[FromBody]JsonPatchDocument<Post> jsonPatch)
        {
            Post toPatch = postRepo.GetSingle(postRepo.guidColumnName,id);
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
                if(!baseRepo.Update(toPatch,this.postRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!postRepo.Update(column,value,this.postRepo.guidColumnName,id))
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