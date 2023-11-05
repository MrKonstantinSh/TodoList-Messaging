using AutoMapper;
using TodoList.WebApi.Features.Todos.Commands;

namespace TodoList.WebApi.Features.Todos;

public sealed class TodoMappingProfile : Profile
{
    public TodoMappingProfile()
    {
        CreateMap<CreateTodoCommand, Todo>();
    }
}