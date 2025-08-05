using RACAS.Filters;
using RACAS.Models;
using RACAS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACAS.Model;
using Microsoft.AspNetCore.Http;
using RACAS.Constants;

namespace RACAS.Controllers
{
    [AuthorizeActionFilter]
    public class LookupController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly ILookupServices lookupServices;
        private readonly IUsersServices userServices;
        private readonly ICommonServices commonServices;

        public LookupController( IWebHostEnvironment environment, IConfiguration configuration, ILookupServices _lookupServices, IUsersServices _userServices, ICommonServices _commonServices)
        {

            this.environment = environment;
            _configuration = configuration;
            lookupServices = _lookupServices;
            userServices = _userServices;
            commonServices = _commonServices;
        }
        [Route("Lookup")]
        [Route("Lookup/index")]
        public async Task<IActionResult> Index()
        {
            var list = await lookupServices.GetLookups();
            return View(list);
        }

        [HttpPost]
        [Route("Lookup/insertCountry")]
        public async Task<IActionResult> insertCountry([FromBody] Countries model)
        {
            try
            {
                var obj = await lookupServices.InsertCountry(model);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("Lookup/insertBranch")]
        public async Task<IActionResult> insertBranch([FromBody] Branches model)
        {
            try
            {
                var obj = await lookupServices.InsertBranch(model);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("Lookup/insertCausedBy")]
        public async Task<IActionResult> insertCausedBy([FromBody] CausedBy model)
        {
            try
            {
                var obj = await lookupServices.InsertCausedBy(model);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("Lookup/insertDescription")]
        public async Task<IActionResult> insertDescription([FromBody] Descriptions model)
        {
            try
            {
                var obj = await lookupServices.InsertDescription(model);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      

        [HttpPost]
        [Route("Lookup/delete")]
        public async Task<IActionResult> delete([FromBody] CommonId model)
        {
            string result = await lookupServices.delete(model.Id);
            return Json(result);

        }

        [HttpPost]
        [Route("Lookup/deleteBranch")]
        public async Task<IActionResult> deleteBranch([FromBody] CommonId model)
        {
            string result = await lookupServices.deleteBranch(model.Id);
                return Json(result);

        }

        [HttpPost]
        [Route("Lookup/deleteCausedBy")]
        public async Task<IActionResult> deleteCausedBy([FromBody] CommonId model)
        {
            string result = await lookupServices.deleteCausedBy(model.Id);
            return Json(result);

        }

        [HttpPost]
        [Route("Lookup/deleteDescription")]
        public async Task<IActionResult> deleteDescription([FromBody] CommonId model)
        {
            string result = await lookupServices.deleteDescription(model.Id);
            return Json(result);

        }

        [HttpGet]
        [Route("Lookup/deleteUserType")]
        public async Task<IActionResult> deleteUserType([FromQuery] long UserTypeId)
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            CommonDataParam.LoginId = LoginId;
            var obj = await lookupServices.deleteUserType(UserTypeId);
            return Ok(obj);

        }


    }
}
