using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using PwdValidator.Service.Actions;
using PwdValidator.Service.Exceptions;
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
            catch (Exception exception) 
                when (exception is CommandParsingException || 
                      exception is MissingArgumentException ||
                      exception is FileNotFoundException)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception unknownEx)
            {
                Console.WriteLine($"Unknown exception: {unknownEx}");
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
                    ActionBuilder.Execute<ActionSetupDb>(new ActionSetupDbOptions() { Overwrite = overwrite });
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

                var startFromOption = command.Option("-s|--start-from <integer-value>",
                    "Row-number to start importing from",
                    CommandOptionType.SingleValue);

                var ignoreDuplicateExceptionOption = command.Option("-id|--ignore-duplicates",
                    "Choose whether to ignore duplication exceptions from the database", 
                    CommandOptionType.SingleValue);
                
                command.OnExecute(() =>
                {
                    if (locationArgument.Values == null || locationArgument.Values.Count == 0)
                        throw new MissingArgumentException("Source-file not specified or invalid...");
                    var sourceFile = locationArgument.Values[0];
                    
                    var numberOfRecordsToImport = recordCountOption.HasValue()
                        ? Convert.ToInt32(recordCountOption.Value())
                        : int.MaxValue;
                    var minimalOccurenceCount = minimalOccurenceCountOption.HasValue()
                        ? Convert.ToInt32(minimalOccurenceCountOption.Value())
                        : Constants.DEFAULT_MIN_PREVALENCE;
                    var startFrom = startFromOption.HasValue() 
                        ? Convert.ToInt32(startFromOption.Value()) 
                        : 0;
                    var ignoreDuplicates = ignoreDuplicateExceptionOption.HasValue() && Convert.ToBoolean(ignoreDuplicateExceptionOption.Value());

                    try
                    {
                        ActionBuilder.Execute<ActionPopulateDb>( new ActionPopulateDbOptions()
                        {
                            Source = sourceFile,
                            IgnoreDuplicates = ignoreDuplicates,
                            Limit = numberOfRecordsToImport,
                            MinPrevalance = minimalOccurenceCount,
                            StartFromRow = startFrom
                        });
                        
                        return 0;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        Console.WriteLine(e.Message);
                        
                        return -1;
                    }
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