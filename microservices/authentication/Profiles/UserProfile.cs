using AutoMapper;

namespace VignetteAuth.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<Models.Car, Protos.Car>();
            CreateMap<Models.User, Protos.User>()
                .ForMember(dest => dest.Cars, opt => opt.MapFrom(src => src.CarsList));
        }
    }
}
