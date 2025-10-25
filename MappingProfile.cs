using ClinicApp.Models;
using ClinicApp.ViewModels;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserViewModel>();
        CreateMap<ApplicationUserViewModel, ApplicationUser>();
        CreateMap<Patient, PatientViewModel>();
        CreateMap<PatientViewModel, Patient>();
        CreateMap<ProductMaster, ProductMasterViewModel>();
        CreateMap<ProductMasterViewModel, ProductMaster>();
    }
}
