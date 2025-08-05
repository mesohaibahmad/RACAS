using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class Countries
    {
        public long Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
