using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.ViewModels
{
    public class DoctorPrescriptionVM
    {
        public DrPrescription Prescription { get; set; }
        public List<DrPrescriptionDetails> Medicines { get; set; }

        public List<PatientsMaster> PatientList { get; set; }
        public List<DiseaseMaster> DiseaseList { get; set; }
        public List<DosePattern> DosePatternList { get; set; }
        public List<ProductMaster> MedicineList { get; set; }

        // Doctor list for dropdowns
        public List<DoctorMaster> DoctorList { get; set; }
        public List<DrPrescriptionClinical> Clinical { get; set; }
    }
}
