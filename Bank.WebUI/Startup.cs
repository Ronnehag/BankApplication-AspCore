using System;
using System.Text;
using Bank.Application.Bank.Queries.GetBankStatistics;
using Bank.Application.Enumerations;
using Bank.Application.Interfaces;
using Bank.Common;
using Bank.Infrastructure;
using Bank.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Bank.WebUI
{
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<IDateTime, MachineDateTime>();

            // Setting up the DB context using SqlServer
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<BankAppDataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            }
            else
            {
                services.AddDbContext<BankAppDataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }

            // Adding scoped context
            services.AddScoped(typeof(IBankDbContext), typeof(BankAppDataContext));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BankAppDataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;

                // SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            // Add MediatR using dependency injection
            services.AddMediatR(typeof(GetBankStatisticsQueryHandler));

            // Setting upp Authorization policys
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policyBuilder =>
                    policyBuilder.RequireClaim(Claims.Admin, "true"));
                options.AddPolicy("Cashier",
                    policyBuilder => policyBuilder.RequireClaim(Claims.Cashier, "true"));
            });

            // Adding Authentication using Core Identity default authentication as well as JwtBearers scheme for the API-controller
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SecretKey"])),
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Audiance"]
                    };
                });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/User/Login";
                opt.LogoutPath = "/User/Logout";
                opt.AccessDeniedPath = "/User/Denied";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "customer_details",
                    template: "customer/{id:int?}",
                    defaults: new
                    {
                        controller = "Customer",
                        action = "Index",
                    });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
