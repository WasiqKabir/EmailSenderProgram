using BusinessServices.CustomerService;
using BusinessServices.EmailSender;
using BusinessServices.Repositories;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderProgram
{
    public static class Startup
    {
        public static ServiceProvider ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<ICustomerRepository, CustomerService>()
                .AddScoped<IEmailSenderRepository, EmailSenderService>()
                .AddScoped<IDataLayer, DataLayer>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
