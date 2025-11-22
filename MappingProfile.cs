using ClinicApp.Models;
using ClinicApp.ViewModels;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Existing mappings
        CreateMap<ApplicationUser, ApplicationUserViewModel>();
        CreateMap<ApplicationUserViewModel, ApplicationUser>();
        CreateMap<Patient, PatientViewModel>();
        CreateMap<PatientViewModel, Patient>();
        CreateMap<ProductMaster, ProductMasterViewModel>();
        CreateMap<ProductMasterViewModel, ProductMaster>();
        CreateMap<LedgerMaster, LedgerMasterViewModel>();
        CreateMap<LedgerMasterViewModel, LedgerMaster>();
        CreateMap<DescriptionMaster, DescriptionMasterViewModel>();
        CreateMap<DescriptionMasterViewModel, DescriptionMaster>();

        // Child mapping
        CreateMap<DescriptionMasterTestValue, DescriptionMasterTestValueViewModel>();
        CreateMap<DescriptionMasterTestValueViewModel, DescriptionMasterTestValue>();

        // Room mappings
        CreateMap<RoomMaster, RoomMasterViewModel>()
            .ForMember(dest => dest.RoomTypeList, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<RoomTypeMaster, RoomTypeMasterViewModel>().ReverseMap();

        // ICD Code mapping
        CreateMap<ICDCodeMaster, Icd10CodeViewModel>().ReverseMap();

        // CommonMaster mapping
        CreateMap<CommonMaster, CommonMasterViewModel>().ReverseMap();

        // ======================================================
        // Prescription mappings (commented for now)
        // ======================================================
        //CreateMap<Prescription, PrescriptionEntryViewModel>()
        //    .ForMember(dest => dest.Medicines, opt => opt.Ignore())
        //    .ForMember(dest => dest.ClinicalDetails, opt => opt.Ignore())
        //    .ReverseMap();
        //CreateMap<PrescriptionMedicine, PrescriptionMedicineVM>().ReverseMap();
        //CreateMap<PrescriptionClinicalDetail, PrescriptionClinicalDetailVM>().ReverseMap();

        // ======================================================
        // NEW: PatientRegistration mapping
        // ======================================================
        CreateMap<Patient, PatientRegistrationViewModel>()
            .ForMember(dest => dest.UHIDTypes, opt => opt.Ignore())
            .ForMember(dest => dest.Sexes, opt => opt.Ignore())
            .ForMember(dest => dest.PatientTitles, opt => opt.Ignore())
            .ForMember(dest => dest.GuardianTitles, opt => opt.Ignore())
            .ForMember(dest => dest.GSTStates, opt => opt.Ignore())
            .ForMember(dest => dest.Countries, opt => opt.Ignore())
            .ForMember(dest => dest.MaritalStatuses, opt => opt.Ignore())
            .ForMember(dest => dest.BloodGroups, opt => opt.Ignore())
            .ForMember(dest => dest.ConsultantDoctors, opt => opt.Ignore())
            .ForMember(dest => dest.RefDoctors, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentTermsList, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationTypes, opt => opt.Ignore())
            .ForMember(dest => dest.CompInsCampList, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Photo, opt => opt.Ignore()); // Assuming Photo handling is manual
    }
}
