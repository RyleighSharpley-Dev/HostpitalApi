using HospitalApi;
using HospitalApi.Attributes;
using HospitalApi.Data;
using HostpitalApi.DTOs;
using HostpitalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET all patients - Admin only
    [HttpGet("all")]
    [AuthorizeRole("Admin")]
    public IActionResult GetAllPatients()
    {
        var patients = _context.Patients
            .Select(p => p.ToDto())
            .ToList();
        return Ok(patients);
    }

    // GET patient by ID - Admin only
    [HttpGet("byId")]
    [AuthorizeRole("Admin")]
    public IActionResult GetPatient([FromQuery] Guid id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null) return NotFound();
        return Ok(patient.ToDto());
    }

    // POST - add patient - Admin only
    [HttpPost]
    [AuthorizeRole("Admin")]
    public IActionResult AddPatient([FromBody] PatientDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            return BadRequest(new { message = "FirstName cannot be empty." });

        var patient = new Patient
        {
            PatientId = dto.PatientId != Guid.Empty ? dto.PatientId : Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MedicalHistory = dto.MedicalHistory ?? "",
            Role = "Patient",
            apiKey = Guid.NewGuid().ToString(),
            DoctorId = dto.DoctorId
        };

        _context.Patients.Add(patient);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patient.ToDto());
    }

    // PUT - update patient - Admin only
    [HttpPut("admin-update")]
    [AuthorizeRole("Admin")]
    public IActionResult AdminUpdatePatient([FromQuery] Guid id, [FromBody] PatientDTO dto)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null) return NotFound();

        patient.FirstName = dto.FirstName;
        patient.MedicalHistory = dto.MedicalHistory;
        patient.DoctorId = dto.DoctorId;

        _context.SaveChanges();
        return Ok(patient.ToDto());
    }

    // DELETE - Admin only
    [HttpDelete]
    [AuthorizeRole("Admin")]
    public IActionResult DeletePatient([FromQuery] Guid id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null) return NotFound();

        _context.Patients.Remove(patient);
        _context.SaveChanges();
        return Ok("Patient Was Deleted Successfully");
    }

    [HttpGet("my-patients")]
    [AuthorizeRole("Doctor")]
    public IActionResult GetMyPatients()
    {
        // Retrieve the authenticated doctor from the HttpContext
        var doctor = HttpContext.Items["User"] as Doctor;
        if (doctor == null)
            return Unauthorized(new { message = "Invalid API Key" });

        // Include Doctor navigation property to populate DoctorName in DTO
        var patients = _context.Patients
            .Where(p => p.DoctorId == doctor.DoctorId)
            .Include(p => p.Doctor)
            .Select(p => p.ToDto())
            .ToList();

        return Ok(patients);
    }

    // PUT - update medical history - Doctor only
    [HttpPut("update-medical-history")]
    [AuthorizeRole("Doctor")]
    public IActionResult UpdateMedicalHistory([FromQuery] Guid patientId, [FromBody] PatientDTO dto)
    {
        var doctor = HttpContext.Items["User"] as Doctor;
        if (doctor == null) return Unauthorized();

        var patient = _context.Patients.Find(patientId);
        if (patient == null) return NotFound();

        if (patient.DoctorId != doctor.DoctorId)
            return Forbid("Doctors can only update their own patients.");

        patient.MedicalHistory = dto.MedicalHistory ?? patient.MedicalHistory;

        _context.SaveChanges();
        return Ok(patient.ToDto());
    }

    // GET own details - Patient only
    [HttpGet("me")]
    [AuthorizeRole("Patient")]
    public IActionResult GetMyDetails()
    {
        var patient = HttpContext.Items["User"] as Patient;
        if (patient == null) return Unauthorized();

        return Ok(patient.ToDto());
    }
}
