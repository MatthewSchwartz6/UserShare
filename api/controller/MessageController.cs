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
    public class MessageController : BaseController<Message>
    {
        private MessageRepository messageRepo;
        public MessageController()
        {
            messageRepo = new MessageRepository();
            this.messageRepo.SetFields("Message","messageGUID");
            this.baseRepo.SetFields("Message","messageGUID");
        }
        [HttpPatch]
        [Route("PartiallyUpdate[controller]")]
        public IActionResult PartiallyUpdateMessage(string id,[FromBody]JsonPatchDocument<Message> jsonPatch)
        {
            Message toPatch = messageRepo.GetSingle(messageRepo.guidColumnName,id);
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
                if(!baseRepo.Update(toPatch,this.messageRepo.guidColumnName,id))
                {
                    return new StatusCodeResult(500);
                }
            }
            else 
            {
                string column = jsonPatch.Operations.Single().path.Substring(1);
                string value = jsonPatch.Operations.Single().value.ToString();
                if(!messageRepo.Update(column,value,this.messageRepo.guidColumnName,id))
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