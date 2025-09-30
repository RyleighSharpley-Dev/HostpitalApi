using HostpitalApi.DTOs;
using HostpitalApi.Models;
using System.Linq;

namespace HospitalApi
{
    public static class Mappers
    {
        public static PatientDTO ToDto(this Patient patient)
        {
            return new PatientDTO
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MedicalHistory = patient.MedicalHistory,
                ApiKey = patient.apiKey,
                DoctorId = patient.DoctorId,
               
            };
        }

        public static DoctorDTO ToDto(this Doctor doctor)
        {
            return new DoctorDTO
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Role = doctor.Role,
                ApiKey = doctor.apiKey,
                Patients = doctor.Patients?.Select(p => p.ToDto()).ToList() ?? new List<PatientDTO>()
            };
        }
    }
}
