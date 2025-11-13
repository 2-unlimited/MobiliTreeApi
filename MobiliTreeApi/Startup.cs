using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MobiliTreeApi.Shared.Domain;
using MobiliTreeApi.CreateSession;
using MobiliTreeApi.CreateSession.Domain;
using MobiliTreeApi.Events;
using MobiliTreeApi.GetSessions;
using MobiliTreeApi.GetInvoices;
using MobiliTreeApi.GetInvoices.Domain;
using MobiliTreeApi.Shared.ServiceBus;
using MobiliTreeApi.Shared.Projection;
using MobiliTreeApi.Shared.Storage;

namespace MobiliTreeApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Fake Infrastructure (Can be ignored)
            services.AddSingleton<IFakeMessageBus, FakeMessageBus>();
            services.AddHostedService<FakeMessageBusProcessor>();
            services.AddSingleton<IFakeBaseSessionStorage, FakeBaseSessionStorage>();
            services.AddSingleton<GetInvoices.Storage.IParkingFacilityRepository>(_ =>
                new GetInvoices.Storage.ParkingFacilityRepository(FakeData.GetSeedServiceProfiles()));
            services.AddSingleton(_ => FakeData.GetSeedCustomers());

            // 'Real' Shared Infrastructure Services
            services.AddSingleton<ISessionStorage, SessionStorage>();
            services.AddTransient<ISessionManagement, SessionManagement>();
            services.AddTransient<ISessionProjection, SessionProjection>();
            services.AddTransient<ISessionAggregateRepository, FakeSessionAggregateRepository>();
            services.AddTransient<IHandleMessages<SessionCreated>, SessionCreatedHandler>();
            services.AddTransient<IInvoiceManagement, InvoiceManagement>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCreateSessionApi();
                endpoints.MapGetSessionsApi();
                endpoints.MapGetInvoicesApi();
            });
        }
    }
}
