// Models/Appointment.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Appointment
{
    public int AppointmentID { get; set; }
    public string PatientID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Optional: Display patient name
    public string PatientName { get; set; }
}
