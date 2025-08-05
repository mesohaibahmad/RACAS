using RACAS.Filters;
using RACAS.Model;
using RACAS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using RACAS.Models;
using RACAS.Constants;
using System.Collections.Generic;

namespace RACAS.Controllers
{
    [AuthorizeActionFilter]
    public class PartnersController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly IPartnersServices partnersServices;
        private readonly IStringLocalizer<HomeController> stringLocalizer;
        private readonly IUsersServices userServices;
        public PartnersController(IWebHostEnvironment environment, IConfiguration configuration
            , IPartnersServices _partnersServices, IStringLocalizer<HomeController> _stringLocalizer, IUsersServices _userServices)
        {

            this.environment = environment;
            _configuration = configuration;
            stringLocalizer = _stringLocalizer;
            partnersServices = _partnersServices;
            userServices = _userServices;
        }
        [Route("partner/index")]
        [Route("partner")]
        public async Task<IActionResult> Index()
        {
            var list = await partnersServices.GetAllPartners();
            return View(list);
        }

        [HttpPost]
        [Route("partner/insertPartner")]
        public async Task<IActionResult> Insert([FromBody] Partners model)
        {
            string obj = await partnersServices.InsertPartner(model);
            return Json(obj);
        }

        [HttpPost]
        [Route("partner/delete")]
        public async Task<IActionResult> Delete([FromBody] CommonId model)
        {
            string result = await partnersServices.DeletePartner(model.Id);
            return Json(result);

        }

        [HttpPost]
        [Route("partner/AssignBranches")]
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
                string result = await userServices.AssignBranches(model, "Partner");

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        [Route("partner/UnAssignBranches")]
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
                string result = await userServices.UnAssignBranches(model, "Partner");

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        [Route("partner/GetPartnerBranches")]
        public async Task<IActionResult> GetPartnerBranches([FromQuery] long PartnerId)
        {
            try
            {
                var result = await partnersServices.GetPartnerBranches(PartnerId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        
    }
}
