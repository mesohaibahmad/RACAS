using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Auth
{
    public class UpdateUserModel
    {
        public string EmployeeCode { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
    }
}
