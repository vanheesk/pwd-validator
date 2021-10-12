using System.Linq;
using PwdValidator.Service.Responses;
using PwdValidator.Service.Utilities;
using RepoDb;

namespace PwdValidator.Service.Services
{
    public class HashService
    {
        public Hash GetHashInfo(string hashValue)
        {
            var connection = DbContextFactory.GetInstance().GetConnection();

            var result= connection
                .Query<Hash>(h => h.Value == hashValue)
                .FirstOrDefault();
                
            return result;
        }
        
    }
    
}