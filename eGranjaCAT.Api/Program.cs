using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddHttpContextAccessor();


builder.Services
    .AddMappings()
    .AddApiVersioningSetup()
    .AddJwtAuthentication(builder.Configuration)
    .AddCustomAuthorization()
    .AddSwaggerDocumentation()
    .AddApplicationDI()
    .AddDomainDI()
    .AddInfrastructureDI(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "eGranjaCAT - RESTful API");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();