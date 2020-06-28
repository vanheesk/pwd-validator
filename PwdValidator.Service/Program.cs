using System;
using System.Globalization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PwdValidator.Service.Actions;
using PwdValidator.Service.Utilities;

namespace PwdValidator.Service
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            FileLogger.Write(LogLevel.INFO, "Application started at {0})", new object[] {DateTime.Now.ToString(CultureInfo.InvariantCulture)}, true);
            
            // Setup Configuration
            ConfigurationHelper.Instance().Init(args);
            FileLogger.Write(LogLevel.INFO, "Running on {0}", new object[]{ ConfigurationHelper.Instance().GetValue("OS") });

            // Setup Application Options
            SetupConsoleApplicationOptions(args);
            
            FileLogger.Write(LogLevel.INFO, "Application stopped...");
        }

        private static void SetupConsoleApplicationOptions(string[] args)
        {
            try
            {
                var app = new CommandLineApplication();
                app.Name = "ARKE.PasswordValidator";
                app.Description = ".NET Core console app allowing to validate password security.";

                app.HelpOption("-?|-h|--help");
                
                CreateCommandForSetupAction(app);
                CreateCommandForPopulateAction(app);
                CreateCommandForRunAsServiceAction(app);

                app.Execute(args);
            }
            catch (CommandParsingException cpe)
            {
                Console.WriteLine(cpe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown exception: {e}");
            }
        }

        /// <summary>
        /// Check the arguments passed in match a request to populate a database based on the provided textfile
        /// </summary>
        private static void ActionPopulate(string sourceFile, int numberOfRecordsToImport = int.MaxValue, int minimalOccurenceCount = 1)
        {
            FileLogger.Write(LogLevel.INFO, numberOfRecordsToImport == int.MaxValue
                ? $"Populate DB requested without record limit"
                : $"Populate DB requested with option recordcount set to {numberOfRecordsToImport}");
            
            FileLogger.Write(LogLevel.INFO, minimalOccurenceCount == 1
                ? $"Minimal occurence count set to include all"
                : $"Minimal occurence count set to {minimalOccurenceCount}");
            
            var runner = new ActionPopulateDb();
            runner.Execute(new string[] { sourceFile, numberOfRecordsToImport.ToString(), minimalOccurenceCount.ToString()});
        }

        private static void CreateCommandForSetupAction(CommandLineApplication app)
        {
            app.Command("setup", (command) =>
            {
                command.Description = "Performs the SETUP step which initializes the embedded database";
                command.HelpOption("-?|-h|--help");

                var overwriteOption = command.Option("-o|--overwrite <true/false>",
                    "Overwrite an existing database.",
                    CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    var overwriteExisting = overwriteOption.HasValue() && Convert.ToBoolean(overwriteOption.Value());
                    ActionSetup(overwriteExisting);

                    return 0;
                });
            });
            
            FileLogger.Write(LogLevel.DEBUG, "Finished creating command [setup].");
        }

        private static void CreateCommandForPopulateAction(CommandLineApplication app)
        {
            app.Command("populate", (command) =>
            {
                command.Description =
                    "Performs the POPULATE step which populates the database with data from the specified textfile";
                command.HelpOption("-?|-h|--help");

                var locationArgument = command.Argument("[sourcefile]",
                    "The sourcefile to use for populating the service's database.");

                var recordCountOption = command.Option("-rc|--rc <integer-value>",
                    "Number of records to import. If no value is specified, all data will be imported.",
                    CommandOptionType.SingleValue);

                var minimalOccurenceCountOption = command.Option("-m|--min <integer-value>",
                    "Minimum prevalence counter required to be considered 'unsafe'",
                    CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    var sourceFile = locationArgument.Values[0];
                    var numberOfRecordsToImport = recordCountOption.HasValue()
                        ? Convert.ToInt32(recordCountOption.Value())
                        : int.MaxValue;
                    var minimalOccurenceCount = minimalOccurenceCountOption.HasValue()
                        ? Convert.ToInt32(minimalOccurenceCountOption.Value())
                        : 1;
                    ActionPopulate(sourceFile, numberOfRecordsToImport, minimalOccurenceCount);

                    return 0;
                });
            });
            
            FileLogger.Write(LogLevel.DEBUG, "Finished creating command [populate].");
        }

        private static void CreateCommandForRunAsServiceAction(CommandLineApplication app)
        {
            app.Command("service", (command) =>
            {
                command.Description =
                    "Performs the SERVICE step which starts the application as a web service application";
                command.HelpOption("-?|-h|--help");

                command.OnExecute(() =>
                {
                    ActionRunAsService();

                    return 0;
                });
            });
            
            FileLogger.Write(LogLevel.DEBUG, "Finished creating command [run-as-service].");
        }

        /// <summary>
        /// Check the arguments passed in match a request to setup a database 
        /// </summary>
        private static void ActionSetup(bool overwrite)
        {                         
            FileLogger.Write(LogLevel.INFO, $"Setup requested with option overwrite set to {overwrite}");

            var runner = new ActionSetupDb();
            runner.Execute();
        }

        /// <summary>
        /// Check the arguments passed in match a request to run the application as an API service
        /// </summary>
        private static void ActionRunAsService()
        {
            FileLogger.Write(LogLevel.INFO, $"Run as service requested");
            
            CreateWebHostBuilder(new string[0]).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        
        private static IHostBuilder CreateHostBuilder<T> (string[] args) where T : class, IHostedService =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { services.AddHostedService<T>(); });
    }
    
}