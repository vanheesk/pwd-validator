using MCMSPasswordValidator.DTO;
using MCMSPasswordValidator.Utilities;

namespace MCMSPasswordValidator.Services
{
    public class HashService
    {
        public HashInfo GetHashInfo(string hashValue)
        {
            return DatabaseHelper.Instance.GetHashInfo(hashValue);
        }
    }
}