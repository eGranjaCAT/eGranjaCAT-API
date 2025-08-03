using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddCustomAuthorization()
    .AddSwaggerDocumentation()
    .AddMappings()
    .AddApplicationDI()
    .AddInfrastructureDI(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "eGranjaCAT - RESTful API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();