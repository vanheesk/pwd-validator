using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using PasswordValidatorService.Utilities;

namespace PasswordValidatorService
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Application started at {DateTime.Now}");

            // Initialize the configuration class
            ConfigurationHelper.Instance().Init(args);

            Console.WriteLine($"Running on {ConfigurationHelper.Instance().GetValue("OS")}");

            SetupConsoleApplicationOptions(args);
                        
            Console.WriteLine($"Application stopped at {DateTime.Now}...");
        }

        private static void SetupConsoleApplicationOptions(string[] args)
        {
            try
            {
                var app = new CommandLineApplication();
                app.Name = "ARKE.PasswordValidator";
                app.Description = ".NET Core console app allowing to validate password security.";

                app.HelpOption("-?|-h|--help");

                app.Command("setup", (command) =>
                {
                    command.Description = "Performs the SETUP step which initializes the embedded database";
                    command.HelpOption("-?|-h|--help");

                    var overwriteOption = command.Option("-o|--overwrite <true/false>",
                        "Overwrite an existing database.",
                        CommandOptionType.SingleValue);

                    command.OnExecute(() =>
                    {
                        var overwriteExisting =
                            overwriteOption.HasValue() && Convert.ToBoolean(overwriteOption.Value());
                        ActionSetup(overwriteExisting);

                        return 0;
                    });
                });

                app.Command("populate", (command) =>
                {
                    command.Description =
                        "Performs the POPULATE step which populates the database with data from the specified textfile";
                    command.HelpOption("-?|-h|--help");

                    var locationArgument = command.Argument("[sourcefile]",
                        "The sourcefile to use for populating the service's database.");

                    var recordCountOption = command.Option("-rc|--rc <integer-value>",
                        "Overwrite an existing database.",
                        CommandOptionType.SingleValue);

                    var minimalOccurenceCountOption = command.Option("-m|--min <integer-value>",
                        "Minimal amount of occurenced required to be considered 'unsafe'",
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
        /// Check the arguments passed in match a request to setup a database 
        /// </summary>
        private static void ActionSetup(bool overwrite)
        {                         
            Console.WriteLine($"Setup requested with option overwrite set to {overwrite}");
                
            // Create (if not exists) the firebird embedded database (or overwrite is specified as argument)
            DatabaseHelper.Instance.CreateDatabase(overwrite);
        }

        /// <summary>
        /// Check the arguments passed in match a request to populate a database based on the provided textfile
        /// </summary>
        private static void ActionPopulate(string sourceFile, int numberOfRecordsToImport = int.MaxValue, int minimalOccurenceCount = 1)
        {
            Console.WriteLine(numberOfRecordsToImport == int.MaxValue
                ? $"Populate DB requested without record limit"
                : $"Populate DB requested with option recordcount set to {numberOfRecordsToImport}");

            Console.WriteLine(minimalOccurenceCount == 1
                ? $"Minimal occurence count set to include all"
                : $"Minimal occurence count set to {minimalOccurenceCount}");

            new FileParser().Parse(sourceFile, numberOfRecordsToImport, minimalOccurenceCount);
        }

        /// <summary>
        /// Check the arguments passed in match a request to run the application as an API service
        /// </summary>
        private static void ActionRunAsService()
        {
            Console.WriteLine($"Run as service requested");
            
            CreateWebHostBuilder(new string[0]).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        
    }
    
}