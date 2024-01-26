using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservation.Models
{
    public class Room
    {
        [Key]
        public int Room_id { get; set; }

        public string Room_Type { get; set; }
        public double Price { get; set; }
        public string Room_Name { get; set; }
        public string Room_Description { get; set; }
        public bool IsAvailable { get; set; }

    
        public string Room_imageUrl { get; set; }
    }
}
