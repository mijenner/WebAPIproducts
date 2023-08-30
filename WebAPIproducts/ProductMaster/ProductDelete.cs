using Dapper;
using Microsoft.Data.Sqlite;
using WebAPIproducts.Database;

namespace WebAPIproducts.ProductMaster
{
   public class ProductDelete : IProductDelete
   {
      private readonly DatabaseConfig databaseConfig;

      public ProductDelete(DatabaseConfig databaseConfig)
      {
         this.databaseConfig = databaseConfig;
      }

      public async Task<bool> Delete(int productId)
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         int affectedRows = await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @ProductId;", new { ProductId = productId });

         return affectedRows > 0;
      }

   }
}
