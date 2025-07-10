using AutoMapper;
using task_management_system_api.Dtos;
using task_management_system_api.Models;

public class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        CreateMap<Userrole, UserRoleReadDto>();
        CreateMap<UserRoleReadDto, Userrole>();
    }
}