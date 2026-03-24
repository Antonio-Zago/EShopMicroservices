using Marten;

var builder = WebApplication.CreateBuilder(args);

//Add Services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("database")!);
}).UseLightweightSessions();

var app = builder.Build();

//Configure HTTPS request pipeline
app.MapCarter();

app.Run();
