namespace WebAPIproducts.ProductMaster
{
   public interface IProductCreate
   {
      Task<bool> Create(Product product);
   }
}
