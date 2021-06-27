using GuardWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GuardApiApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class Guard : ControllerBase
    {
        private GuardianDBContext _db;

        public Guard(GuardianDBContext db)
        {
            _db = db;
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            var data = JsonConvert.SerializeObject(true);
            return Ok(data);
        }

        [HttpGet("login")]
        public IActionResult Login(string user, string pass)
        {
            string key = Utilities.Utilities._KEY;
            string tk = Utilities.Utilities.rndTransferKey();
            string p0 = Utilities.Utilities._P0;
            string p1 = Utilities.Utilities.EncryptString(user, key); ;
            string p2 = Utilities.Utilities.EncryptString(pass, key);
            string p3 = Utilities.Utilities.EncryptString(tk, key);

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

            string backTk = Utilities.Utilities.DecryptString(splashInfo.Status, key);
            if (tk == Utilities.Utilities.Reverse(backTk))
            {
                splashInfo.id = Utilities.Utilities.DecryptString(splashInfo.id, key);
                splashInfo.name = Utilities.Utilities.DecryptString(splashInfo.name, key);
                splashInfo.Status = Utilities.Utilities.DecryptString(splashInfo.Status, key);

                var currentUser = _db.Users.Where(a => a.Id == int.Parse(splashInfo.id)).FirstOrDefault();

                if (currentUser != null)
                {
                    //check name
                    if (!currentUser.Name.Equals(splashInfo.name))
                    {
                        currentUser.Name = splashInfo.name;
                    }

                    //check pass
                    if (!currentUser.Password.Equals(Utilities.Utilities.sha512(pass + Utilities.Utilities._SALT)))
                    {
                        currentUser.Password = Utilities.Utilities.sha512(pass + Utilities.Utilities._SALT);
                    }

                    _db.Users.Update(currentUser);
                    _db.SaveChanges();

                    var apiresult = new ApiUser() { id = splashInfo.id, name = splashInfo.name, Status = "true" };
                    var data = JsonConvert.SerializeObject(apiresult);
                    return Ok(data);

                }
                else
                {
                    User t = new User();

                    t.Id = int.Parse(splashInfo.id);
                    t.Username = user;
                    t.Password = Utilities.Utilities.sha512(pass + Utilities.Utilities._SALT);
                    t.Name = splashInfo.name;

                    _db.Users.Add(t);
                    _db.SaveChanges();

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

        [HttpGet("syncPlaces")]
        public IActionResult SyncPlaces()
        {
            var places = _db.Locations.Select(a => new { a.Id, a.Name, a.Qr, a.Nfc }).ToList();
            var data = JsonConvert.SerializeObject(places);
            return Ok(data);
        }

        [HttpPost("postCheckedPlace")]
        public IActionResult postCheckedPlaces([FromBody] SubmittedLocation checkedPlace)
        {
            SubmittedLocation data = new SubmittedLocation()
            {
                DateTime = checkedPlace.DateTime,
                DeviceId = 1,
                LocationId = checkedPlace.LocationId,
                UserId = checkedPlace.UserId
            };
            try
            {
                _db.SubmittedLocations.Add(data);
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }

            return Ok(data.Id);
        }

        [HttpGet("getPlanning")]
        public IActionResult GetPlanning(int userId)
        {
            DateTime twoPastHour = DateTime.Now.Add(new TimeSpan(-12, 0, 0));
            var plannings = _db.Plans
                .Where(a => a.UserId == userId &&
                            a.DateTime > twoPastHour)
                .Select(a => new { a.Id, a.UserId, a.DateTime, a.ShiftId, a.LocationId })
                .OrderBy(a=>a.DateTime)
                .ToList();
            var data = JsonConvert.SerializeObject(plannings);
            return Ok(data);
        }
    }

    public class ApiUser
    {
        public string Status;
        public string id;
        public string name;
    }
}
