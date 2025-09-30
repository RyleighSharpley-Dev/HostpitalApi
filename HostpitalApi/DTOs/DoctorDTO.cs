namespace HostpitalApi.DTOs
{
    public class DoctorDTO
    {
        public Guid DoctorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "Doctor";

        public string? ApiKey { get; set; } = string.Empty;
        public List<PatientDTO> Patients { get; set; } = new List<PatientDTO>();
    }
}
