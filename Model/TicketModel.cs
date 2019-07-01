
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class TicketModel
    {
        [Required]
        public int BusID { get; set; }
        public int[] seats { get; set; }
        public int NumberOfSeats { get; set; }
        public int  Cost {get; set;}
        public string SeatNumbers { get; set; }
        public string CustomerEmail { get; set; }
        public string BusSource { get;set;}
        public string BusDestination { get;set;}
    }
}
