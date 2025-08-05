using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class UserTypes
    {
        public long Id { get; set; }
        public long UserTypeId { get; set; }
        public string TypeName { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public string RecordStatus { get; set; }
    }
}
