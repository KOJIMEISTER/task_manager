using AutoMapper;

using task_management_system_api.Dtos;
using task_management_system_api.Models;
namespace task_management_system_api.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserReadDto, User>();
    }
}