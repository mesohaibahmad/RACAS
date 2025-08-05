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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RACAS.Constants;
using iText.Commons.Actions.Contexts;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using iText.Kernel.Geom;
using MailKit.Search;

namespace RACAS.Controllers
{
    [AuthorizeActionFilter]
    public class PaymentsController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _configuration;
        private readonly ILookupServices lookupServices;
        private readonly IPartnersServices partnerServices;
        private readonly IPaymentServices paymentServices;

        public PaymentsController( IWebHostEnvironment environment, IConfiguration configuration, ILookupServices _lookupServices
            , IPartnersServices _partnerServices, IPaymentServices _paymentServices)
        {

            this.environment = environment;
            _configuration = configuration;
            lookupServices = _lookupServices;
            this.partnerServices = _partnerServices;
            paymentServices = _paymentServices;

        }

        [Route("Payments")]
        [Route("Payments/index")]
        public async Task<IActionResult> Requests()
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            CommonDataParam.LoginId = LoginId;

            string userName = $"{HttpContext.Session.GetString("FirstName")} {HttpContext.Session.GetString("LastName")}";
            ViewBag.UserName = userName;

            PaymentModel paymentModel = await paymentServices.GetAllPaymentObjects();

            return View(paymentModel);
        }
        [Route("Payments/Detail")]
        public async Task<IActionResult> Detail(long PaymentId)
        {
            if (PaymentId <= 0)
            {
                return BadRequest("Invalid PaymentId.");
            }

            ViewBag.PaymentId = PaymentId;

            return View();



        }

        [HttpGet]
        [Route("payments/getLogDetails")]
        public async Task<IActionResult> AllLogRequests(long PaymentId)
        {
            try
            {
                var paymentModel = await paymentServices.GetPaymentDetails(PaymentId);
                var LogModel = await paymentServices.GetLogDetails(PaymentId);

                return Ok(new
                {
                    LogDetails = LogModel,
                    PaymentDetails = paymentModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("payments/AllPaymentRequests")]
        public async Task<IActionResult> AllPaymentRequests(string SearchText, string SortColumn, string SortOrder, int PageIndex, int PageSize,
            string RecordStatus, int SubmittedById, int ControlCheckById, int ApprovedById, string PaymentType, int PartnerId, string InvoiceNumber)
        {
            try
            {
                var paymentModel = await paymentServices.GetAllPayments(SearchText, SortColumn, SortOrder, PageIndex, PageSize, RecordStatus, SubmittedById
                    , ControlCheckById, ApprovedById, PaymentType, PartnerId, InvoiceNumber);

                return Ok(paymentModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("payments/insertPayment")]
        public async Task<IActionResult> Insert([FromBody] MainLedger model)
        {
            model.CostIncuredDate = model.CostIncuredDate.Date; // Set only the date part


            long result = await paymentServices.InsertPayment(model);
            if (result > 0)
                return Ok(result);
            else
                return BadRequest(result);

        }
        [HttpPost]
        [Route("payments/MultiRowsAction")]
        public async Task<IActionResult> MultiRowsAction([FromBody] EventLogModel model)
        {
            long LoginId = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                LoginId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            CommonDataParam.LoginId = LoginId;

            string result = await paymentServices.MultiRowsAction(model);
            return Json(result);

        }


       
        [HttpPost]
        [Route("Payments/insertBranch")]
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
        [Route("Payments/delete")]
        public async Task<IActionResult> Delete([FromBody] CommonId model)
        {
            string result = await paymentServices.DeletePayment(model.Id);
            if (result == "success")
            {
                return Ok(new { message = "User deleted successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to delete the user" });
            }

        }

        [HttpPost]
        [Route("Payments/deleteBranch")]
        public async Task<IActionResult> deleteBranch([FromBody] CommonId model)
        {
            string result = await lookupServices.deleteBranch(model.Id);
            if (result == "success")
                return Ok(result);
            else
                return BadRequest(result);

        }


    }
}
