namespace FleetManagerWeb.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
    }
}
