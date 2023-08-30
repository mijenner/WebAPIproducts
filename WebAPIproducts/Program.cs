using Microsoft.Extensions.Configuration;
using WebAPIproducts.Database;
using WebAPIproducts.ProductMaster;

namespace WebAPIproducts
{
   public class Program
   {
      public static void Main(string[] args)
      {
         // Database konfiguration and bootstrap: 
         var databaseConfig = new DatabaseConfig() { Name = "Data Source=productdb.sqlite" };
         var databaseBootstrap = new DatabaseBootstrap(databaseConfig);
         databaseBootstrap.Setup();

         // Create objects for CRUD operations: 
         var dbCreate = new ProductCreate(databaseConfig);
         var dbRead = new ProductRead(databaseConfig);
         var dbUpdate = new ProductUpdate(databaseConfig);
         var dbDelete = new ProductDelete(databaseConfig);
         // Add a little dummy-data to get us started: 
         // COMMENT OUT below once run a few times. 
         /* 
         dbCreate.Create(new Product() { Id = 1, Name = "Hest", Inventory = 40, Price = 40.3M });
         dbCreate.Create(new Product() { Id = 1, Name = "Ko", Inventory = 30, Price = 140.3M });
         dbCreate.Create(new Product() { Id = 1, Name = "Ged", Inventory = 20, Price = 240.3M });
         */


         // Setup web application (server): 
         var builder = WebApplication.CreateBuilder(args);

         // CORS
         builder.Services.AddCors();

         // Add services to the container.
         builder.Services.AddAuthorization();

         // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
         builder.Services.AddEndpointsApiExplorer();
         builder.Services.AddSwaggerGen();

         var app = builder.Build();

         // Configure the HTTP request pipeline.
         if (app.Environment.IsDevelopment())
         {
            app.UseSwagger();
            app.UseSwaggerUI();
         }

         app.UseHttpsRedirection();

         // CORS: 
         app.UseCors(
            options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
         );

         app.UseAuthorization();

         // CRUD part R - single 
         app.MapGet("/product/{id}", async (int id) =>
         {
            var product = await dbRead.ReadById(id);

            if (product != null)
            {
               return Results.Ok(product); // Return a 200 OK response with the product
            }
            else
            {
               return Results.NotFound(); // Return a 404 Not Found response
            }
         }).WithName("GetProductById").WithOpenApi();

         // CRUD part R - multiple 
         app.MapGet("/productlist", async () =>
            {
               var products = await dbRead.Read();

               if (products != null && products.Any())
               {
                  return Results.Ok(products);  // Return status code 200 
               }
               else
               {
                  return Results.NoContent(); // Return status code 204 
               }
            }).WithName("GetProductList").WithOpenApi();

         // CRUD - part U  
         app.MapPut("/product/{id}", async (int id, Product updatedProduct) =>
               {
                  updatedProduct.Id = id;
                  bool success = await dbUpdate.Update(updatedProduct);
                  if (success)
                  {
                     return Results.Ok(); // Successful update
                  }
                  else
                  {
                     return Results.NotFound(); // Item not found
                  }
               }).WithName("UpdateProduct").WithOpenApi();

         // CRUD - part C 
         app.MapPost("/product", async (Product newProduct) =>
         {
            bool success = await dbCreate.Create(newProduct);
            if (success)
            {
               return Results.Created("/product/", newProduct);
            }
            else
            {
               return Results.BadRequest(); // Bad request
            }
         })
         .WithName("CreateProduct")
         .WithOpenApi();

         // CRUD - part D 
         app.MapDelete("/product/{id}", async (int id) =>
         {
            bool success = await dbDelete.Delete(id);
            if (success)
            {
               return Results.NoContent(); // Successful deletion
            }
            else
            {
               return Results.NotFound(); // Item not found
            }
         }).WithName("DeleteProduct").WithOpenApi();

         IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Properties/launchSettings.json")
            .Build();

         var profiles = configuration.GetSection("profiles");
         var projectNameProfile = profiles.GetSection("https");
         var applicationUrl = projectNameProfile["applicationUrl"];
         if (applicationUrl != null)
         {
            var temp = applicationUrl.Split(new char[] { ';' }, StringSplitOptions.None);
            if (temp[0] != null)
            {
               applicationUrl = temp[0]; 
            }
         }

         Console.WriteLine("");
         Console.WriteLine("Open index.html file in Visual Studio and search for variable baseUrl");
         Console.WriteLine("Adjust it so that it matches URL below, i.e.");
         Console.WriteLine($"const baseUrl = '{applicationUrl}'");
         Console.WriteLine("");
         Console.WriteLine("Next, open index.html with web-browser and test it");
         Console.WriteLine("");

         app.Run();

      }
   }
}