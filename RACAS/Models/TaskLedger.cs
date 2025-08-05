using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class TaskLedger
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string Comment { get; set; }
        public long BranchId { get; set; }
        public string Location { get; set; }
        public TimeSpan NecessaryTime { get; set; }
        public long LevelId { get; set; }
        public long AssignedDate { get; set; }
        public DateTime TaskAssignedDate { get; set; }
        public DateTime TaskFinishedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string RecordStatus { get; set; }
    }
}
