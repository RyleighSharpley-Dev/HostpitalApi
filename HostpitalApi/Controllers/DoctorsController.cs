using HospitalApi;
using HospitalApi.Attributes;
using HospitalApi.Data;
using HostpitalApi.DTOs;
using HostpitalApi.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DoctorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET all doctors - Admin only
    [HttpGet("all")]
    [AuthorizeRole("Admin")]
    public IActionResult GetAllDoctors()
    {
        var doctors = _context.Doctors.Select(d => d.ToDto()).ToList();
        return Ok(doctors);
    }

    // GET doctor by ID using query param
    [HttpGet]
    [AuthorizeRole("Admin")]
    public IActionResult GetDoctor([FromQuery] Guid id)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor == null) return NotFound();
        return Ok(doctor.ToDto());
    }

    // POST - add doctor using JSON body
    [HttpPost]
    [AuthorizeRole("Admin")]
    public IActionResult AddDoctor([FromBody] DoctorDTO dto)
    {
        var doctor = new Doctor
        {
            DoctorId = dto.DoctorId != Guid.Empty ? dto.DoctorId : Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = "Doctor",
            apiKey = Guid.NewGuid().ToString()
        };

        _context.Doctors.Add(doctor);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorId }, doctor.ToDto());
    }

    // PUT - update doctor using JSON body
    [HttpPut]
    [AuthorizeRole("Admin")]
    public IActionResult UpdateDoctor([FromQuery] Guid id, [FromBody] DoctorDTO dto)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor == null) return NotFound();

        doctor.FirstName = dto.FirstName;
        doctor.LastName = dto.LastName;

        _context.SaveChanges();
        return Ok(doctor.ToDto());
    }

    // DELETE using query param
    [HttpDelete]
    [AuthorizeRole("Admin")]
    public IActionResult DeleteDoctor([FromQuery] Guid id)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor == null) return NotFound();

        _context.Doctors.Remove(doctor);
        _context.SaveChanges();
        return NoContent();
    }
}
