using System;
using System.Globalization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PwdValidator.Service.Actions;
using PwdValidator.Service.Utilities;
using Serilog;

namespace PwdValidator.Service
{
    
    public class Program
    {
        public static IConfiguration Configuration;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Application started at {0})", new object[] {DateTime.Now.ToString(CultureInfo.InvariantCulture)});
            
            InitializeConfiguration(args);
            InitializeLogger();
            
            Log.Information("Running on {0}", new object[]{ Configuration["OS"] });

            // Setup Application Options
            SetupConsoleApplicationOptions(args);
            
            Log.Information("Application stopped...");
        }

        /// <summary>
        /// Initializes the configuration so we can easily work with any of the information in either settings-file
        /// or passed in through command-line options.
        /// </summary>
        /// <param name="args">Command-line options</param>
        private static void InitializeConfiguration(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }
        
        /// <summary>
        /// Creates the logging component based on settings specified in the appsettings.json file.
        /// </summary>
        private static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }
        
        private static void SetupConsoleApplicationOptions(string[] args)
        {
            try
            {
                var app = new CommandLineApplication();
                app.Name = "ARKE.Password-Validator";
                app.Description = "Password validator based on the HaveIBeenPwned database.";

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

        private static void CreateCommandForSetupAction(CommandLineApplication app)
        {
            Log.Verbose("Start creating command [setup].");
            
            app.Command("setup", (command) =>
            {
                command.Description = "Performs the SETUP step which initializes the embedded database";
                command.HelpOption("-?|-h|--help");

                var overwriteOption = command.Option("-o|--overwrite <true/false>",
                    "Overwrite an existing database.",
                    CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    var overwrite = overwriteOption.HasValue() && Convert.ToBoolean(overwriteOption.Value());
                    ActionBuilder.Execute<ActionSetupDb>(new string[] { overwrite.ToString() });
                    return 0;
                });
            });
            
            Log.Verbose("Finished creating command [setup].");
        }

        private static void CreateCommandForPopulateAction(CommandLineApplication app)
        {
            Log.Verbose("Start creating command [populate].");
            
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
                    
                    ActionBuilder.Execute<ActionPopulateDb>(new string[] { sourceFile, numberOfRecordsToImport.ToString(), minimalOccurenceCount.ToString() });
                    return 0;
                });
            });
            
            Log.Verbose( "Finished creating command [populate].");
        }

        private static void CreateCommandForRunAsServiceAction(CommandLineApplication app)
        {
            Log.Verbose("Start creating command [run-as-service].");
            
            app.Command("service", (command) =>
            {
                command.Description =
                    "Performs the SERVICE step which starts the application as a web service application";
                command.HelpOption("-?|-h|--help");

                command.OnExecute(() =>
                {
                    ActionBuilder.Execute<ActionRunAsService>(null);
                    return 0;
                });
            });
            
            Log.Verbose("Finished creating command [run-as-service].");
        }

    }
    
}