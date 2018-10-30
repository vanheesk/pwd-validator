using PasswordValidatorService.DTO;
using PasswordValidatorService.Utilities;

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