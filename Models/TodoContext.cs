using Microsoft.EntityFrameworkCore;

namespace dotnet_react_todo.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> todos { get; set; }
    }
}