using RACAS.Filters;
using RACAS.Model;
using RACAS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RACAS.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RACAS.Models;

namespace RACAS.Controllers
{
    [AuthorizeActionFilter]
    public class UsersController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly IUsersServices userServices;
        private readonly ICommonServices commonServices;
        private readonly IBranchServices branchServices;
        public UsersController(IWebHostEnvironment environment,
            IConfiguration configuration, IUsersServices _userServices,
            ICommonServices _commonServices, IBranchServices branchServices)
        {

            this.environment = environment;
            _configuration = configuration;
            userServices = _userServices;
            commonServices = _commonServices;
            this.branchServices = branchServices;
        }
        [Route("Users")]
        [Route("Users/index")]
        public async Task<IActionResult> Index(int UserId=0)
        {
            var look = await userServices.GetUserType();
            var list2 = await userServices.GetList();

            UserDetails detail = new UserDetails();
            detail.Users = list2;
            detail.ModuleList = await userServices.GetModuleList();
            detail.UserBranchList = await userServices.GetUserTypeBranches(UserId, "User");
            detail.BranchList = await this.branchServices.GetBranches();
            if (detail.BranchList != null && detail.UserBranchList != null)
            {
                var ids = detail.UserBranchList.Select(x => x.BranchId).ToList();
                detail.BranchList = detail.BranchList.Where(x => !ids.Contains(x.Id)).ToList();
            }
            var objresult = await userServices.SelectedItems(UserId);

            if (list2?.Count > 0)
            {
                detail.UserData = list2.Where(x => x.Id == UserId).FirstOrDefault();
            }
            else
            {
                detail.UserData = new Models.User()
                {
                    Id = 0,
                    FirstName = "",
                    LastName="",
                    UserName="",
                    Password="",
                    Email="",
                    UserTypeId=0
                };
            }

            detail.Selected = string.Join(",", objresult);


            ViewData["LookupList"] = look;
            return View(detail);
        }

        [Route("Users/Detail")]
        public async Task<IActionResult> Detail()
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            var UserId = CommonDataParam.LoginId = LoginId;

            var user = await userServices.GetUser(UserId);
            return View(user);
        }

        [Route("Users/UserTypes")]
        public async Task<IActionResult> UserTypes(int UserTypeId = 0)
        {
            UserDetails detail = new UserDetails();
            var look = await userServices.GetUserType();
           detail.UserTypeList = look;
            detail.ModuleList = await userServices.GetModuleList();
            detail.UserBranchList = await userServices.GetUserTypeBranches(UserTypeId,"User");
            detail.BranchList = await this.branchServices.GetBranches();
            if (detail.BranchList != null && detail.UserBranchList!=null)
            {
                var ids= detail.UserBranchList.Select(x => x.BranchId).ToList();
                detail.BranchList = detail.BranchList.Where(x => !ids.Contains(x.Id)).ToList();
            }
            var objresult = await userServices.SelectedItems(UserTypeId);

            if (look != null && UserTypeId > 0)
            {
                detail.UserType = look.Where(x => x.UserTypeId == UserTypeId).FirstOrDefault();
            }
            else
            {
                detail.UserType = new Models.UserTypes()
                {
                    Id = 0,
                    TypeName = ""
                };
            }

            detail.Selected = string.Join(",", objresult);

            return View(detail);
        }

        [HttpPost]
        [Route("Users/insertType")]
        public async Task<IActionResult> insertType([FromBody] UserTypeModel model)
        {
            try
            {
                long LoginId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                {
                    LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                }
                CommonDataParam.LoginId = LoginId;
                string result = await userServices.InsertUserType(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        [Route("Users/AssignBranches")]
        public async Task<IActionResult> AssignBranches([FromBody] List<UserBranchesModel> model)
        {
            try
            {
                long LoginId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                {
                    LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                }
                CommonDataParam.LoginId = LoginId;
                string result = await userServices.AssignBranches(model,"User");

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        [Route("Users/UnAssignBranches")]
        public async Task<IActionResult> UnAssignBranches([FromBody] List<UserBranchesModel> model)
        {
            try
            {
                long LoginId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                {
                    LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                }
                CommonDataParam.LoginId = LoginId;
                string result = await userServices.UnAssignBranches(model, "User");

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        [HttpPost]
        [Route("Users/insert")]
        public async Task<IActionResult> insert([FromBody] UserInputModel model)
        {
           

            try
            {
                long LoginId = 0;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                {
                    LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                }
                CommonDataParam.LoginId = LoginId;
                string result = await userServices.Insert(model);
                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }



        }

        [HttpPost]
        [Route("Users/EditUser")]
        public async Task<IActionResult> Edit([FromBody] UserInputModel model)
        {


            try
            {
              
                string result = await userServices.EditUser(model);
                return Json(new {success = true, message =  "User Modified"});

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }



        }

        //public string ToHtmlTable<UserInputModel>(List<UserInputModel> list, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle, string propertiesToIncludeAsColumns = "")
        //{
        //    dynamic result = new StringBuilder();
        //    if (String.IsNullOrEmpty(tableSyle))
        //    {
        //        result.Append("<table id=\"" + typeof(UserInputModel).Name + "Table\">");
        //    }
        //    else
        //    {
        //        result.Append((Convert.ToString("<table id=\"" + typeof(UserInputModel).Name + "Table\" class=\"") + tableSyle) + "\">");
        //    }

        //    var propertyArray = typeof(UserInputModel).GetProperties();

        //    foreach (var prop in propertyArray)
        //    {
        //        if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(prop.Name + ","))
        //        {
        //            if (String.IsNullOrEmpty(headerStyle))
        //            {
        //                result.AppendFormat("<th>{0}</th>", prop.Name);
        //            }
        //            else
        //            {
        //                result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, prop.Name);
        //            }
        //        }
        //    }

        //    for (int i = 0; i <= list.Count() - 1; i++)
        //    {
        //        if (!String.IsNullOrEmpty(rowStyle) && !String.IsNullOrEmpty(alternateRowStyle))
        //        {
        //            result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);

        //        }
        //        else
        //        {
        //            result.AppendFormat("<tr>");
        //        }
        //        foreach (var prop in propertyArray)
        //        {
        //            if (string.IsNullOrEmpty(propertiesToIncludeAsColumns) || propertiesToIncludeAsColumns.Contains(prop.Name + ","))
        //            {
        //                object value = prop.GetValue(list.ElementAt(i), null);
        //                result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
        //            }
        //        }
        //        result.AppendLine("</tr>");
        //    }

        //    result.Append("</table>");

        //    return result.ToString();
        //}
        [HttpGet]
        [Route("Users/delete")]
        public async Task<IActionResult> delete([FromQuery] long UserId)
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            CommonDataParam.LoginId = LoginId;
            var obj = await userServices.delete(UserId);
            return Ok(obj);

        }
        [HttpGet]
        [Route("Users/deleteUserType")]
        public async Task<IActionResult> deleteUserType([FromQuery] long UserTypeId)
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            CommonDataParam.LoginId = LoginId;
            var obj = await userServices.deleteUserType(UserTypeId);
            return Ok(obj);

        }
        [Route("Users/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            
            return View();
        }
    }
}
