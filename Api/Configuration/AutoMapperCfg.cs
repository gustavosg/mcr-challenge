using AutoMapper;

using Application.DTO.Application.Motorcycle;
using Application.DTO.Application.MotorcycleRental;
using Application.DTO.Application.Rental;
using Application.DTO.Application.User;
using Core.Entities;
using Core.Enum;

namespace Api.Configuration
{
    public static class AutoMapperCfg
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));

            services.AddSingleton(CreateConfiguration().CreateMapper());

            return services;
        }

        private static MapperConfiguration CreateConfiguration()
            =>
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MotorcycleAddRequestDTO, MotorcycleModel>().ReverseMap();
                cfg.CreateMap<MotorcycleModel, MotorcycleAddResponseDTO>().ReverseMap();
                cfg.CreateMap<MotorcycleEditRequestDTO, MotorcycleModel>().ReverseMap();
                cfg.CreateMap<MotorcycleModel, MotorcycleEditResponseDTO>().ReverseMap();
                cfg.CreateMap<MotorcycleModel, MotorcycleGetResponseDTO>()
                    .ForMember(dest => dest.FullDescription, opt => opt.MapFrom(src => String.Format("{0} - {1}", src.Model, src.Plate)));

                cfg.CreateMap<MotorcycleRentalAddRequestDTO, MotorcycleRentalModel>();
                cfg.CreateMap<MotorcycleRentalModel, MotorcycleRentalAddResponseDTO>();
                cfg.CreateMap<MotorcycleRentalModel, MotorcycleRentalGetResponseDTO>();

                cfg.CreateMap<RentalGetRequestDTO, RentalModel>().ReverseMap();
                cfg.CreateMap<RentalModel, RentalGetResponseDTO>().ReverseMap();

                cfg.CreateMap<UserModel, UserDto>().ReverseMap();
                cfg.CreateMap<RegisterAdminRequestDTO, UserModel>().ReverseMap();
                cfg.CreateMap<UserModel, RegisterAdminResponseDTO>().ReverseMap();
                cfg.CreateMap<AuthenticateResponseDTO, UserModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();

                cfg.CreateMap<RegisterDeliveryPersonRequestDTO, UserModel>()
                    .ForMember(dest => dest.DriverCardType, opt => opt.MapFrom(src => Enum.Parse<DriverCardType>(src.DriverCardType)));
                ;
                cfg.CreateMap<UserModel, RegisterDeliveryPersonResponseDTO>().ReverseMap();

            });
    }
}
