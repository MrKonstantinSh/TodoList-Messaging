using AutoMapper;
using TodoList.WebApi.Features.Users.Commands;

namespace TodoList.WebApi.Features.Users;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
    }
}