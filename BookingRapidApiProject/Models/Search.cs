namespace BookingRapidApiProject.Models
{
    public class Search
    {
        public string city_name { get; set; }
        public string dest_id { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public int guest { get; set; }
        public int children { get; set; }
        public int room { get; set; }
    }
}
