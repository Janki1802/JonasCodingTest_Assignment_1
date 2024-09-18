using AutoMapper;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi
{
    public class AppServicesProfile : Profile
    {
        public AppServicesProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<BaseInfo, BaseDto>();
            CreateMap<CompanyInfo, CompanyDto>();
            CreateMap<ArSubledgerInfo, ArSubledgerDto>();
            CreateMap<EmployeeInfo, EmployeeDto>()
            .ForMember(dest => dest.OccupationName, opt => opt.MapFrom(src => src.Occupation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => src.LastModified));
        }
    }
}