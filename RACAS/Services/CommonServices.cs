
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using MimeKit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RACAS.DAL;
using RACAS.Model;
using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        //public async Task<Dictionary<string, object>> DashboardData(int year, int month)
        //{
        //    Dictionary<string, object> response = new Dictionary<string, object>();
        //    BaseDataAccess baseDataAccess = new BaseDataAccess();

        //    List<SqlParameter> listparam = new List<SqlParameter>();
        //    listparam.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //    listparam.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //    List<SqlParameter> listparam2 = new List<SqlParameter>();
        //    listparam2.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //    listparam2.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //    var rd = baseDataAccess.GetDataReaderToDictionary("SpGetPaymentSummary", listparam);
        //    response.Add("Stats", rd);
        //    var rd2 = baseDataAccess.GetDataReaderToDictionary("SpGetCompareGraph", listparam2);
        //    response.Add("Graph", rd2);





        //    return response;

        //}

        //      public async Task<Dictionary<string, object>> DashboardData(int year, int month)
        //      {
        //          Dictionary<string, object> response = new Dictionary<string, object>();
        //          BaseDataAccess baseDataAccess = new BaseDataAccess();

        //          List<SqlParameter> listparam = new List<SqlParameter>();
        //          listparam.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //          listparam.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //          List<SqlParameter> listparam2 = new List<SqlParameter>();
        //          listparam2.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //          listparam2.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //          var rd = baseDataAccess.GetDataReaderToDictionary("SpGetPaymentSummary", listparam);
        //          response.Add("Stats", rd);
        //          var rd2 = baseDataAccess.GetDataReaderToDictionary("SpGetCompareGraph", listparam2);
        //          response.Add("Graph", rd2);

        //          // Step 1: Convert raw DB data to PaymentData list
        //          var graphData = rd2
        //      .Where(item => item.ContainsKey("Labels") && item.ContainsKey("Expense") && item.ContainsKey("Income"))
        //      .Select(item => new PaymentData
        //      {
        //          Day = DateTime.ParseExact(item["Labels"].ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture).Day,
        //          Expense = float.TryParse(item["Expense"].ToString(), out var exp) ? exp : 0f,
        //          Income = float.TryParse(item["Income"].ToString(), out var inc) ? inc : 0f
        //      }).Where(p => p.Day > 0 ).ToList();


        //          if (graphData == null || graphData.Count == 0)
        //          {

        //              response["Predicted"] = new List<Dictionary<string, object>>();
        //              return response;
        //          }

        //          var mlContext = new MLContext();
        //          var dataView = mlContext.Data.LoadFromEnumerable(graphData);

        //          // Step 2: Anomaly Detection on Expense
        //          var anomalyPipeline = mlContext.Transforms.DetectIidSpike(
        //    outputColumnName: nameof(AnomalyPrediction.Prediction),
        //    inputColumnName: "Expense",
        //    confidence: 95,
        //    pvalueHistoryLength: 7 // ✅ Must be > 0 if AlertOn != RawScore
        //);

        //          var anomalyModel = anomalyPipeline.Fit(dataView);
        //          var anomalyTransformed = anomalyModel.Transform(dataView);
        //          var anomalyResults = mlContext.Data.CreateEnumerable<AnomalyPrediction>(anomalyTransformed, reuseRowObject: false).ToList();

        //          // Step 3: Expense Prediction using Regression
        //          var predictionData = graphData.Select(p => new PaymentData { Day = p.Day, Expense = p.Expense }).ToList();
        //          var predictionDataView = mlContext.Data.LoadFromEnumerable(predictionData);

        //          var predictionPipeline = mlContext.Transforms.CopyColumns("Label", nameof(PaymentData.Expense))
        //              .Append(mlContext.Transforms.Concatenate("Features", nameof(PaymentData.Day)))
        //              .Append(mlContext.Regression.Trainers.FastTree());

        //          var predictionModel = predictionPipeline.Fit(predictionDataView);

        //          float lastDay = graphData.Max(x => x.Day);
        //          int startDay = (int)Math.Ceiling(lastDay);
        //          var futureInputs = Enumerable.Range(startDay + 1, 5)
        //              .Select(i => new PaymentData { Day = i }).ToList();

        //          var futurePrediction = mlContext.Data.LoadFromEnumerable(futureInputs);
        //          var predictedResults = predictionModel.Transform(futurePrediction);
        //          var predictedValues = mlContext.Data.CreateEnumerable<PaymentPrediction>(predictedResults, reuseRowObject: false).ToList();

        //          // Step 4: Add anomaly flags to rd2
        //          for (int i = 0; i < rd2.Count; i++)
        //          {
        //              rd2[i]["IsAnomaly"] = anomalyResults[i].Prediction[0] == 1 ? true : false;
        //              rd2[i]["AlertScore"] = anomalyResults[i].Prediction[2];
        //          }

        //          // Step 5: Add prediction list to response
        //          var predictionList = new List<Dictionary<string, object>>();
        //          for (int i = 0; i < predictedValues.Count; i++)
        //          {
        //              predictionList.Add(new Dictionary<string, object>
        //              {
        //                  ["Day"] = lastDay + 1 + i,
        //                  ["PredictedExpense"] = predictedValues[i].Score
        //              });
        //          }

        //          response["Graph"] = rd2;
        //          response["Predicted"] = predictionList;

        //          return response;
        //      }


        //public async Task<Dictionary<string, object>> DashboardData(int year, int month)
        //{
        //    Dictionary<string, object> response = new Dictionary<string, object>();
        //    BaseDataAccess baseDataAccess = new BaseDataAccess();

        //    List<SqlParameter> listparam = new List<SqlParameter>();
        //    listparam.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //    listparam.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //    List<SqlParameter> listparam2 = new List<SqlParameter>();
        //    listparam2.Add(new SqlParameter { ParameterName = "@Year", Value = year });
        //    listparam2.Add(new SqlParameter { ParameterName = "@Month", Value = month });

        //    var rd = baseDataAccess.GetDataReaderToDictionary("SpGetPaymentSummary", listparam);
        //    response.Add("Stats", rd);
        //    var rd2 = baseDataAccess.GetDataReaderToDictionary("SpGetCompareGraph", listparam2);
        //    response.Add("Graph", rd2);

        //    var graphData = rd2
        //        .Where(item => item.ContainsKey("Labels") && item.ContainsKey("Expense") && item.ContainsKey("Income"))
        //        .Select(item => new PaymentData
        //        {
        //            Day = DateTime.ParseExact(item["Labels"].ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture).Day,
        //            Expense = float.TryParse(item["Expense"].ToString(), out var exp) ? exp : 0f,
        //            Income = float.TryParse(item["Income"].ToString(), out var inc) ? inc : 0f
        //        })
        //        .Where(p => p.Day > 0)
        //        .ToList();

        //    if (graphData == null || graphData.Count == 0)
        //    {
        //        response["Predicted"] = new List<Dictionary<string, object>>();
        //        return response;
        //    }

        //    var mlContext = new MLContext();
        //    var dataView = mlContext.Data.LoadFromEnumerable(graphData);

        //    // Step 1: Anomaly Detection on Expense
        //    var anomalyPipeline = mlContext.Transforms.DetectIidSpike(
        //        outputColumnName: nameof(AnomalyPrediction.Prediction),
        //        inputColumnName: nameof(PaymentData.Expense),
        //        confidence: 95,
        //        pvalueHistoryLength: 7
        //    );

        //    var anomalyModel = anomalyPipeline.Fit(dataView);
        //    var anomalyTransformed = anomalyModel.Transform(dataView);
        //    var anomalyResults = mlContext.Data.CreateEnumerable<AnomalyPrediction>(anomalyTransformed, reuseRowObject: false).ToList();

        //    // Step 2: Prediction Pipelines
        //    var expensePipeline = mlContext.Transforms.CopyColumns("Label", nameof(PaymentData.Expense))
        //        .Append(mlContext.Transforms.Concatenate("Features", nameof(PaymentData.Day)))
        //        .Append(mlContext.Regression.Trainers.FastTree());

        //    var incomePipeline = mlContext.Transforms.CopyColumns("Label", nameof(PaymentData.Income))
        //        .Append(mlContext.Transforms.Concatenate("Features", nameof(PaymentData.Day)))
        //        .Append(mlContext.Regression.Trainers.FastTree());

        //    var expenseModel = expensePipeline.Fit(dataView);
        //    var incomeModel = incomePipeline.Fit(dataView);

        //    float lastDay = graphData.Max(x => x.Day);
        //    int startDay = (int)Math.Ceiling(lastDay);
        //    var futureInputs = Enumerable.Range(startDay + 1, 5)
        //        .Select(i => new PaymentData { Day = i }).ToList();

        //    var futureDataView = mlContext.Data.LoadFromEnumerable(futureInputs);

        //    var predictedExpenseResults = expenseModel.Transform(futureDataView);
        //    var predictedIncomeResults = incomeModel.Transform(futureDataView);

        //    var predictedExpenses = mlContext.Data.CreateEnumerable<PaymentPrediction>(predictedExpenseResults, reuseRowObject: false).ToList();
        //    var predictedIncomes = mlContext.Data.CreateEnumerable<PaymentPrediction>(predictedIncomeResults, reuseRowObject: false).ToList();

        //    // Step 3: Add anomaly flags
        //    for (int i = 0; i < rd2.Count; i++)
        //    {
        //        rd2[i]["IsAnomaly"] = anomalyResults[i].Prediction[0] == 1;
        //        rd2[i]["AlertScore"] = anomalyResults[i].Prediction[2];
        //    }

        //    // Step 4: Combine prediction results
        //    var predictionList = new List<Dictionary<string, object>>();
        //    for (int i = 0; i < predictedExpenses.Count; i++)
        //    {
        //        predictionList.Add(new Dictionary<string, object>
        //        {
        //            ["Day"] = startDay + 1 + i,
        //            ["PredictedExpense"] = predictedExpenses[i].Score,
        //            ["PredictedIncome"] = predictedIncomes[i].Score
        //        });
        //    }

        //    response["Graph"] = rd2;
        //    response["Predicted"] = predictionList;

        //    return response;
        //}


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
            response["Stats"] = rd;
            response["Graph"] = rd2;

            var graphData = rd2
                .Where(item => item.ContainsKey("Labels") && item.ContainsKey("Expense") && item.ContainsKey("Income"))
                .Select(item => new PaymentData
                {
                    Day = DateTime.ParseExact(item["Labels"].ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture).Day,
                    Expense = float.TryParse(item["Expense"].ToString(), out var exp) ? exp : 0f,
                    Income = float.TryParse(item["Income"].ToString(), out var inc) ? inc : 0f
                })
                .Where(p => p.Day > 0)
                .ToList();

            if (graphData == null || graphData.Count == 0)
            {
                response["Predicted"] = new List<Dictionary<string, object>>();
                return response;
            }

            var mlContext = new MLContext();
            var dataView = mlContext.Data.LoadFromEnumerable(graphData);

            // -------- Anomaly Detection for Expense --------
            var expenseAnomalyPipeline = mlContext.Transforms.DetectIidSpike(
                outputColumnName: nameof(AnomalyPrediction.Prediction),
                inputColumnName: nameof(PaymentData.Expense),
                confidence: 95,
                pvalueHistoryLength: 7
            );
            var expenseAnomalyModel = expenseAnomalyPipeline.Fit(dataView);
            var expenseAnomalyTransformed = expenseAnomalyModel.Transform(dataView);
            var expenseAnomalyResults = mlContext.Data.CreateEnumerable<AnomalyPrediction>(expenseAnomalyTransformed, reuseRowObject: false).ToList();

            // -------- Anomaly Detection for Income --------
            var incomeAnomalyPipeline = mlContext.Transforms.DetectIidSpike(
                outputColumnName: nameof(AnomalyPrediction.Prediction),
                inputColumnName: nameof(PaymentData.Income),
                confidence: 95,
                pvalueHistoryLength: 7
            );
            var incomeAnomalyModel = incomeAnomalyPipeline.Fit(dataView);
            var incomeAnomalyTransformed = incomeAnomalyModel.Transform(dataView);
            var incomeAnomalyResults = mlContext.Data.CreateEnumerable<AnomalyPrediction>(incomeAnomalyTransformed, reuseRowObject: false).ToList();

            // -------- Prediction Pipelines --------
            var expensePipeline = mlContext.Transforms.CopyColumns("Label", nameof(PaymentData.Expense))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(PaymentData.Day)))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree());

            var incomePipeline = mlContext.Transforms.CopyColumns("Label", nameof(PaymentData.Income))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(PaymentData.Day)))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree());

            var expenseModel = expensePipeline.Fit(dataView);
            var incomeModel = incomePipeline.Fit(dataView);

            float lastDay = graphData.Max(x => x.Day);
            int startDay = (int)Math.Ceiling(lastDay);

            var futureInputs = Enumerable.Range(startDay + 1, 5)
                .Select(i => new PaymentData { Day = i }).ToList();
            var futureDataView = mlContext.Data.LoadFromEnumerable(futureInputs);

            var predictedExpenseResults = expenseModel.Transform(futureDataView);
            var predictedIncomeResults = incomeModel.Transform(futureDataView);

            var predictedExpenses = mlContext.Data.CreateEnumerable<PaymentPrediction>(predictedExpenseResults, reuseRowObject: false).ToList();
            var predictedIncomes = mlContext.Data.CreateEnumerable<PaymentPrediction>(predictedIncomeResults, reuseRowObject: false).ToList();

            // -------- Add Anomaly Flags --------
            for (int i = 0; i < rd2.Count && i < expenseAnomalyResults.Count && i < incomeAnomalyResults.Count; i++)
            {
                rd2[i]["IsExpenseAnomaly"] = expenseAnomalyResults[i].Prediction[0] == 1;
                rd2[i]["ExpenseAlertScore"] = expenseAnomalyResults[i].Prediction[2];

                rd2[i]["IsIncomeAnomaly"] = incomeAnomalyResults[i].Prediction[0] == 1;
                rd2[i]["IncomeAlertScore"] = incomeAnomalyResults[i].Prediction[2];
            }

            // -------- Combine Predictions --------
            var predictionList = new List<Dictionary<string, object>>();
            for (int i = 0; i < predictedExpenses.Count; i++)
            {
                predictionList.Add(new Dictionary<string, object>
                {
                    ["Day"] = lastDay + 1 + i,
                    ["PredictedExpense"] = predictedExpenses[i].Score,
                    ["PredictedIncome"] = predictedIncomes[i].Score
                });
            }

            response["Graph"] = rd2;
            response["Predicted"] = predictionList;

            return response;
        }

    }
}
