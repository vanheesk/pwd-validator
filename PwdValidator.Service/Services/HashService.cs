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
            return DbContextFactory.GetInstance().GetConnection()
                .Query<Hash>(h => h.Value == hashValue)
                .FirstOrDefault();
        }
    }
}