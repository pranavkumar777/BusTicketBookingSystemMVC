

using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
   public class BusModel
    {
        [Required]
        public int BusID { get; set; }
        [Required ]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use alphabets only please")]
        public string BusTravelsName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use alphabets only please")]
        public string BusSource { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use alphabets only please")]
        public string BusDestination { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BusDepartureDate { get; set; }
        [Required]
        public string BusDepartureTime { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = " must be numeric")]
        public int BusSeatCount { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "must be numeric")]
        public int BusTicketCost { get; set; }
    }
}
