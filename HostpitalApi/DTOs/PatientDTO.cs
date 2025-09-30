namespace HostpitalApi.DTOs
{
    public class PatientDTO
    {
        public Guid PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
        public string?  ApiKey { get; set; }

        public Guid DoctorId { get; set; }
    }
}
