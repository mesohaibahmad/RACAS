using iText.Layout.Element;
using RACAS.Models;
using System.Collections.Generic;

namespace RACAS.Model
{
    public class PaymentModel
    {
        public List<Countries> CountryList { get; set; }
        public List<Branches> BranchList { get; set; }
        public List<Partners> PartnerList { get; set; }
        public List<User> UsersList { get; set; }
        public List<UserBranch> UserBranchList { get; set; }
        public List<CausedBy> CausedByList { get; set; }
        public List<Descriptions> DescriptionList { get; set; }
    }
}
