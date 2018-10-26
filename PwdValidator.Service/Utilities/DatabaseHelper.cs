using System;
using System.Data;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using MCMSPasswordValidator.DTO;

namespace MCMSPasswordValidator.Utilities
{
    
    public class DatabaseHelper
    {
        
        private const string DB_FILENAME = @"PasswordsDB.FDB";
        private const string DB_DATASOURCE = @"HASHVALUES";
        private const string DB_USERNAME = @"my_username";
        private const string DB_PASSWORD = @"my_password";
        private const string DB_CLIENTLIBRARY = @".\Firebird\fbclient.dll";

        private static DatabaseHelper _instance;
        private FbConnection dbConnection;

        private string sqlStatement;
        private FbCommand cmd;
        
        private DatabaseHelper()
        { }

        public static DatabaseHelper Instance
        {
            get            
            {
                if (_instance == null)
                    _instance = new DatabaseHelper();
                
                if (_instance.dbConnection == null)
                    _instance.dbConnection = new FbConnection(_instance.ConnectionString);

                if (_instance.cmd == null)
                    _instance.cmd = new FbCommand {Connection = _instance.dbConnection};

                return _instance ;
            }
        }
        
        private string ConnectionString
        {
            get
            {
                var connectionString = new FbConnectionStringBuilder
                {
                    Database = DB_FILENAME,
                    DataSource = DB_DATASOURCE,
                    UserID = DB_USERNAME,
                    Password = DB_PASSWORD,
                    ServerType = FbServerType.Embedded,
                    Charset = "UTF8",
                    ClientLibrary = DB_CLIENTLIBRARY
                };

                return connectionString.ConnectionString;
            }
        } 

        public void CreateDatabase(bool overwrite = true)
        {
            if (File.Exists(DB_FILENAME) && overwrite)
                File.Delete(DB_FILENAME);                

            if (File.Exists(DB_FILENAME)) 
                return;
            
            FbConnection.CreateDatabase(ConnectionString);
            CreateTables();
        }

        private void CreateTables()
        {
            sqlStatement = "CREATE TABLE PasswordHash (hash varchar(50) NOT NULL, countOccurrence int NOT NULL)";

            OpenConnection();
            
            var cmd = new FbCommand(sqlStatement, dbConnection);             
            cmd.ExecuteNonQuery();
        }

        public void OpenConnection()
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
        }

        public void CloseConnection()
        {
            if (dbConnection.State != ConnectionState.Closed)
                dbConnection.Close();
        }

        public void InsertPasswordHash(string hashValue, string countOccurrences)
        {
            sqlStatement = $"INSERT INTO PasswordHash (hash, countOccurrence) VALUES ('{hashValue}', {countOccurrences})";

            cmd.CommandText = sqlStatement;            
            cmd.ExecuteNonQuery();            
        }

        public HashInfo GetHashInfo(string hashValue)
        {
            OpenConnection();
            
            var hashInfo = new HashInfo();

            sqlStatement = $"SELECT hash, countOccurence FROM PasswordHash WHERE hash = '{hashValue}'";

            cmd.CommandText = sqlStatement;
            using (var reader = cmd.ExecuteReader())
            {
                hashInfo.HashValue = reader.GetValue(0).ToString();
                hashInfo.Count = (short)reader.GetValue(1);
            }
            
            CloseConnection();
            
            return hashInfo;
        }
        
    }
}