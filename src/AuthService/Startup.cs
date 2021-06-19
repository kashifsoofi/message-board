namespace AuthService
{
    using System;
    using System.Linq;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var provider = Configuration.GetValue<string>("Database:Provider");

            // Add IdentityServer
            services.AddIdentityServer()
                .AddTestUsers(Config.TestUsers)
                .AddConfigurationStore(options =>
                {
                    switch (provider)
                    {
                        case "Sqlite":
                            options.ConfigureDbContext = b => b.UseSqlite(
                                Configuration.GetConnectionString("SqliteConfigurationConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly("AuthService.Migrations.Sqlite"));
                            break;
                        case "SqlServer":
                            options.ConfigureDbContext = b => b.UseSqlServer(
                                Configuration.GetConnectionString("SqlServerConfigurationConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly("AuthService.Migrations.SqlServer"));
                            break;
                        default:
                            throw new Exception($"Unsupported provider: [{provider}]");
                    }
                })
                .AddOperationalStore(options =>
                {
                    switch (provider)
                    {
                        case "Sqlite":
                            options.ConfigureDbContext = b => b.UseSqlite(
                                Configuration.GetConnectionString("SqlitePersistedGrantConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly("AuthService.Migrations.Sqlite"));
                            break;
                        case "SqlServer":
                            options.ConfigureDbContext = b => b.UseSqlServer(
                                Configuration.GetConnectionString("SqlServerPersistedGrantConnection"),
                                sqlOptions => sqlOptions.MigrationsAssembly("AuthService.Migrations.SqlServer"));
                            break;
                        default:
                            throw new Exception($"Unsupported provider: [{provider}]");
                    }
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService v1"));
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var persistedGrantContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                persistedGrantContext.Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
