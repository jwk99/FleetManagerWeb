using System;
using System.ComponentModel.DataAnnotations;

namespace FleetManagerWeb.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

        public DateTime Date {  get; set; }

        public string Description { get; set; }

        public decimal Cost {  get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }

    }
}
