using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class Partners
    {
        public long Id { get; set; }
        public string Vatid { get; set; }
        public long IdNumber { get; set; }
        public string CompanyName { get; set; }
        public bool IsTaxablePerson { get; set; }
        public string FullAddress { get; set; }
        public string PostCode { get; set; }
        public string TownCity { get; set; }
        public string Country { get; set; }
        public string ContactPerson1 { get; set; }
        public string ContactEmail1 { get; set; }
        public string ContactPhone1 { get; set; }
        public string? ContactPerson2 { get; set; }
        public string? ContactEmail2 { get; set; }
        public string? ContactPhone2 { get; set; }
        public string? InsuranceCompany { get; set; }
        public long? InsurancePolicyNumber { get; set; }
        public DateTime? InsurancePolicyValidUntil { get; set; }
        public bool InsuranceContract { get; set; }
        public bool ContractSigned { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public string RecordStatus { get; set; }
    }
}
