using PasswordValidatorService.Utilities;
using PwdValidator.Service.Responses;

namespace PasswordValidatorService.Services
{
    public class HashService
    {
        public HashInfo GetHashInfo(string hashValue)
        {
            return DatabaseHelper.Instance.GetHashInfo(hashValue);
        }
    }
}