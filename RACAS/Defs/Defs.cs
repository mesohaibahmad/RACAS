using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Defs
{
    public static class BusinessUnit
    {
        public const string VN_HEC = "";
    }


    public enum PaymentStatus
    {
        Prnding = 0,
        Fullfilled = 1,
        Failed = 2,
        Cancelled = 3,
        Refunded = 4,
        Processing = 5
    }

    //public enum CollectorIdPrefix
    //{
    //    CG = 1,
    //    HEC = 3,
    //    PM = 4,
    //    TEC = 5
    //}

}
