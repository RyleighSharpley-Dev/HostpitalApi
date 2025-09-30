namespace HostpitalApi.Models
{
    public class Doctor
    {
        public Guid DoctorId { get; set; } = Guid.NewGuid();                
        public string FirstName { get; set; }  =string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "Doctor";
        public string apiKey { get; set; } = Guid.NewGuid().ToString();

        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
