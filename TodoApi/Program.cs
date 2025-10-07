using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// força endereço http local (útil para desenvolvimento)
builder.WebHost.UseUrls("http://localhost:5000");

// permitir CORS para o frontend local
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

var todos = new List<Todo>();

app.MapGet("/todos", () => Results.Ok(todos));

app.MapGet("/todos/{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/todos", (TodoCreate dto) =>
{
    var id = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;
    var todo = new Todo { Id = id, Title = dto.Title ?? "", Done = false };
    todos.Add(todo);
    return Results.Created($"/todos/{id}", todo);
});

app.MapPut("/todos/{id:int}", (int id, TodoUpdate dto) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();
    todo.Title = dto.Title ?? todo.Title;
    todo.Done = dto.Done;
    return Results.Ok(todo);
});

app.MapDelete("/todos/{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();
    todos.Remove(todo);
    return Results.NoContent();
});

app.Run();

// modelos simples
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
}

public class TodoCreate
{
    public string? Title { get; set; }
}

public class TodoUpdate
{
    public string? Title { get; set; }
    public bool Done { get; set; }
}