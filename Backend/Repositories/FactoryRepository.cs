using Microsoft.Data.Sqlite;

namespace Backend.Repositories
{
    public class FactoryRepository
    {
        public SqliteConnection GetConnection()
        {
            return new SqliteConnection("Data Source=bin/Debug/net9.0/MeuBanco.db");
        }
    }
}
