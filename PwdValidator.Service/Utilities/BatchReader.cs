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
            
            string line;
            string hash;
            int prevalance;
            while ((line = sr.ReadLine()) != null)
            {
                lineCounter++;

                // If we are below the row-number from which we are requested to import, continue with the next record
                if (options.StartFromRow > lineCounter) continue;

                hash = line.Substring(0, line.IndexOf(":"));
                prevalance = Convert.ToInt32(line.Substring(line.IndexOf(":") + 1));

                // Don't bother with records that have a lower occurence then what we consider 'safe'
                if (prevalance < options.MinPrevalance) continue;

                currentCounter++;
                if (currentCounter % Constants.DEFAULT_WRITE_OUTPUT_AFTER_X_RECORDS == 0)
                {
                    Log.Information($"Records inserted: {currentCounter} at {DateTime.Now}");
                    Console.WriteLine($"Records inserted: {currentCounter} at {DateTime.Now}");
                }

                try
                {
                    dbConnection.Insert(new Hash(hash, prevalance));
                }
                catch (Exception exception) when (exception is SqlException && options.IgnoreDuplicates)
                {
                    if (!exception.Message.Contains("duplicate key"))
                        throw;
                }
            
                // If we have passed the requested number of records to import, then abort 
                if (currentCounter > options.Limit) return;
            }
        }
    }
    
}