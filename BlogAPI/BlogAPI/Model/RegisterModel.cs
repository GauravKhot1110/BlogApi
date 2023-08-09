using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email s required")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Role { get; set; }


    }
    public class UserDetails
    {
        public string username { get; set; }
        public string userrole { get; set; }

        public string token { get; set; }
       // public object claim { get; set; }
    }
}