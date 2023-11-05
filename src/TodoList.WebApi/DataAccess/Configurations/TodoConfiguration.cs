using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.DataAccess.Configurations;

public sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos");
        
        builder.Property(u => u.Id)
            .HasColumnName("id");
        
        builder.Property(u => u.Title)
            .HasColumnName("title")
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(u => u.Description)
            .HasColumnName("description")
            .HasMaxLength(10_000)
            .IsRequired();
        
        builder.Property(u => u.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();
        
        builder
            .HasMany<User>(t => t.AssignedUsers)
            .WithMany(u => u.Todos)
            .UsingEntity(
                "users_todos",
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("user_id").HasPrincipalKey(nameof(User.Id)),
                l => l.HasOne(typeof(Todo)).WithMany().HasForeignKey("todo_id").HasPrincipalKey(nameof(Todo.Id)),
                j => j.HasKey("user_id", "todo_id"));
    }
}