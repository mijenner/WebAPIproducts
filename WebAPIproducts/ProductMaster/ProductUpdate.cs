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

         // First, retrieve the existing product details
         //var productRead = new ProductRead(databaseConfig);
         //var existingProduct = await productRead.ReadById(product.Id);
         //if (existingProduct == null)
         //{
         //   // throw new Exception($"Product with Id {product.Id} not found.");
         //   Console.WriteLine("Product with Id " + product.Id + " not found.");
         //   return;
         //}

         int affectedRows = await connection.ExecuteAsync("UPDATE Products SET Name = @Name, Inventory = @Inventory, Price = @Price WHERE Id = @Id;", product);

         return affectedRows > 0; 

      }

   }
}
