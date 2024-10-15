using System.Data.SqlClient;
using Dapper;
using ShoppingList.Model;

var allowedOrigins = "_allowOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        policy =>
        {
            policy.WithOrigins("http://example.com",
                    "http://www.contoso.com",
                    "http://localhost:7051",
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
app.UseCors(allowedOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();



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

app.UseStaticFiles();
app.Run();

