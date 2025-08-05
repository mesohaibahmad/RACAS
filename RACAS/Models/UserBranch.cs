using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class UserBranch
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string RecordType { get; set; }
    }
}
