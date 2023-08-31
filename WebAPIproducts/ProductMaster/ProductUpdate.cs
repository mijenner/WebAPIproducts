using Dapper;
using Microsoft.Data.Sqlite;
using WebAPIproducts.Database;

namespace WebAPIproducts.ProductMaster
{
   public class ProductUpdate : IProductUpdate
   {
      private readonly DatabaseConfig databaseConfig;

      public ProductUpdate(DatabaseConfig databaseConfig)
      {
         this.databaseConfig = databaseConfig;
      }

      public async Task<bool> Update(Product product)
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         int affectedRows = await connection.ExecuteAsync("UPDATE Products SET Name = @Name, Inventory = @Inventory, Price = @Price WHERE Id = @Id;", product);

         return affectedRows > 0; 

      }

   }
}
