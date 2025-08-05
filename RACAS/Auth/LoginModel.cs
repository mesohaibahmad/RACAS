using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustPayApi.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        //public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
