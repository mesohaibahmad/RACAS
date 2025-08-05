using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Model
{
    public class UserDetails
    {
        public User UserData { get; set; }
        public UserTypes UserType { get; set; }
        public List<ModuleListModel> ModuleList { get; set; }
        public List<UserTypes> UserTypeList { get; set; }
        public List<User> Users { get; set; }
        public string Selected { get; set; }

        public List<Branches> BranchList { get; set; }
        public List<UserBranchesModel> UserBranchList { get; set; }
    }
}
