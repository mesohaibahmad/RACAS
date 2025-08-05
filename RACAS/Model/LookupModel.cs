using iText.Layout.Element;
using RACAS.Models;
using System.Collections.Generic;

namespace RACAS.Model
{
    public class LookupModel
    {
        public List<Branches> BranchList { get; set; }
        public List<Countries> CountryList { get; set; }
        public List<CausedBy> CausedByList { get; set; }
        public List<Descriptions> DescriptionList { get; set; }

      
        //public List<ModuleListModel> ModuleList { get; set; }
        //public List<Lookup> LookUps { get; set; }
        public List<UserTypes> UserTypeList { get; set; }
        //public string Selected { get; set; }

        //public List<Branches> BranchList { get; set; }
        //public List<UserBranchesModel> UserBranchList { get; set; }

    }
}
