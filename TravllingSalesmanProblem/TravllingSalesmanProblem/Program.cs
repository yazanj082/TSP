﻿// C# program to implement
// traveling salesman problem
// using naive approach.
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using TravllingSalesmanProblem;

namespace TravllingSalesmanProblem
{

    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
                var host = CreateDefaultBuilder().Build();
                using IServiceScope serviceScope = host.Services.CreateScope();
                IServiceProvider provider = serviceScope.ServiceProvider;
                var workerInstance = provider.GetRequiredService<TaskHelper>();
                workerInstance.Execute();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            static IHostBuilder CreateDefaultBuilder()
            {
                return Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(app =>
                    {
                        app.AddJsonFile("appsettings.json");
                    }).ConfigureServices(services =>
                    {
                        services.AddSingleton<TaskHelper>();
                    });
            }
        }
        
    }
}