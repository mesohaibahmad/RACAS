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

namespace RACAS.Controllers
{
    [AuthorizeActionFilter]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly ICommonServices commonServices;
        private readonly IStringLocalizer<HomeController> stringLocalizer;
        public HomeController(IWebHostEnvironment environment, IConfiguration configuration
            , ICommonServices _commonServices, IStringLocalizer<HomeController> _stringLocalizer)
        {

            this.environment = environment;
            _configuration = configuration;
            commonServices = _commonServices;
            stringLocalizer = _stringLocalizer;
        }
        [Route("home/index")]
        public async Task<IActionResult> Index()
        {
           

          //  var obj=await commonServices.TotalUserCounts();
          //  ViewData["TotalUsers"] = obj;
            return View();
        }

        [HttpGet]
        [Route("home/LoadValues")]
        public async Task<IActionResult> LoadValues(int year, int month)
        {
            try
            {
                var list = await commonServices.DashboardData( year,  month);

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("home/ChangeLanguage")]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
