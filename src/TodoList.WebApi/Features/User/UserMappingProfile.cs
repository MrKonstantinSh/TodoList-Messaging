using AutoMapper;
using TodoList.WebApi.Features.User.Commands;

namespace TodoList.WebApi.Features.User;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserCommand, User>();
    }
}