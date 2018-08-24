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
    public class CommentController : BaseController<Comment>
    {
        private CommentRepository commentRepo;
        public CommentController()
        {
            commentRepo = new CommentRepository(); 
            this.commentRepo.SetFields("Comment","commentGUID");
            this.baseRepo.SetFields("Comment","commentGUID");  
        }

        
        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdateComment(string id,[FromBody]JsonPatchDocument<Comment> jsonPatch)
        {
            Comment toPatch = commentRepo.GetSingle(commentRepo.guidColumnName,id);
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
                if(!commentRepo.Update(toPatch,this.commentRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!commentRepo.Update(column,value,this.commentRepo.guidColumnName,id))
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