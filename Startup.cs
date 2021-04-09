using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Services.CalculatorService;
using VitoshaBank.Services.CalculatorService.Interfaces;
using VitoshaBank.Services.ChargeAccountService;
using VitoshaBank.Services.ChargeAccountService.Interfaces;
using VitoshaBank.Services.CreditService;
using VitoshaBank.Services.CreditService.Interfaces;
using VitoshaBank.Services.DebitCardService;
using VitoshaBank.Services.DebitCardService.Interfaces;
using VitoshaBank.Services.DepositService;
using VitoshaBank.Services.DepositService.Interfaces;
using VitoshaBank.Services.InterestService;
using VitoshaBank.Services.InterestService.Interfaces;
using VitoshaBank.Services.SupportTicketService;
using VitoshaBank.Services.SupportTicketService.Interfaces;
using VitoshaBank.Services.TransactionService;
using VitoshaBank.Services.TransactionService.Interfaces;
using VitoshaBank.Services.UserService;
using VitoshaBank.Services.UserService.Interfaces;
using VitoshaBank.Services.WalletService;
using VitoshaBank.Services.WalletService.Interfaces;


namespace VitoshaBank
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

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IDepositsService, DepositsService>();
            services.AddScoped<ICreditsService, CreditsService>();
            services.AddScoped<IWalletsService, WalletsService>();
            services.AddScoped<IDebitCardsService, DebitCardsService>();
            services.AddScoped<ISupportTicketsService, SupportTicketsService>();
            services.AddScoped<IChargeAccountsService, ChargeAccountsService>();
            services.AddScoped<ICalculatorService, CalculatorService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<ICreditPayOff, CreditPayOff>();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddControllersWithViews();
            services.AddSwaggerGen();

            services.AddDbContext<BankSystemContext>(options => options.UseNpgsql(Configuration.GetConnectionString("BankConnection")));
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "frontend/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "frontend";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
