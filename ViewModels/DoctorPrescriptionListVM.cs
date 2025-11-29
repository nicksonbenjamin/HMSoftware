namespace ClinicApp.ViewModels
{
    public class DoctorPrescriptionListVM
{
    public int DrPrescriptionId { get; set; }
    public string PatientName { get; set; }
    public string PrescriptionNo => DrPrescriptionId.ToString(); // Or generate your own
    public string EntryType { get; set; }
    public int EntryNumber { get; set; }
    public DateTime PrescriptionDate { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public string DiseaseName { get; set; }
}

}
