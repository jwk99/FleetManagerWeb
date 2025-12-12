using System.ComponentModel.DataAnnotations;

namespace FleetManagerWeb.Models
{
    public class Vacation
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Data początku jest wymagana.")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "Data końca jest wymagana.")]
        public DateTime To { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int? SubstituteDriverId {  get; set; }
        public Driver SubstituteDriver { get; set; }
    }
}
