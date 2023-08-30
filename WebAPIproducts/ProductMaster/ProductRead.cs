using Dapper;
using Microsoft.Data.Sqlite;
using WebAPIproducts.Database;

namespace WebAPIproducts.ProductMaster
{
   public class ProductRead : IProductRead
   {
      private readonly DatabaseConfig databaseConfig;

      public ProductRead(DatabaseConfig databaseConfig)
      {
         this.databaseConfig = databaseConfig;
      }

      public async Task<IEnumerable<Product>> Read()
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         return await connection.QueryAsync<Product>("SELECT Id, Name, Inventory, Price FROM Products;");
      }

      public async Task<Product> ReadById(int productId)
      {
         using var connection = new SqliteConnection(databaseConfig.Name);

         return await connection.QueryFirstOrDefaultAsync<Product>("SELECT Id, Name, Inventory, Price FROM Products WHERE Id = @ProductId;",
             new { ProductId = productId });
      }

   }
}
