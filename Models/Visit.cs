// Models/Visit.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Visit
{
    public Guid VisitID { get; set; }
    public Guid PatientID { get; set; }
    public DateTime VisitDate { get; set; }
    public string Notes { get; set; }
    public int AttendingStaffID { get; set; }

    // Optional display fields
    public string PatientName { get; set; }
    public string StaffName { get; set; }
}