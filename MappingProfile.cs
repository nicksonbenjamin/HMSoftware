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
	    CreateMap<LedgerMaster, LedgerMasterViewModel>();
        CreateMap<LedgerMasterViewModel, LedgerMaster>();
        CreateMap<DescriptionMaster, DescriptionMasterViewModel>();
        CreateMap<DescriptionMasterViewModel, DescriptionMaster>();

               // ⚡ Child mapping
        CreateMap<DescriptionMasterTestValue, DescriptionMasterTestValueViewModel>();
        CreateMap<DescriptionMasterTestValueViewModel, DescriptionMasterTestValue>();

          // 🏨 Room mappings
        CreateMap<RoomMaster, RoomMasterViewModel>()
            .ForMember(dest => dest.RoomTypeList, opt => opt.Ignore()) // this list is loaded separately
            .ReverseMap();

        CreateMap<RoomTypeMaster, RoomTypeMasterViewModel>()
            .ReverseMap();

            //CreateMap<RoomMaster, RoomMasterViewModel>().ReverseMap();
    }
}
