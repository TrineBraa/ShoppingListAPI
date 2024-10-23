using System;
using System.Data.SqlClient;
using Dapper;
using ShoppingList.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

var allowedOrigins = "_allowOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        policy =>
        {
            policy.WithOrigins("http://example.com",
                    "http://www.contoso.com",
                    "http://localhost:5174",
                    "http://localhost:5173",
                    "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
const string connStr = @"Data Source=DESKTOP-1HV37H6\SQLEXPRESS;Initial Catalog=ShoppingList;Integrated Security=True;Connect Timeout=30;Encrypt=False";
const string connStrToDo = @"Data Source=DESKTOP-1HV37H6\SQLEXPRESS; Initial Catalog = TodoList; Integrated Security = True; Connect Timeout = 30; Encrypt = False;";
app.UseCors(allowedOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//Shoppinglist Application Del

app.MapGet("/shoppingList", async () =>
{
        var connection = new SqlConnection(connStr);
        const string SQL = "select id, Shopping FROM shoppingList";
        var shoppingItems = await connection.QueryAsync<ShoppingItem>(SQL);
        return shoppingItems;
});

app.MapPost("/shoppingList", async (ShoppingItem shoppingItem) =>
{
    var connection = new SqlConnection(connStr);
    const string SQL = "INSERT INTO shoppingList (id, Shopping) VALUES (@id, @Shopping)";
    var rowsAffects = await connection.ExecuteAsync(SQL, shoppingItem);
    return rowsAffects;
});

app.MapPut("/shoppingList", async (ShoppingItem shoppingItem) =>
{
    var connection = new SqlConnection(connStr);
    const string SQL = "UPDATE shoppingList Set Shopping = @Shopping WHERE id = @id";
    var rowsAffected = await connection.ExecuteAsync(SQL, shoppingItem);
    return rowsAffected;
});

app.MapDelete("/shoppingList/{id}", async (Guid id) =>
{
    var connection = new SqlConnection(connStr);
    const string SQL = "DELETE FROM shoppingList WHERE id = @id";
    var rowsAffected = await connection.ExecuteAsync(SQL, new { id = id });
    return rowsAffected;
});


//Todo Application Del

app.MapGet("/Todo", async () =>
{
    var connection = new SqlConnection(connStrToDo);
    const string SQL = "select id, Task, Done FROM TodoList";
    var TodoItems = await connection.QueryAsync<TodoTask>(SQL);
    return TodoItems;
});

app.MapPost("/Todo", async (TodoTask todoTask) =>
{
    var connection = new SqlConnection(connStrToDo);
    const string SQL = "INSERT INTO TodoList (Id, Task) VALUES (@Id, @Task)";
    var rowsAffected = await connection.ExecuteAsync(SQL, todoTask);
    return rowsAffected;
});

app.MapPut("/Todo", async (TodoTask todoTask) =>
{
    var connection = new SqlConnection(connStrToDo);
    const string SQL = "UPDATE TodoList SET Task = @Task WHERE Id = @Id";
    var rowsAffected = await connection.ExecuteAsync(SQL, todoTask);
    return rowsAffected;
});

app.MapDelete("/Todo/{id}", async (Guid id) =>
{
    var connection = new SqlConnection(connStrToDo);
    const string SQL = "DELETE FROM TodoList WHERE id = @id";
    var rowsAffected = await connection.ExecuteAsync(SQL, new { id = id });
    return rowsAffected;
});




app.UseStaticFiles();
app.Run();

