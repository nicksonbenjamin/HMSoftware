public class DrPrescriptionDetails
{
    public int DrPrescriptionDetailsId { get; set; }
    public int ProductId { get; set; }
    public int DosePatternId { get; set; }
    public string DosePattern { get; set; }
    public string DoseUsage { get; set; }
    public int DoseDays { get; set; }
    public int Qty { get; set; }

    public string ProductName { get; set; }
}
