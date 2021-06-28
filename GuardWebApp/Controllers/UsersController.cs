﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuardWebApp.Models;
using GuardWebApp.Controllers.Utils;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

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


        public static string tt = "";

        [HttpGet("login")]
        public async Task<ActionResult<User>> Login(string username, string password)
        {
            string key = ApiUtilities._KEY;
            string tk = ApiUtilities.rndTransferKey();
            string p0 = ApiUtilities._P0;
            string p1 = ApiUtilities.EncryptString(username, key);
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

            ApiUser EncryptUserModel = JsonConvert.DeserializeObject<ApiUser>(result);

            string backTk = ApiUtilities.Reverse(ApiUtilities.DecryptString(EncryptUserModel.Status, key));
            ApiUser DecryptUserModel = new ApiUser();
            if (tk == backTk)
            {
                DecryptUserModel.id = ApiUtilities.DecryptString(EncryptUserModel.id, key);
                DecryptUserModel.name = ApiUtilities.DecryptString(EncryptUserModel.name, key);
                DecryptUserModel.Status = ApiUtilities.DecryptString(EncryptUserModel.Status, key);
                DecryptUserModel.IsGuard = ApiUtilities.DecryptString(EncryptUserModel.IsGuard, key);
                DecryptUserModel.IsGuardAdmin = ApiUtilities.DecryptString(EncryptUserModel.IsGuardAdmin, key);
                DecryptUserModel.IsEmployeeRequest = ApiUtilities.DecryptString(EncryptUserModel.IsEmployeeRequest, key);
                DecryptUserModel.IsGuardRecorder = ApiUtilities.DecryptString(EncryptUserModel.IsGuardRecorder, key);
                DecryptUserModel.IsMould = ApiUtilities.DecryptString(EncryptUserModel.IsMould, key);
                DecryptUserModel.token = ApiUtilities.DecryptString(EncryptUserModel.token, key);

                var currentUser = _context.Users.Where(a => a.Id == int.Parse(DecryptUserModel.id)).FirstOrDefault();

                if (currentUser != null)
                {
                    //check name
                    if (!currentUser.Name.Equals(DecryptUserModel.name))
                    {
                        currentUser.Name = DecryptUserModel.name;
                    }

                    //check pass
                    if (!currentUser.Password.Equals(ApiUtilities.sha512(password + ApiUtilities._SALT)))
                    {
                        currentUser.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
                    }

                    _context.Users.Update(currentUser);
                    _context.SaveChanges();

                    var apiresult = new ApiUser()
                    {
                        id = DecryptUserModel.id,
                        name = DecryptUserModel.name,
                        IsEmployeeRequest = DecryptUserModel.IsEmployeeRequest,
                        IsGuard = DecryptUserModel.IsEmployeeRequest,
                        IsGuardAdmin = DecryptUserModel.IsGuardAdmin,
                        IsGuardRecorder = DecryptUserModel.IsGuardRecorder,
                        IsMould = DecryptUserModel.IsMould,
                        token = DecryptUserModel.token,
                        Status = "true"
                    };
                    var data = JsonConvert.SerializeObject(apiresult);
                    return Ok(data);

                }
                else
                {
                    long userType = 1;
                    if (DecryptUserModel.IsGuardRecorder == "True")
                    {
                        userType = _context.UserTypes.FirstOrDefault(a => a.Type == "Recorder").Id;
                    }
                    else if (DecryptUserModel.IsGuardAdmin == "True")
                    {
                        userType = _context.UserTypes.FirstOrDefault(a => a.Type == "Admin").Id;
                    }
                    else
                    {
                        userType = _context.UserTypes.FirstOrDefault(a => a.Type == "Guard").Id;
                    }

                    User newUser = new User();

                    newUser.Id = Int64.Parse(DecryptUserModel.id);
                    newUser.Username = username;
                    newUser.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
                    newUser.Name = DecryptUserModel.name;
                    newUser.UserTypeId = userType;

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    var apiresult = new ApiUser()
                    {
                        id = DecryptUserModel.id,
                        name = DecryptUserModel.name,
                        IsEmployeeRequest = DecryptUserModel.IsEmployeeRequest,
                        IsGuard = DecryptUserModel.IsGuard,
                        IsGuardAdmin = DecryptUserModel.IsGuardAdmin,
                        IsGuardRecorder = DecryptUserModel.IsGuardRecorder,
                        IsMould = DecryptUserModel.IsMould,
                        token = DecryptUserModel.token,
                        Status = "true"
                    };
                    var data = JsonConvert.SerializeObject(apiresult);
                    return Ok(data);
                }

            }
            else
            {
                return new JsonResult(new { data = "false", uid = "" });
            }
        }

        [HttpGet("attend")]
        public async /*Task<ActionResult<List<clsAttendanceTime>>>*/ Task Attend(string userId, string guardId, string datetime)
        {
            string key = ApiUtilities._KEY;
            string tk = ApiUtilities.rndTransferKey();
            string p0 = ApiUtilities._P0;
            string p1 = ApiUtilities.EncryptString(userId, key);
            string p2 = ApiUtilities.EncryptString(tk, key);
            string p3 = ApiUtilities.EncryptString(guardId, key);
            string p4 = ApiUtilities.EncryptString(datetime, key);

            var theWebRequest = HttpWebRequest.Create("http://192.168.10.250/ExLogin.aspx/AT");
            theWebRequest.Method = "POST";
            theWebRequest.ContentType = "application/json; charset=utf-8";
            theWebRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");

            using (var writer = theWebRequest.GetRequestStream())
            {
                string send = null;
                send = "{\"p0\":\"" + p0 + "\",\"p1\":\"" + p1 + "\",\"p2\":\"" + p2 + "\",\"p3\":\"" + p3 + "\",\"p4\":\"" + p4 + "\"}";

                var data = Encoding.UTF8.GetBytes(send);

                writer.Write(data, 0, data.Length);
            }

            var theWebResponse = (HttpWebResponse)theWebRequest.GetResponse();
            var theResponseStream = new StreamReader(theWebResponse.GetResponseStream());

            string result = theResponseStream.ReadToEnd();

            string asdas = result;

            //try
            //{
            //    result = "{" + result.Substring(28).Replace("}}", "}");
            //}
            //catch (Exception e)
            //{
            //    var apiresult = new ApiUser() { id = "", name = "", Status = "false" };
            //    var data = JsonConvert.SerializeObject(apiresult);
            //    return Ok(data);
            //}

            //var splashInfo = JsonConvert.DeserializeObject<ApiUser>(result);

            //string backTk = ApiUtilities.DecryptString(splashInfo.Status, key);
            //if (tk == ApiUtilities.Reverse(backTk))
            //{
            //    splashInfo.id = ApiUtilities.DecryptString(splashInfo.id, key);
            //    splashInfo.name = ApiUtilities.DecryptString(splashInfo.name, key);
            //    splashInfo.Status = ApiUtilities.DecryptString(splashInfo.Status, key);

            //    var currentUser = _context.Users.Where(a => a.Id == int.Parse(splashInfo.id)).FirstOrDefault();

            //    if (currentUser != null)
            //    {
            //        //check name
            //        if (!currentUser.Name.Equals(splashInfo.name))
            //        {
            //            currentUser.Name = splashInfo.name;
            //        }

            //        //check pass
            //        if (!currentUser.Password.Equals(ApiUtilities.sha512(password + ApiUtilities._SALT)))
            //        {
            //            currentUser.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
            //        }

            //        _context.Users.Update(currentUser);
            //        _context.SaveChanges();

            //        var apiresult = new ApiUser() { id = splashInfo.id, name = splashInfo.name, Status = "true" };
            //        var data = JsonConvert.SerializeObject(apiresult);
            //        return Ok(data);

            //    }
            //    else
            //    {
            //        User t = new User();

            //        t.Id = int.Parse(splashInfo.id);
            //        t.Username = username;
            //        t.Password = ApiUtilities.sha512(password + ApiUtilities._SALT);
            //        t.Name = splashInfo.name;
            //        t.Type = "guard";

            //        _context.Users.Add(t);
            //        _context.SaveChanges();

            //        var apiresult = new ApiUser() { id = splashInfo.id, name = splashInfo.name, Status = "true" };
            //        var data = JsonConvert.SerializeObject(apiresult);
            //        return Ok(data);
            //    }

            //}
            //else
            //{
            //    return new JsonResult(new { data = "false", uid = "" });
            //}
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

        public class clsAttendanceTime
        {
            public string StartDate;
            public string EndDate;
            public string leave;
        }
    }
}
