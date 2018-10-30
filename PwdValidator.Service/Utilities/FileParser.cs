using System.IO;

namespace PasswordValidatorService.Utilities
{
    
    public class FileParser
    {

        public void Parse(string filename, int recordCount)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();
            
            DatabaseHelper.Instance.OpenConnection();

            var currentCounter = 0;
            
            using (var fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var bs = new BufferedStream(fs))
            using (var sr = new StreamReader(bs))
            {               
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    currentCounter++;

                    line = line.Trim();
                    var array = line.Split(':');

                    DatabaseHelper.Instance.InsertPasswordHash(array[0], array[1]);
                    
                    if (currentCounter > recordCount)
                        return;
                }
            }
            
            DatabaseHelper.Instance.CloseConnection();
        }
        
    }
    
}