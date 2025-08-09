using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Model
{
    public class DashboardViewModel
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
    }
    public class DashboardInputModel
    {
        public string TableType { get; set; }
        public string ColumnName { get; set; }
    }
    public class DashboardTotalUsers
    {
        public int TotalInfluencers { get; set; }
        public int  TotalVIP { get; set; }
        public int TotalPartnerships { get; set; }
    }

    public class PaymentData
    {
        public float Day { get; set; }
        public float Expense { get; set; }
        public float Income { get; set; }
    }

    public class AnomalyPrediction
    {
        [VectorType(3)]
        public double[] Prediction { get; set; }
    }

    public class PaymentPrediction
    {
        public float Score { get; set; }
    }

}
