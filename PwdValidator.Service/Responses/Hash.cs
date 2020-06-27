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
        
        public string Value { get; set; }
        
        public int Count { get; set; }
        
    }
}