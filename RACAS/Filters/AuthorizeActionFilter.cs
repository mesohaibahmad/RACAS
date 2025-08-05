using RACAS.Model;
using RACAS.Models;
using RACAS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Filters
{
    public class AuthorizeActionFilter : ActionFilterAttribute
    {
       
        public AuthorizeActionFilter()
        {
          
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            long UserloginId = 0;
            if (!string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("UserTypeId")))
            {
                UserloginId = Convert.ToInt64(filterContext.HttpContext.Session.GetString("UserTypeId"));
            }
            var dBContext = filterContext.HttpContext.RequestServices.GetRequiredService<RACASContext>();
            Controller controller = filterContext.Controller as Controller;

            var list = from um in dBContext.UserModules
                       join m in dBContext.Modules on um.ModuleId equals m.Id
                       join a in dBContext.ModuleActions on um.ActionId equals a.Id into abc
                       from action in abc.DefaultIfEmpty()
                       where um.UserTypeId == UserloginId
                       select new MenuViewModel
                       {
                           ActionMethod = m.ActionMethod,
                           Controller = m.ControllerName,
                           MenuIcon = m.MenuIcon,
                           ModuleId = m.ModuleId,
                           ActionId = um.ActionId,
                           ActionName = action == null ? "" : action.ActionName,
                           ModuleName = m.ModuleName
                       };


            controller.ViewData["UserRoleMenu"] = list.ToList();
        }
    }
}
