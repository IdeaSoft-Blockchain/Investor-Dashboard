using CoinDriveICO.BusinessLayer.Services;
using CoinDriveICO.DataLayer;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.BusinessLayer
{
    public static class DiBootstrapper
    {
        public static void RegisterServices(this IServiceCollection services, DbContextSettings dbSettings)
        {
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAdapterApiService, AdapterApiService>();
            services.AddTransient<ITransactionWorkerService, TransactionWorkerService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.RegisterDataLayer(dbSettings);
        }
    }
}