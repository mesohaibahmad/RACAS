using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class Branches
    {
        public long Id { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public long CountryId { get; set; }
        public string Division { get; set; }
    }
}
