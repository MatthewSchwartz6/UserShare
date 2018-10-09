using System;
using System.Web;
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
    [Route("/api/UserClient")]
    public class UserClientController : Controller
    {
        private UserClientRepository userClientRepo;
        public UserClientController()
        {
            userClientRepo = new UserClientRepository();
        }
        [HttpGet]
        public IActionResult GetUserClient(string id)
        {
            UserClient userClient = userClientRepo.GetUserClient(id);
            if (userClient == null)
            {
                return new StatusCodeResult(404);
            }
            return Ok(userClient);
        }
        [HttpGet]
        [Route("/api/UserClient/PostComments")]
        public IActionResult GetPostComments(string id)
        {
            var comments = userClientRepo.GetAllPostComments(id);
            if (comments == null)
            {
                return null;
            }
            return Ok(comments);
        }
        [HttpPost]
        [Route("/api/UserClient/CheckMembership")]
        public IActionResult CheckMembership([FromBody]Member member)
        {
            string result = userClientRepo.Login(member.password,member.name);
            var userRepo = new UserRepository();
            var user = userRepo.GetSingle("userGuid",result);
            //TODO: get the token somehow
            string token = "todo: implement token validation/creation";
            if (user != null)
            {
                var body = new Dictionary<string,string>{
                    {"token",token},
                    {"profileName",user.profileName},
                    {"guid",result}
                };
                var response = new Dictionary<string,Dictionary<string,string>>{
                    {"body" , body}
                };

                return Ok(response);
            }
            return new StatusCodeResult(404);
        }
        [HttpPost]
        [Route("/api/UserClient/Membership")]
        public IActionResult CreateMembership([FromBody]object obj)
        {
            if (obj == null)
            {
                return BadRequest("Invalid submition");
            }
            var u = JsonConvert.DeserializeObject<User>(obj.ToString());
            var m = JsonConvert.DeserializeObject<Member>(obj.ToString());
            var userRepo = new UserRepository();
            User user = new User() {
                firstName = u.firstName,
                lastName = u.lastName,
                profileName = u.profileName,
                emailAddress = u.emailAddress,
                streetAddress = u.streetAddress,
                stateProvidence = u.stateProvidence,
                country = u.country,
                zip = u.zip,
                phone = u.phone,
                age = u.age
            };
            Member member = new Member(){
                name = u.profileName,
                password = m.password
            };
            var response = new Dictionary<string,string>();
            var profileNames = userClientRepo.GetAllProfileNames();
            if (profileNames != null)
            {
                if (profileNames.Contains(user.profileName))
                {
                    response.Add("response","User already exists."); 
                    return Ok(response);
                }
            }
            if (userRepo.Add(user))
            {
                var userFromDb = userRepo.GetSingle("profileName",member.name);
                string userGuid = userFromDb.userGuid.ToString();
                string profileUrl = "/profile/" + userGuid;
                bool hasAddedProfileUrl = userRepo.Update("profileUrl",profileUrl,"userGuid",userGuid);
                bool hasAddedPassword = userClientRepo.StorePassword(member.password,member.name, userGuid);
                if (hasAddedPassword && hasAddedProfileUrl)
                {
                    response.Add("response","User created successfully.");
                    return Ok(response);
                }
            }

            return new StatusCodeResult(500);            

        }

    }
}