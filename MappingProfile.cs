using AutoMapper;
using ClinicApp.Models;
using ClinicApp.ViewModels;
using System.Collections.Generic;

namespace ClinicApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ======================================================
            // General mappings
            // ======================================================
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();
            CreateMap<ProductMaster, ProductMasterViewModel>().ReverseMap();
            CreateMap<LedgerMaster, LedgerMasterViewModel>().ReverseMap();
            CreateMap<DescriptionMaster, DescriptionMasterViewModel>().ReverseMap();
            CreateMap<DescriptionMasterTestValue, DescriptionMasterTestValueViewModel>().ReverseMap();
            CreateMap<RoomMaster, RoomMasterViewModel>()
                .ForMember(dest => dest.RoomTypeList, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<RoomTypeMaster, RoomTypeMasterViewModel>().ReverseMap();
            CreateMap<ICDCodeMaster, Icd10CodeViewModel>().ReverseMap();
            CreateMap<CommonMaster, CommonMasterViewModel>().ReverseMap();

            // ======================================================
            // Doctor Prescription mappings
            // ======================================================
            // DoctorPrescriptionVM -> DrPrescription
            CreateMap<DoctorPrescriptionVM, DrPrescription>()
                .ForMember(dest => dest.DrPrescriptionId, opt => opt.MapFrom(src => src.Prescription.DrPrescriptionId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Prescription.PatientId))
                .ForMember(dest => dest.DeseaseId, opt => opt.MapFrom(src => src.Prescription.DeseaseId))
                .ForMember(dest => dest.EntryType, opt => opt.MapFrom(src => src.Prescription.EntryType))
                .ForMember(dest => dest.EntryNumber, opt => opt.MapFrom(src => src.Prescription.EntryNumber))
                .ForMember(dest => dest.EntryPeriod, opt => opt.MapFrom(src => src.Prescription.EntryPeriod))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Prescription.Height))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Prescription.Weight))
                .ForMember(dest => dest.BMI, opt => opt.MapFrom(src => src.Prescription.BMI))
                .ForMember(dest => dest.TempInCelcius, opt => opt.MapFrom(src => src.Prescription.TempInCelcius))
                .ForMember(dest => dest.BP, opt => opt.MapFrom(src => src.Prescription.BP))
                .ForMember(dest => dest.SPO2, opt => opt.MapFrom(src => src.Prescription.SPO2))
                .ForMember(dest => dest.PulseRate, opt => opt.MapFrom(src => src.Prescription.PulseRate))
                .ForMember(dest => dest.NextVisitDate, opt => opt.MapFrom(src => src.Prescription.NextVisitDate))
                .ForMember(dest => dest.ConsultantDoctorId, opt => opt.MapFrom(src => src.Prescription.ConsultantDoctorId))
                .ForMember(dest => dest.RefDoctorId, opt => opt.MapFrom(src => src.Prescription.RefDoctorId));

            // DrPrescription -> DoctorPrescriptionVM
            CreateMap<DrPrescription, DoctorPrescriptionVM>()
                .ForMember(dest => dest.Prescription, opt => opt.MapFrom(src => src));

            // DrPrescriptionDetails -> DoctorPrescriptionVM.Medicines
            CreateMap<DrPrescriptionDetails, DoctorPrescriptionVM>()
                .ForMember(dest => dest.Medicines, opt => opt.MapFrom(src => new List<DrPrescriptionDetails> { src }));

            // DoctorPrescriptionVM.Medicines -> DrPrescriptionDetails
            CreateMap<DrPrescriptionDetails, DrPrescriptionDetails>().ReverseMap();

            // DrPrescriptionClinical -> DoctorPrescriptionVM.ClinicalNotes
            CreateMap<DrPrescriptionClinical, DrPrescriptionClinical>().ReverseMap();

            // ======================================================
            // PatientRegistration mappings
            // ======================================================
            CreateMap<PatientsMaster, PatientRegistrationViewModel>()
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
                .ForMember(dest => dest.Photo, opt => opt.Ignore());
        }
    }
}
