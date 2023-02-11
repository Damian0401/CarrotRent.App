using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(options => 
{
    options.AddPolicy("CorsPolicy", opt =>
        opt.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
});

var app = builder.Build();

app.UseSwagger();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapRestEndpoints();

app.UseSwaggerUI();
app.Run();
