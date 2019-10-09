

using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class CustomerModel
    {
        [Required(ErrorMessage ="Invalid Name")]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z\s]$")]
        public string CustomerName { get; set; }     

        [Required(ErrorMessage = "Please enter valid Email id")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Inavlid mobile number")]
        public string CustomerMobileNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        public string CustomerPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        public string CustomerConfirmPassword { get; set; }

        public string CustomerRole { get; set; }
    }
}
