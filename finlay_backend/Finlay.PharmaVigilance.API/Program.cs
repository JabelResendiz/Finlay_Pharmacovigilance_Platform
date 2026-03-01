

using Finlay.PharmaVigilance.Infrastructure.Initializer;
using Finlay.PharmaVigilance.Api;
using Finlay.PharmaVigilance.Application;
using Finlay.PharmaVigilance.Infrastructure;
using Finlay.PharmaVigilance.Api.Middleware;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;


services.AddPresentation();
services.AddAplication(builder.Configuration);
services.AddInfrastructure(builder.Configuration);


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value.Errors.Select(e => e.ErrorMessage)
            });

        return new BadRequestObjectResult(new
        {
            success = false,
            status = 400,
            message = "Validation failed",
            errors
        });
    };
});


//configuration so that API listens on all network interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5137);  // Escucha en todas las interfaces en el puerto 5217
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await DatabaseInitializer.InitializeAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
//app.UseCors("LocalhostPolicy");
        
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();