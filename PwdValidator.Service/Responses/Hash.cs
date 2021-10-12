using System.Security.Policy;
using RepoDb.Attributes;

namespace PwdValidator.Service.Responses
{
    
    public class Hash
    {
        
        public Hash()
        { }

        public Hash(string value, int count)
        {
            Value = value;
            Count = count;
        }
        
        /// <summary>
        /// The password as a SHA-1 encrypted string
        /// </summary>
        public string Value { get; }
        
        /// <summary>
        /// The prevalence of the password.
        /// Phrased differently: The total number of cases in the dataset of known breaches at a specific time. 
        /// </summary>
        public int Count { get; }
        
    }
}