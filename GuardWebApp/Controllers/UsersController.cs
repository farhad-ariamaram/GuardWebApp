using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuardWebApp.Models;
using GuardWebApp.Controllers.Utils;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace GuardWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GuardianDBContext _context;

        public UsersController(GuardianDBContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public async Task<ActionResult<User>> Login(string username, string password)
        {
            string key = ApiUtilities._KEY;
            string tk = ApiUtilities.rndTransferKey();
            string p0 = ApiUtilities._P0;
            string p1 = ApiUtilities.EncryptString(username, key); ;
            string p2 = ApiUtilities.EncryptString(password, key);
            string p3 = ApiUtilities.EncryptString(tk, key);

            var theWebRequest = HttpWebRequest.Create("http://192.168.10.250/ExLogin.aspx/LI");
            theWebRequest.Method = "POST";
            theWebRequest.ContentType = "application/json; charset=utf-8";
            theWebRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

            using (var writer = theWebRequest.GetRequestStream())
            {
                string send = null;
                send = "{\"p0\":\"" + p0 + "\",\"p1\":\"" + p1 + "\",\"p2\":\"" + p2 + "\",\"p3\":\"" + p3 + "\"}";

                var data = Encoding.UTF8.GetBytes(send);

                writer.Write(data, 0, data.Length);
            }

            var theWebResponse = (HttpWebResponse)theWebRequest.GetResponse();
            var theResponseStream = new StreamReader(theWebResponse.GetResponseStream());

            string result = theResponseStream.ReadToEnd();

            try
            {
                result = "{" + result.Substring(28).Replace("}}", "}");
            }
            catch (Exception e)
            {
                var apiresult = new ApiUser() { id = "", name = "", Status = "false" };
                var data = JsonConvert.SerializeObject(apiresult);
                return Ok(data);
            }

            var splashInfo = JsonConvert.DeserializeObject<ApiUser>(result);

            string backTk = ApiUtilities.DecryptString(splashInfo.Status, key);
            if (tk == ApiUtilities.Reverse(backTk))
            {
                splashInfo.id = ApiUtilities.DecryptString(splashInfo.id, key);
                splashInfo.name = ApiUtilities.DecryptString(splashInfo.name, key);
                splashInfo.Status = ApiUtilities.DecryptString(splashInfo.Status, key);

                var currentUser = _context.Users.Where(a => a.Id == int.Parse(splashInfo.id)).FirstOrDefault();

                if (currentUser != null)
                {
                    //check name
                    if (!currentUser.Name.Equals(splashInfo.name))
                    {
                        currentUser.Name = splashInfo.name;
                    }

                    //check pass
                    if (!currentUser.Password.Equals(ApiUtilities.sha512(password + ApiUtilities._SALT)))
                    {
                        currentUser.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
                    }

                    _context.Users.Update(currentUser);
                    _context.SaveChanges();

                    var apiresult = new ApiUser() { id = splashInfo.id, name = splashInfo.name, Status = "true" };
                    var data = JsonConvert.SerializeObject(apiresult);
                    return Ok(data);

                }
                else
                {
                    User t = new User();

                    t.Id = int.Parse(splashInfo.id);
                    t.Username = username;
                    t.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
                    t.Name = splashInfo.name;
                    t.Type = "guard";

                    _context.Users.Add(t);
                    _context.SaveChanges();

                    var apiresult = new ApiUser() { id = splashInfo.id, name = splashInfo.name, Status = "true" };
                    var data = JsonConvert.SerializeObject(apiresult);
                    return Ok(data);
                }

            }
            else
            {
                return new JsonResult(new { data = "false", uid = "" });
            }
        }

        public class ApiUser
        {
            public string Status;
            public string id;
            public string name;
        }
    }
}
