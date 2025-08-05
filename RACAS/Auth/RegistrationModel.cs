using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustPayApi.Authentication
{
    public class RegisterModel
    {

        public string EmployeeId { get; set; }
        public int LocationId { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Business unit is required")]
        public string BUCode { get; set; }

        [Required(ErrorMessage = "Company Code is required")]
        public string CoCode { get; set; }

        [Required(ErrorMessage = "Country code is required")]
        public string CountryCode { get; set; }

        public string AdditionalID { get; set; }
    }
}
