using RepoDb.DbSettings;

namespace PwdValidator.Service.Utilities
{
    public sealed class SqlServerDbSetting : BaseDbSetting
    {
        public SqlServerDbSetting()
            : base()
        {
            AreTableHintsSupported = true;
            AverageableType = typeof(double);
            ClosingQuote = "]";
            DefaultSchema = "dbo";
            IsDirectionSupported = true;
            IsExecuteReaderDisposable = true;
            IsMultiStatementExecutable = true;
            IsPreparable = true;
            IsUseUpsert = false;
            OpeningQuote = "[";
            ParameterPrefix = "@";
            SchemaSeparator = ".";
        }
        
    }
    
}