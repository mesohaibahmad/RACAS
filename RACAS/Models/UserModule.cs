using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class UserModule
    {
        public long Id { get; set; }
        public long UserTypeId { get; set; }
        public int ModuleId { get; set; }
        public int ActionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public string RecordStatus { get; set; }
        public long? UserId { get; set; }
    }
}
