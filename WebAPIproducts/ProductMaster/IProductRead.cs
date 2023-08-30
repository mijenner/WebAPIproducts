namespace WebAPIproducts.ProductMaster
{
   public interface IProductRead
   {
      Task<IEnumerable<Product>> Read();
      Task<Product> ReadById(int productId);
   }
}
