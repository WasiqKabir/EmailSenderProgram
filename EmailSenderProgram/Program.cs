using BusinessServices.CustomerService;
using BusinessServices.EmailSender;
using BusinessServices.Repositories;
using Common;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;

namespace EmailSenderProgram
{
    internal class Program
    {
        //private readonly CustomerRepository _customerRepository;
        private readonly ICustomerRepository _customerRepository;

        public Program(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        /// <summary>
        /// This application is run everyday
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Send Welcomemail");
            var serviceProvider = Startup.ConfigureServices();
            var customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();
            Program program = new Program(customerRepository);

            await program.RunProgram();

            Console.ReadKey();
        }


        private async Task RunProgram()
        {
            try
            {
                // using await if second one is dependent on first one else not required
                //bool success = await _customerRepository.SendEmailToNewCustomers();
                bool success = await _customerRepository.SendEmailToNewCustomers();

                if (!success)
                    Console.WriteLine("Something went wrong at SendEmailToNewCustomers");

                //success = await _customerRepository.CustomerMailSender("EOComebackToUs");
                success = await _customerRepository.CustomerMailSender(VoucherCode.ComeBackToUs);

                if (!success)
                    Console.WriteLine("Something went wrong at CustomerMailSender");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
        }


    }
}