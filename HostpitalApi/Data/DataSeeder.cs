using HospitalApi.Data;
using HospitalApi.Models;
using HostpitalApi.Models;
using System;
using System.Linq;

namespace HospitalApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Doctors.Any() || context.Patients.Any())
                return;
            //Seed Admin
            var admin = new Admin
            {
                AdminId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                FirstName = "Super",
                LastName = "Admin",
                apiKey = "admin-api-key"
            };

            context.Admins.Add(admin);
            // Seed Doctors
            var doctor1 = new Doctor
            {
                DoctorId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FirstName = "Alice",
                LastName = "Smith",
                Role = "Doctor",
                apiKey = "doctor-api-key-1"
            };

            var doctor2 = new Doctor
            {
                DoctorId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FirstName = "Bob",
                LastName = "Johnson",
                Role = "Doctor",
                apiKey = "doctor-api-key-2"
            };

            context.Doctors.AddRange(doctor1, doctor2);

            // Seed Patients
            var patient1 = new Patient
            {
                PatientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                FirstName = "Charlie",
                LastName = "Chaplin",
                MedicalHistory = "No major conditions",
                Role = "Patient",
                apiKey = "patient-api-key-1",
                DoctorId = doctor1.DoctorId
            };

            var patient2 = new Patient
            {
                PatientId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                FirstName = "Dana",
                LastName = "White",
                MedicalHistory = "Allergic to penicillin",
                Role = "Patient",
                apiKey = "patient-api-key-2",
                DoctorId = doctor2.DoctorId
            };

            context.Patients.AddRange(patient1, patient2);

            context.SaveChanges();
        }
    }
}
