using Dapper;
using Microsoft.Data.Sqlite;

namespace WebAPIproducts.Database
{
   public class DatabaseBootstrap : IDatabaseBootstrap
   {
      private readonly DatabaseConfig databaseConfig;

      public DatabaseBootstrap(DatabaseConfig databaseConfig)
      {
         this.databaseConfig = databaseConfig;
      }

      public void Setup()
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Products';");
         var tableName = table.FirstOrDefault();
         if (!string.IsNullOrEmpty(tableName) && tableName == "Products")
            return;

         // connection.Execute("Create Table Product (" +
         //    "Name VARCHAR(100) NOT NULL," +
         //    "Description VARCHAR(1000) NULL);");

         connection.Execute("CREATE TABLE Products (" +
             "Id INTEGER," +
             "Name TEXT," +
             "Inventory INTEGER," +
             "Price REAL," +
             "PRIMARY KEY(\"Id\" AUTOINCREMENT));");
      }

   }
}
