using Infraestructure.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json.Serialization;
using Infraestructure.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Configuration;
using Microsoft.IdentityModel.Logging;
using WebApi.WellKnownNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(setupAction =>
{
    //Return Not Acceptable Status Code when api is requested in a format that it does not support
    setupAction.ReturnHttpNotAcceptable = true;
})
.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
.ConfigureApiBehaviorOptions(setupAction =>
 {
     setupAction.InvalidModelStateResponseFactory = context =>
     {
         // create a problem details object
         var problemDetailsFactory = context.HttpContext.RequestServices
             .GetRequiredService<ProblemDetailsFactory>();
         var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                 context.HttpContext,
                 context.ModelState);

         // add additional info not added by default
         problemDetails.Detail = "See the errors field for details.";
         problemDetails.Instance = context.HttpContext.Request.Path;

         // find out which status code to use
         var actionExecutingContext =
               context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

         // if there are modelstate errors & all keys were correctly
         // found/parsed we're dealing with validation errors
         if ((context.ModelState.ErrorCount > 0) &&
             (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
         {
             //problemDetails.Type = "";
             problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
             problemDetails.Title = "One or more validation errors occurred.";

             return new UnprocessableEntityObjectResult(problemDetails)
             {
                 ContentTypes = { "application/problem+json" }
             };
         }

         // if one of the keys wasn't correctly found / couldn't be parsed
         // we're dealing with null/unparsable input
         problemDetails.Status = StatusCodes.Status400BadRequest;
         problemDetails.Title = "One or more errors on input occurred.";
         return new BadRequestObjectResult(problemDetails)
         {
             ContentTypes = { "application/problem+json" }
         };
     };
 })
;
// CORS Configuration
var allowedHosts = builder.Configuration[AppSettings.AllowedHosts].Split(',');

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedHosts", builder =>
    {
        builder.WithOrigins(allowedHosts.ToArray());
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

//Api Docs Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Application Core Services Configuration
builder.Services.AddAutoMapperWithProfiles();
builder.Services.AddEntitiesServicesAndRepositories();
builder.Services.AddCustomApplicationServices();

//Unit of Work Implementation Configuration
builder.Services.AddDbContext<UnitOfWorkContainer>(options =>
   options.UseMySql(builder.Configuration.GetConnectionString(AppSettings.DefaultConnectionString), 
   Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"))
   .LogTo(Console.WriteLine,LogLevel.Information)
   .EnableDetailedErrors()
   );

//Security Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)                
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(AppSettings.AzureAd));


IdentityModelEventSource.ShowPII = true;

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(appBuilder => appBuilder.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error happened, please try later");
    }));
}
app.UseHttpsRedirection();

app.UseCors("AllowedHosts");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

