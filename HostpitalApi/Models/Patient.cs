using System.Numerics;

namespace HostpitalApi.Models
{
    public class Patient
    {
        public Guid PatientId { get; set; } = Guid.NewGuid();               
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";
        public string apiKey { get; set; } = Guid.NewGuid().ToString();

        public Guid DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }          
    }
}
