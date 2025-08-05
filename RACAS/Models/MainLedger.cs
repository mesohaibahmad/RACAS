using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class MainLedger
    {
        public long Id { get; set; }
        public string ContractNumber { get; set; }
        public long PartnerId { get; set; }
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public long CausedById { get; set; }
        public long DescriptionId { get; set; }
        public string Comment { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime CostIncuredDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public long OrderedById { get; set; }
        public bool IsSaved { get; set; }
        public bool IsSubmitted { get; set; }
        public long SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
        public bool IsControlCheck { get; set; }
        public long ControlCheckBy { get; set; }
        public DateTime ControlCheckDate { get; set; }
        public long PaymentApprovalBy { get; set; }
        public DateTime PaymentApprovalDate { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string RecordStatus { get; set; }
        public bool? IsOrdered { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<DateTime> OrderedByDate { get; set; }
        public bool? IsRejected { get; set; }
        public long? RejectedBy { get; set; }
        public Nullable<DateTime> RejectedDate { get; set; }
    }
}
