namespace WebAPIproducts.ProductMaster
{
   public interface IProductUpdate
   {
      Task<bool> Update(Product product);
   }
}
