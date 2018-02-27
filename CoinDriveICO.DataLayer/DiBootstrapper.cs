using System;
using CoinDriveICO.DataLayer.Context;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories;
using CoinDriveICO.DataLayer.Repositories.Interfaces;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.DataLayer
{
    public static class DiBootstrapper
    {
        public static void RegisterDataLayer(this IServiceCollection services, DbContextSettings dbSettings)
        {
            services.AddDbContext<CoinDriveContext>(options => options.UseSqlServer(dbSettings.ConnectionString).EnableSensitiveDataLogging());
            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = true;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                }).AddEntityFrameworkStores<CoinDriveContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/SignOut";
            });

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            services.AddTransient<IInnerTransactionsRepository, InnerTransactionsRepository>();
            services.AddTransient<IAffiliatePayoffsRepository, AffiliatePayoffsRepository>();
        }
    }
}