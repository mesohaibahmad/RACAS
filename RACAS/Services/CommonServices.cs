
using RACAS.Model;
using RACAS.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Microsoft.Data.SqlClient;
using RACAS.DAL;

namespace RACAS.Services
{
    public interface ICommonServices
    {
        Task<Dictionary<string, object>> DashboardData(int year, int month);
    }
    public class CommonServices : ICommonServices
    {
        private readonly RACASContext dBContext;
        private readonly IWebHostEnvironment environment;
        public IConfiguration Configuration { get; set; }
        public CommonServices(RACASContext _dbContext, IWebHostEnvironment _environment
            , IConfiguration configuration)
        {
            dBContext = _dbContext;
            environment = _environment;
            Configuration = configuration;
        }
        public async Task<Dictionary<string, object>> DashboardData(int year, int month)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            BaseDataAccess baseDataAccess = new BaseDataAccess();

            List<SqlParameter> listparam = new List<SqlParameter>();
            listparam.Add(new SqlParameter { ParameterName = "@Year", Value = year });
            listparam.Add(new SqlParameter { ParameterName = "@Month", Value = month });

            List<SqlParameter> listparam2 = new List<SqlParameter>();
            listparam2.Add(new SqlParameter { ParameterName = "@Year", Value = year });
            listparam2.Add(new SqlParameter { ParameterName = "@Month", Value = month });

            var rd = baseDataAccess.GetDataReaderToDictionary("SpGetPaymentSummary", listparam);
            response.Add("Stats", rd);
            var rd2 = baseDataAccess.GetDataReaderToDictionary("SpGetCompareGraph", listparam2);
            response.Add("Graph", rd2);
            return response;

        }
    }
}
