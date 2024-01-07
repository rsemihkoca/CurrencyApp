
using Api.Middleware;
using AutoMapper;
using Base.Models;
using Business.Cqrs;
using Business.Mapper;

// using AutoMapper;
// using FluentValidation.AspNetCore;
// using Microsoft.EntityFrameworkCore;
// using Data;
// using Business.Cqrs;
// using Business.Mapper;
// using Business.Validator;

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
        // services.AddSingleton<Base.Models.PositionOptions>();
        
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());
        //
        //
        // services.AddControllers().AddFluentValidation(x =>
        // {
        //     x.RegisterValidatorsFromAssemblyContaining<CreateCustomerValidator>();
        // });
        //
        services.AddSingleton<IUrlPaths, UrlPaths>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetAllSupportedCurrencies).Assembly));
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
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
