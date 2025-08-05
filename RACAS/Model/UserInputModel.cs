using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Model
{
    public class UserInputModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public List<string> UserRights { get; set; }
    }

    public class UserTypeModel
    {
        public long USerTypeId { get; set; }
        public string TypeName { get; set; }
        public List<string> UserRights { get; set; }
    }
}
