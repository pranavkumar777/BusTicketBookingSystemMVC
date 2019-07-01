
using System.ComponentModel.DataAnnotations;

namespace Model
{
   public class AdminModel
    {
        [Required(ErrorMessage = "Please enter valid Email id")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        public string Password { get; set; }


    }
}
