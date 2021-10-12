using System;
using System.IO;
using System.Data.SqlClient;
using PwdValidator.Service.Actions;
using PwdValidator.Service.Responses;
using RepoDb;
using Serilog;

namespace PwdValidator.Service.Utilities
{
    
    public class BatchReader
    {

        public void Read(ActionPopulateDbOptions options)
        {
            if (!File.Exists(options.Source))
                throw new FileNotFoundException($"File {options.Source} could not be found.");

            using var dbConnection = DbContextFactory.GetInstance().GetConnection();
            
            var currentCounter = 0;
            var lineCounter = 0;

            using var fs = File.Open(options.Source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var bs = new BufferedStream(fs);
            using var sr = new StreamReader(bs);
            
            // Create the variables to be used within the loop and make sure to only use a single allocation to increase performance.
            string line;
            string hash;
            int prevalence;
            int indexSeparator;
            
            // Create additional boolean operators to increase performance
            var shouldCheckStartingRow = options.StartFromRow != 1;
            var hasMinimumPrevalence = options.MinPrevalance != Constants.DEFAULT_MIN_PREVALENCE;
            var hasRowLimit = options.Limit != int.MaxValue;
            
            while ((line = sr.ReadLine()) != null)
            {
                lineCounter++;

                // If we are below the row-number from which we are requested to import, continue with the next record
                if (shouldCheckStartingRow && options.StartFromRow > lineCounter) continue;

                indexSeparator = line.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                hash = line.Substring(0, indexSeparator);
                prevalence = Convert.ToInt32(line.Substring(indexSeparator + 1));

                // Don't bother with records that have a lower occurence then what we consider 'safe'
                if (hasMinimumPrevalence && prevalence < options.MinPrevalance) continue;

                currentCounter++;
                if (currentCounter % Constants.DEFAULT_WRITE_OUTPUT_AFTER_X_RECORDS == 0)
                {
                    Log.Information($"Records inserted: {currentCounter} at {DateTime.Now}");
                    Console.WriteLine($"Records inserted: {currentCounter} at {DateTime.Now}");
                }

                dbConnection.Insert(new Hash(hash, prevalence));
            
                // If we have passed the requested number of records to import, then abort 
                if (hasRowLimit && currentCounter > options.Limit) return;
            }
        }
    }
    
}