using Api.Middleware;
using AutoMapper;
using Base.Models;
using Business.Cqrs;
using Business.Mapper;
using Business.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Schema.Request;

namespace Api;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<PositionOptions>(Configuration.GetSection(PositionOptions.Position));

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());
        services.AddSingleton<IUrlPaths, UrlPaths>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllSupportedCurrencies).Assembly));
        
        services.AddControllers();
        
        services.AddFluentValidationAutoValidation();
        services.AddSingleton<IValidator<ConvertCurrencyRequest>, ConvertCurrencyValidator>();

        // services.AddValidatorsFromAssemblyContaining<ConvertCurrencyValidator>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlerMiddleware>();

        // app.UseHttpsRedirection();
        app.UseRouting();
        // app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); });
    }
}