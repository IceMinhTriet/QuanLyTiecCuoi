using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    public class DbConnection
    {
        const string SERVERNAME = "127.0.0.1";
        private string _serverName;
        private string _databaseName;
        private string _userName;
        private string _password;

        public DbConnection(string serverName, string databaseName)
        {
            _serverName = serverName;
            _databaseName = databaseName;
        }

        public DbConnection(string serverName, string databaseName, string userName, string password)
        {
            _serverName = serverName;
            _databaseName = databaseName;
            _userName = userName;
            _password = password;
        }

        public DbConnection(string database, string userName, string password)
        {
            _userName = userName;
            _password = password;
            _databaseName = database;
        }

        public string GetConnectionString()
        {
            _serverName = string.IsNullOrEmpty(_serverName) ? SERVERNAME : _serverName;

            if (_userName == null)
            {
                return @"Data Source=" + _serverName + ";Initial Catalog=" + _databaseName + ";Integrated Security=true";
            }
            else
            {
                return @"Data Source=" + _serverName + ";Initial Catalog=" + _databaseName + ";User Id=" + _userName + ";Password=" + _password;
            }
        }
    }
}
