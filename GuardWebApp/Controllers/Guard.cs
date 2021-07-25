using GuardWebApp.Controllers.Utils;
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

        //[HttpGet("login")]
        //public async Task<ActionResult<User>> Login(string user, string pass)
        //{
        //    string key = ApiUtilities._KEY;
        //    string tk = ApiUtilities.rndTransferKey();
        //    string p0 = ApiUtilities._P0;

        //    string p1 = ApiUtilities.EncryptString(user, key);
        //    string p2 = ApiUtilities.EncryptString(pass, key);
        //    string p3 = ApiUtilities.EncryptString(tk, key);

        //    var theWebRequest = HttpWebRequest.Create("http://192.168.10.250/ExLogin.aspx/LI");
        //    theWebRequest.Method = "POST";
        //    theWebRequest.ContentType = "application/json; charset=utf-8";
        //    theWebRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

        //    using (var writer = theWebRequest.GetRequestStream())
        //    {
        //        string send = null;
        //        send = "{\"p0\":\"" + p0 + "\",\"p1\":\"" + p1 + "\",\"p2\":\"" + p2 + "\",\"p3\":\"" + p3 + "\"}";

        //        var data = Encoding.UTF8.GetBytes(send);

        //        writer.Write(data, 0, data.Length);
        //    }

        //    var theWebResponse = (HttpWebResponse)theWebRequest.GetResponse();
        //    var theResponseStream = new StreamReader(theWebResponse.GetResponseStream());

        //    string result = theResponseStream.ReadToEnd();

        //    try
        //    {
        //        result = "{" + result.Substring(28).Replace("}}", "}");
        //    }
        //    catch (Exception e)
        //    {
        //        var apiresult = new ApiUser() { id = "", name = "", Status = "false" };
        //        var data = JsonConvert.SerializeObject(apiresult);
        //        return Ok(data);
        //    }

        //    ApiUser EncryptUserModel = JsonConvert.DeserializeObject<ApiUser>(result);

        //    string backTk = ApiUtilities.Reverse(ApiUtilities.DecryptString(EncryptUserModel.Status, key));
        //    ApiUser DecryptUserModel = new ApiUser();
        //    if (tk == backTk)
        //    {
        //        DecryptUserModel.id = ApiUtilities.DecryptString(EncryptUserModel.id, key);
        //        DecryptUserModel.name = ApiUtilities.DecryptString(EncryptUserModel.name, key);
        //        DecryptUserModel.Status = ApiUtilities.DecryptString(EncryptUserModel.Status, key);
        //        DecryptUserModel.IsGuard = ApiUtilities.DecryptString(EncryptUserModel.IsGuard, key);
        //        DecryptUserModel.IsGuardAdmin = ApiUtilities.DecryptString(EncryptUserModel.IsGuardAdmin, key);
        //        DecryptUserModel.IsEmployeeRequest = ApiUtilities.DecryptString(EncryptUserModel.IsEmployeeRequest, key);
        //        DecryptUserModel.IsGuardRecorder = ApiUtilities.DecryptString(EncryptUserModel.IsGuardRecorder, key);
        //        DecryptUserModel.IsMould = ApiUtilities.DecryptString(EncryptUserModel.IsMould, key);
        //        DecryptUserModel.token = ApiUtilities.DecryptString(EncryptUserModel.token, key);

        //        var currentUser = _db.Users.Where(a => a.Id == int.Parse(DecryptUserModel.id)).FirstOrDefault();

        //        if (currentUser != null)
        //        {
        //            //check name
        //            if (!currentUser.Name.Equals(DecryptUserModel.name))
        //            {
        //                currentUser.Name = DecryptUserModel.name;
        //            }

        //            //check pass
        //            if (!currentUser.Password.Equals(ApiUtilities.sha512(pass + ApiUtilities._SALT)))
        //            {
        //                currentUser.Password = ApiUtilities.sha512(pass + ApiUtilities._SALT);
        //            }

        //            //token
        //            currentUser.Token = DecryptUserModel.token;

        //            _db.Users.Update(currentUser);
        //            _db.SaveChanges();

        //            var apiresult = new ApiUser()
        //            {
        //                id = DecryptUserModel.id,
        //                name = DecryptUserModel.name,
        //                IsEmployeeRequest = DecryptUserModel.IsEmployeeRequest,
        //                IsGuard = DecryptUserModel.IsEmployeeRequest,
        //                IsGuardAdmin = DecryptUserModel.IsGuardAdmin,
        //                IsGuardRecorder = DecryptUserModel.IsGuardRecorder,
        //                IsMould = DecryptUserModel.IsMould,
        //                token = DecryptUserModel.token,
        //                Status = "true"
        //            };
        //            var data = JsonConvert.SerializeObject(apiresult);

        //            return Ok(data);

        //        }
        //        else
        //        {
        //            long userType = 1;
        //            if (DecryptUserModel.IsGuardRecorder == "True")
        //            {
        //                userType = _db.UserTypes.FirstOrDefault(a => a.Type == "Recorder").Id;
        //            }
        //            else if (DecryptUserModel.IsGuardAdmin == "True")
        //            {
        //                userType = _db.UserTypes.FirstOrDefault(a => a.Type == "Admin").Id;
        //            }
        //            else
        //            {
        //                userType = _db.UserTypes.FirstOrDefault(a => a.Type == "Guard").Id;
        //            }

        //            User newUser = new User();

        //            newUser.Id = Int64.Parse(DecryptUserModel.id);
        //            newUser.Username = user;
        //            newUser.Password = ApiUtilities.sha512(pass + ApiUtilities._SALT);
        //            newUser.Name = DecryptUserModel.name;
        //            newUser.UserTypeId = userType;
        //            newUser.Token = DecryptUserModel.token;

        //            _db.Users.Add(newUser);
        //            _db.SaveChanges();

        //            var apiresult = new ApiUser()
        //            {
        //                id = DecryptUserModel.id,
        //                name = DecryptUserModel.name,
        //                IsEmployeeRequest = DecryptUserModel.IsEmployeeRequest,
        //                IsGuard = DecryptUserModel.IsGuard,
        //                IsGuardAdmin = DecryptUserModel.IsGuardAdmin,
        //                IsGuardRecorder = DecryptUserModel.IsGuardRecorder,
        //                IsMould = DecryptUserModel.IsMould,
        //                token = DecryptUserModel.token,
        //                Status = "true"
        //            };
        //            var data = JsonConvert.SerializeObject(apiresult);
        //            return Ok(data);
        //        }

        //    }
        //    else
        //    {
        //        return new JsonResult(new { data = "false", uid = "" });
        //    }
        //}

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
        public string IsGuard;
        public string IsGuardAdmin;
        public string IsEmployeeRequest;
        public string IsGuardRecorder;
        public string IsMould;
        public string token;
    }
}
