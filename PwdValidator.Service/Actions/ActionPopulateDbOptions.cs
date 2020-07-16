namespace PwdValidator.Service.Actions
{
    public class ActionPopulateDbOptions : IActionOptions
    {
        
        public string Source { get; set; }
        public int Limit { get; set; }
        public int MinPrevalance { get; set; }
        public int? StartFromRow { get; set; }
        public bool IgnoreDuplicates { get; set; }
        
    }
}