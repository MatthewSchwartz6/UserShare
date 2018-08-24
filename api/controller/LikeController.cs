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
    public class LikeController : BaseController<CommentLike>
    {
        private LikeRepository likeRepo;
        public LikeController()
        {
            likeRepo = new LikeRepository();
            this.likeRepo.SetFields("CommentLike","likeGUID");
            this.baseRepo.SetFields("CommentLike","likeGUID");
        }

        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdateLike(string id,[FromBody]JsonPatchDocument<CommentLike> jsonPatch)
        {
            CommentLike toPatch = likeRepo.GetSingle(likeRepo.guidColumnName, id);
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
                if(!likeRepo.Update(toPatch,this.likeRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!likeRepo.Update(column,value,this.likeRepo.guidColumnName,id))
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