using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using app.repository;
using app.model.Dto;
using app.model;

namespace app.controller
{
    [Authorize]
    [Route("/api/[controller]")]
    public class BaseController<T>: Controller
    {
        protected BaseRepository<T> baseRepo;

        public BaseController()
        {
            //routeName = "something";
            baseRepo = new BaseRepository<T>();
        
        }
   
        [HttpGet]
        public IActionResult Get(T t)
        {
            var dict = baseRepo.GetObjectAsDict(t);

            if(dict.Count == 1)
            {
                var items = baseRepo.GetAll();
                Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(new { totalCount = items.Count() }));
                return Ok(items);
            }
            else if (dict.Count() == 2)
            {
                dict.Remove("PropertyCount");
                var item = baseRepo.GetSingle(dict.SingleOrDefault().Key,dict.SingleOrDefault().Value);
                return Ok(item);
            }
            else 
            {
                var returned = baseRepo.GetAllFiltered(dict);
                if (returned == null)
                {
                    return BadRequest("User not found");
                }
                return Ok(returned);
            }
            
                
        }
            
        [HttpPost]
        public IActionResult Add([FromBody]T t)
        {
            if (t == null)
            {
                return BadRequest("Invalid submition");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!baseRepo.Add(t))
            {
                return new StatusCodeResult(500);
            }
            else
                return Ok(t);
        }

        [HttpPut]
        public IActionResult Update(string id, [FromBody]T t)
        {
            if(!baseRepo.Update(t,this.baseRepo.guidColumnName,id))
            {
                return new StatusCodeResult(500);
            }
            return Ok(t);
        }
       
        [HttpDelete]
        public IActionResult Delete(T t)
        {
            var dict = baseRepo.GetObjectAsDict(t);
            dict.Remove("PropertyCount");
            var key = dict.FirstOrDefault().Key;
            var value = dict.FirstOrDefault().Value;
            if (!baseRepo.Remove(key,value))
            {
                return new StatusCodeResult(500);
            }
            return NoContent();
        }
        
    }
}