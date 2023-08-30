namespace WebAPIproducts.ProductMaster
{
   public interface IProductDelete
   {
      Task<bool> Delete(int productId);
   }
}
