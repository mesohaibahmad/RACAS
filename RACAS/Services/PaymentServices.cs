using iText.Commons.Actions.Contexts;
using iText.Kernel.Geom;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RACAS.Constants;
using RACAS.DAL;
using RACAS.Model;
using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace RACAS.Services
{
    public interface IPaymentServices
    {
        Task<PaymentModel> GetAllPaymentObjects();
        Task<List<Dictionary<string, object>>> GetPaymentDetails(long PaymentId);
        Task<List<Dictionary<string, object>>> GetLogDetails(long PaymentId);

        Task<List<Dictionary<string, object>>> GetAllPayments(string SearchText, string SortColumn, string SortOrder, int PageIndex, int PageSize
            , string RecordStatus, int SubmittedById, int ControlCheckById, int ApprovedById, string PaymentType, int PartnerId, string InvoiceNumber);
        Task<long> InsertPayment(MainLedger param);
       
        Task<string> DeletePayment(int Id);
        Task<string> MultiRowsAction(EventLogModel param);
    }

    public class PaymentServices : IPaymentServices
    {
        private readonly RACASContext dBContext;

        public PaymentServices(RACASContext _dbContext)
        {
            dBContext = _dbContext;
        }

        public async Task<PaymentModel> GetAllPaymentObjects()
        {

            try
            {

                PaymentModel objectModel = new PaymentModel();
                //try
                //{
                //    objectModel.PaymentRequests = await dBContext.MainLedger.ToListAsync();
                //}
                //catch (SqlException sqlEx)
                //{
                //    Console.WriteLine($"SQL Error: {sqlEx.Message}");

                //    // Optionally, log more details
                //    foreach (SqlError error in sqlEx.Errors)
                //    {
                //        Console.WriteLine($"Error: {error.Message}");
                //    }
                //}

                objectModel.CountryList = await dBContext.Countries.ToListAsync();
                objectModel.BranchList = await dBContext.Branches.ToListAsync();
                objectModel.PartnerList = await dBContext.Partners.ToListAsync();
                objectModel.UsersList = await dBContext.Users.ToListAsync();
                objectModel.UserBranchList = await dBContext.UserBranches.ToListAsync();
                objectModel.CausedByList = await dBContext.CausedBy.ToListAsync();
                objectModel.DescriptionList = await dBContext.Descriptions.ToListAsync();


                return objectModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<Dictionary<string, object>>> GetPaymentDetails(long PaymentId)
        {
            BaseDataAccess baseDataAccess = new BaseDataAccess();

            List<SqlParameter> listparam = new List<SqlParameter>();
            listparam.Add(new SqlParameter { ParameterName = "@PaymentId", Value = PaymentId });


            var rd = baseDataAccess.GetDataReaderToDictionary("SpGetOnePayment", listparam);

            return rd;

        }
        public async Task<List<Dictionary<string, object>>> GetLogDetails(long PaymentId)
        {
            BaseDataAccess baseDataAccess = new BaseDataAccess();

            List<SqlParameter> listparam = new List<SqlParameter>();
            listparam.Add(new SqlParameter { ParameterName = "@PaymentId", Value = PaymentId});
         

            var rd = baseDataAccess.GetDataReaderToDictionary("SpGetDescription", listparam);

            return rd;

        }

        public async Task<List<Dictionary<string, object>>> GetAllPayments(string SearchText, string SortColumn, string SortOrder, int PageIndex, int PageSize
            , string RecordStatus, int SubmittedById, int ControlCheckById, int ApprovedById, string PaymentType, int PartnerId, string InvoiceNumber)
        {
            BaseDataAccess baseDataAccess = new BaseDataAccess();
            List<SqlParameter> listparam = new List<SqlParameter>();
            listparam.Add(new SqlParameter { ParameterName = "@SearchText", Value = SearchText });
            listparam.Add(new SqlParameter { ParameterName = "@SortColumn", Value = SortColumn });
            listparam.Add(new SqlParameter { ParameterName = "@SortOrder", Value = SortOrder });
            listparam.Add(new SqlParameter { ParameterName = "@PageIndex", Value = PageIndex });
            listparam.Add(new SqlParameter { ParameterName = "@PageSize", Value = PageSize });
            listparam.Add(new SqlParameter { ParameterName = "@RecordStatus", Value = RecordStatus });
            listparam.Add(new SqlParameter { ParameterName = "@SubmittedById", Value = SubmittedById });
            listparam.Add(new SqlParameter { ParameterName = "@ControlCheckById", Value = ControlCheckById });
            listparam.Add(new SqlParameter { ParameterName = "@ApprovedById", Value = ApprovedById });
            listparam.Add(new SqlParameter { ParameterName = "@PaymentType", Value = PaymentType });
            listparam.Add(new SqlParameter { ParameterName = "@PartnerId", Value = PartnerId });
            listparam.Add(new SqlParameter { ParameterName = "@InvoiceNumber", Value = InvoiceNumber });

            var rd = baseDataAccess.GetDataReaderToDictionary("SpGetAllPaymentRequests", listparam);

            return rd;

        }


        public async Task<long> InsertPayment(MainLedger param)
        {
            try
            {
                if (param.Id > 0)
                {
                    var obj = await dBContext.MainLedger.FindAsync(param.Id);
                    if (obj != null)
                    {
                        // Update existing payment record
                        obj.ContractNumber = param.ContractNumber;
                        obj.PartnerId = param.PartnerId;
                        obj.UserId = param.UserId;
                        obj.BranchId = param.BranchId;
                        obj.CausedById = param.CausedById;
                        obj.DescriptionId = param.DescriptionId;
                        obj.Comment = param.Comment.Trim();
                        obj.EntryDate = param.EntryDate == DateTime.MinValue ? DateTime.UtcNow : param.EntryDate;
                        obj.CostIncuredDate = param.CostIncuredDate == DateTime.MinValue ? DateTime.UtcNow : param.CostIncuredDate;
                        obj.InvoiceNumber = param.InvoiceNumber;
                        obj.Amount = param.Amount;
                        obj.OrderedById = param.OrderedById;
                        obj.IsSaved = param.IsSaved;
                        if (param.IsSubmitted)
                        {
                            obj.RecordStatus = "Pending";
                            obj.SubmittedBy = CommonDataParam.LoginId;
                            obj.SubmittedDate = param.SubmittedDate == DateTime.MinValue ? DateTime.UtcNow : param.SubmittedDate;
                        }
                        else
                        {
                            obj.SubmittedBy = 0;
                        }
                        obj.IsSubmitted = param.IsSubmitted;
                        
                        obj.IsControlCheck = param.IsControlCheck;
                        obj.ControlCheckBy = param.ControlCheckBy;
                        obj.ControlCheckDate = param.ControlCheckDate == DateTime.MinValue ? DateTime.UtcNow : param.ControlCheckDate;
                        obj.PaymentApprovalBy = param.PaymentApprovalBy;
                        obj.PaymentApprovalDate = param.PaymentApprovalDate == DateTime.MinValue ? DateTime.UtcNow : param.PaymentApprovalDate;
                        obj.PaymentType = param.PaymentType;
                        obj.ModifiedDate = DateTime.UtcNow;
                        obj.ModifiedBy = CommonDataParam.LoginId;

                        var payment = dBContext.MainLedger.Update(obj);
                        await dBContext.SaveChangesAsync();

                     

                        return param.Id;
                    }

                }
                else
                {
                    // Insert new payment record
                    param.EntryDate = DateTime.UtcNow;
                    param.CostIncuredDate = param.CostIncuredDate == DateTime.MinValue ? DateTime.UtcNow : param.CostIncuredDate;
                    
                    if(param.IsSubmitted){
                        param.SubmittedBy = CommonDataParam.LoginId;
                       
                    }

                    if (param.PaymentType == "Expense")
                    {
                        param.Amount = (param.Amount * -1);
                    }

                    param.SubmittedDate = DateTime.UtcNow;
                    param.IsControlCheck = param.IsControlCheck;
                    param.ControlCheckBy = param.IsControlCheck? CommonDataParam.LoginId:0;
                    param.ControlCheckDate = DateTime.UtcNow;
                    param.PaymentApprovalBy = param.IsApproved ? CommonDataParam.LoginId : 0;
                    param.IsApproved = param.IsApproved;
                    param.PaymentApprovalDate = DateTime.UtcNow;

                    param.RejectedBy = (param.IsRejected??false) ? CommonDataParam.LoginId : 0;
                    param.IsRejected = param.IsRejected;
                    param.RejectedDate = DateTime.UtcNow;


                    param.RecordStatus = param.RecordStatus;
                    param.OrderedByDate = DateTime.UtcNow;
                    param.CreatedBy = CommonDataParam.LoginId;
                    param.CreatedDate = DateTime.UtcNow;
                    param.ModifiedDate = DateTime.UtcNow;
                    param.ModifiedBy= CommonDataParam.LoginId;
                    param.IsOrdered =param.OrderedById>0? true:false;

                   var payment =  dBContext.MainLedger.Add(param);
                    await dBContext.SaveChangesAsync();

                    var newPaymentId = payment.Entity.Id;

                    return newPaymentId;
                }
                return -1;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

        }

       
        public async Task<string> DeletePayment(int Id)
        {
            try
            {
                var obj = await dBContext.MainLedger.FirstOrDefaultAsync(x => x.Id == Id);
                if (obj != null)
                {
                    dBContext.MainLedger.Remove(obj);
                    await dBContext.SaveChangesAsync();
                    return "success";
                }
                return "Payment not found.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> MultiRowsAction(EventLogModel param)
        {
            try
            {
                var _list = await dBContext.MainLedger.Where(x => param.Ids.Contains(x.Id)).ToListAsync();
                if (param.EventType == "Delete")
                {
                    foreach (var item in _list)
                    {
                        item.RecordStatus = "Deleted";
                    }
                   
                }
                else if (param.EventType == "Order")
                {
                    foreach (var item in _list)
                    {
                        item.OrderedByDate =DateTime.UtcNow;
                        item.OrderedById = CommonDataParam.LoginId;
                        item.IsOrdered = true;
                        item.RecordStatus = "Ordered";
                    }
                }
                else if (param.EventType == "Submit")
                {
                    foreach (var item in _list)
                    {
                        item.SubmittedDate = DateTime.UtcNow;
                        item.SubmittedBy = CommonDataParam.LoginId;
                        item.IsSubmitted = true;
                        item.IsSaved = false;
                    }
                }
                else if (param.EventType == "ControlCheck")
                {
                    foreach (var item in _list)
                    {
                        if(item.IsSubmitted){
                            item.ControlCheckDate = DateTime.UtcNow;
                            item.ControlCheckBy = CommonDataParam.LoginId;
                            item.IsControlCheck = true;
                            item.RecordStatus = "Control Check Ok";
                        }
                    }
                }
                else if (param.EventType == "Approve")
                {
                    foreach (var item in _list)
                    {
                        if (item.IsSubmitted && item.IsControlCheck)
                        {
                            item.PaymentApprovalDate = DateTime.UtcNow;
                            item.PaymentApprovalBy = CommonDataParam.LoginId;
                            item.IsApproved = true;
                            item.RecordStatus = "Approved";
                        }
                    }
                }
                else if (param.EventType == "ApplyInvoiceNumber")
                {
                    foreach (var item in _list)
                    {
                        if (item.IsControlCheck)
                        {
                            item.RecordStatus = "Invoice Number Applied";
                            item.InvoiceNumber = param.InvoiceNumber;
                        }
                    }
                }


                dBContext.MainLedger.UpdateRange(_list);
                await dBContext.SaveChangesAsync();



                try
                {
                    foreach (var id in param.Ids)
                    {
                        Logs log = new Logs();
                        var objcat = dBContext.Logs.OrderByDescending(x => x.Id).FirstOrDefault();
                        long eventId = 1;
                        if (objcat != null)
                            eventId = objcat.Id + 1;

                        log.MainLedgerId = id;
                        log.LogType = param.EventType;
                        log.LogDescription = param.EventDescription;
                        log.LogByUserId = CommonDataParam.LoginId;
                        log.LogDateTime = DateTime.UtcNow;
                        log.Id = eventId;

                        dBContext.Logs.Add(log);
                        await dBContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}