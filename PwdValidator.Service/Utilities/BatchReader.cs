using System;
using System.IO;
using PwdValidator.Service.Responses;
using RepoDb;

namespace PwdValidator.Service.Utilities
{
    
    public class BatchReader
    {

        public void Read(string filename, int recordCount, int minimalCount)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            using var dbConnection = DbContextFactory.GetInstance().GetConnection();
            
            var currentCounter = 0;
            var lineCounter = 0;

            using var fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var bs = new BufferedStream(fs);
            using var sr = new StreamReader(bs);
            
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lineCounter++;
                if (lineCounter % 1000000 == 0)
                    Console.WriteLine($"Lines read: {lineCounter} at {DateTime.Now}");

                line = line.Trim();
                var array = line.Split(':');

                // Don't bother with records that have a lower occurence then what we consider 'safe'
                int.TryParse(array[1], out var countOccurrences);
                if (countOccurrences < minimalCount)
                    continue;

                currentCounter++;

                if (currentCounter == 1) // Useful for testing
                    Console.WriteLine($"First hash value found: {array[0]}");

                if (currentCounter % 100000 == 0)
                    Console.WriteLine($"Records inserted: {currentCounter} at {DateTime.Now}");
                
                dbConnection.Insert(new Hash(array[0], countOccurrences));

                if (currentCounter > recordCount)
                    return;
            }
        }
    }
    
}