using Application.IAppServices;
using Application.IValidator;
using Application.Mappings;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.Interfaces;
using Domain.IRepositories;
using Domain.UnitOfWork;
using Infraestructure.Application.AppServices;
using Infraestructure.Application.Validator;
using Infraestructure.Domain.Repositories;
using Infraestructure.Domain.UnitOfWork;
using Infraestructure.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.DependencyInjectionExtensions
{
    /// <summary>
    /// IServiceCollecion extensions, encapsulation of adding application services to the Program/StartUp class
    /// no matter what net. core application client is running.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AutoMapper with Application profiles
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperWithProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(configuration =>
            {
                configuration.AddExpressionMapping();
                configuration.AddProfile<CaoClienteProfile>();
                configuration.AddProfile<CaoFaturaProfile>();
                configuration.AddProfile<CaoOProfile>();
                configuration.AddProfile<CaoSalarioProfile>();
                configuration.AddProfile<CaoSistemaProfile>();
                configuration.AddProfile<CaoUsuarioProfile>();
                configuration.AddProfile<PermissaoSistemaProfile>();
                AppDomain.CurrentDomain.GetAssemblies();

            });
        }
        /// <summary>
        /// Adds Unit of Work, AppServices and Repositories.
        /// </summary>
        /// <param name="services"></param>
        public static void AddEntitiesServicesAndRepositories(this IServiceCollection services)
        {
            /*Why scoped ?
             * Lifetime of Unit of Work should be: 
             * 1. long enough to contain all changes in a single block of transacctions (tipically a request), and
             * 2. short enough to avoid interfere with other changes carried out in another block of transacctions
             * AddSingleton -> Application Lifetime
             * AddScoped -> Request Lifetime
             * AddTransient -> Every time is invoked.
             * Unit of Work should be Scoped. And because is injected into Repositories these ones should be scoped as well.
             * When scoped services injected into singleton services their life time is altered and become singleton.
             * Also .Net's build in dependency injection, validates such a rule, throwing an exception.
             */

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWorkContainer>();

            //AppServices
            services.AddScoped<ICaoClienteAppService, CaoClienteAppService>();
            services.AddScoped<ICaoFaturaAppService, CaoFaturaAppService>();
            services.AddScoped<ICaoOAppService, CaoOAppService>();
            services.AddScoped<ICaoSalarioAppService, CaoSalarioAppService>();
            services.AddScoped<ICaoSistemaAppService, CaoSistemaAppService>();
            services.AddScoped<ICaoUsuarioAppService, CaoUsuarioAppService>();
            services.AddScoped<IPermissaoSistemaAppService, PermissaoSistemaAppService>();

            //Repositories
            services.AddScoped<ICaoClienteRepository, CaoClienteRepository>();
            services.AddScoped<ICaoFaturaRepository, CaoFaturaRepository>();
            services.AddScoped<ICaoORepository, CaoORepository>();
            services.AddScoped<ICaoSalarioRepository, CaoSalarioRepository>();
            services.AddScoped<ICaoSistemaRepository, CaoSistemaRepository>();
            services.AddScoped<ICaoUsuarioRepository, CaoUsuarioRepository>();
            services.AddScoped<IPermissaoSistemaRepository, PermissaoSistemaRepository>();
        }
        /// <summary>
        /// Adds custom Application services
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomApplicationServices(this IServiceCollection services)
        {
            //IValidator
            services.AddScoped<IEntityValidator, DataAnnotationsEntityValidator>();
            //Property Checker
            services.AddScoped<IPropertyCheckerService, PropertyCheckerService>();

            // Inject here any other service from the Application
            services.AddScoped<IDateTimeService, DateTimeService>();
        }
    }
}
