using RACAS.Services;
using JustPayApi.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using RACAS.Models;

namespace RACAS.Controllers
{
    public class UserLoginController : Controller
    {


        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly IUsersServices userServices;
        public UserLoginController(IWebHostEnvironment environment, IConfiguration configuration, IUsersServices _userServices)
        {
          
            this.environment = environment;
            _configuration = configuration;
            userServices = _userServices;
        }

        [Route("userlogin/Index")]
        public IActionResult Index()
        {


            return View();
        }
       

        [Route("userlogin/login")]
        public IActionResult login()
        {
            var userid=HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("index", "home");
            }

            return View("Index");
        }
        [Route("userlogin/logout")]
        public IActionResult logout()
        {

            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("FirstName");
            HttpContext.Session.Remove("LastName");
            HttpContext.Session.Clear();

            return View("Index");
        }

        [HttpPost]
        [Route("UserLogin/LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
        {
            try
            {
                var user = await userServices.UserLogin(model.UserName, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("UserTypeId", Convert.ToString(user.UserTypeId));
                    HttpContext.Session.SetString("UserId", Convert.ToString(user.Id));

                    return Ok(user);
                }
                return Ok(new { success = false, message = "Invalid Username or Password." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          

        }


    }
    public class ADAccessLoginModel
    {
        public string AccessToken { get; set; }
    }
}
