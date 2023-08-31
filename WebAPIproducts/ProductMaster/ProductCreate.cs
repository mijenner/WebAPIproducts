using Dapper;
using Microsoft.Data.Sqlite;
using WebAPIproducts.Database;

namespace WebAPIproducts.ProductMaster
{
   public class ProductCreate : IProductCreate
   {
      private readonly DatabaseConfig databaseConfig;

      public ProductCreate(DatabaseConfig databaseConfig)
      {
         this.databaseConfig = databaseConfig;
      }

      public async Task<bool> Create(Product product)
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         int affectedRows = await connection.ExecuteAsync("INSERT INTO Products (Name, Inventory, Price)" +
             "VALUES (@Name, @Inventory, @Price);", product);

         return affectedRows > 0;
      }

   }
}
