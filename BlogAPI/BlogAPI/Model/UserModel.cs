

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Model
{
     
    [Keyless]
    public class UserLogin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Geneder { get; set; }
        public string ProfileImg { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
     
    public class CustomResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class CustomResponseLogin
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string UserID { get; set; }
    }

    
    
}
