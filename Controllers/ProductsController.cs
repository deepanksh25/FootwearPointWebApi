using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FootwearPointWebApi.DataAccess;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data.SqlClient;
using System.Data;
namespace FootwearPointWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }
   

        [HttpGet("GetProducts")]
        [Authorize(Roles = "Admin,Customer")]
        public IResult GetProducts()
        {
                ProductRepository repo = new ProductRepository();
                var products = repo.getAll();
            return Results.Ok(products);
        }



        [HttpGet("ViewProduct")]
        public IResult ViewProduct([FromBody]int id)
        {
            ProductRepository repo = new ProductRepository();
            ProductViewModel product = repo.getById(id);
            if (product == null)
            {
                return Results.Problem("No Product was found    ");
            }
            
            return Results.Ok(product);

        }
        //[ActionName("AddProduct")]
        //public IActionResult AddProductPageFeeder()
        //{
        //    return View("AddProductForm");
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateProductEntry(ProductViewModel model)
        //{

        //    if (model.ProductImage != null && model.ProductImage.Length > 0)
        //    {

        //        var uniqueFileName = $"{model.ProductImage.FileName.Split('.')[0]}.jpg";

        //        // Path to save the file
        //        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        // Ensure the uploads folder exists
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        // Save the file
        //        if (!Directory.Exists(filePath))
        //        {
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await model.ProductImage.CopyToAsync(fileStream);
        //            }
        //        }

        //        // Set the image URL
        //        model.ProductImageUrl = $"/images/{uniqueFileName}";
        //        int quantity = model.ProductQuantityAdded;
        //        model.ProductQuantityLeft = quantity;

        //        ProductRepository repo = new ProductRepository();
        //        int numRows = repo.Insert(model);


        //        // Redirect or return a view
        //        return View("ViewProduct", model); // Example action
        //    }

        //    ModelState.AddModelError("", "No file uploaded.");


        //    return View("AddProductForm", model);



        //}
        //public IActionResult UpdateProduct(int id)
        //{
        //    ProductRepository repo = new ProductRepository();
        //    ProductViewModel product = repo.getById(id);
        //    return View(product);
        //}
        //public IActionResult DeleteProduct(int productId)
        //{
        //    ProductRepository repo = new ProductRepository();
        //    repo.Delete(productId);
        //    return RedirectToAction("AllProducts");
        //}

        //public async Task<IActionResult> UpdateProductEntry([FromForm] ProductViewModel model, [FromRoute] int id)
        //{
        //    if (model.ProductImage != null && model.ProductImage.Length > 0)
        //    {

        //        var uniqueFileName = $"{model.ProductImage.FileName.Split('.')[0]}.jpg";

        //        // Path to save the file
        //        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        // Ensure the uploads folder exists
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        // Save the file
        //        if (!Directory.Exists(filePath))
        //        {
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await model.ProductImage.CopyToAsync(fileStream);
        //            }
        //        }

        //        // Set the image URL
        //        model.ProductImageUrl = $"/images/{uniqueFileName}";
        //    }
        //    int quantity = model.ProductQuantityAdded;
        //    model.ProductQuantityLeft = quantity;

        //    ProductRepository repo = new ProductRepository();
        //    int numRows = repo.Update(model, id);


        //    // Redirect or return a view
        //    return View("ViewProduct", model); // Example action



        //}
        [HttpGet("Get")]
        public IResult Get()
        {
            ProductRepository repo = new ProductRepository();

            IList<ProductCategory> listCategory = repo.GetCategory();
            IList<ProductSubCategory> listSubCategory = repo.GetSubCategory();
            IList<ProductViewModel> listAll = repo.getAll();

            var response = new
            {
                Products = listAll,
                Categories = listCategory,
                SubCategories = listSubCategory
            };

            return Results.Ok(response);
        }

        [HttpPost("post")]
        public IActionResult Post([FromBody] ShoeViewModel model)
        {
            ProductRepository repo = new ProductRepository();

            if (model == null)
            {
                return BadRequest("Product data is required.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            int affectedRows = repo.Insert(model);

            if (affectedRows > 0)
            {
                return CreatedAtAction(nameof(Get), new { id = model.ProductID }, model); // Return created response
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the product.");
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            ProductRepository repo = new ProductRepository();
            var product = repo.getById(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductViewModel model)
        {
            if (model == null || id != model.ProductID)
            {
                return BadRequest("Invalid product data.");
            }

            ProductRepository repo = new ProductRepository();

            var existingProduct = repo.getById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            int affectedRows = repo.Update(id, model);

            if (affectedRows > 0)
            {
                return Ok(affectedRows);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ProductRepository repo = new ProductRepository();

            var product = repo.getById(id);
            if (product == null)
            {
                return NotFound($"Product with ID = {id} not found.");
            }


            int affectedRows = repo.Delete(id);
            if (affectedRows > 0)
            {
                return Ok("Product deleted successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }

        [HttpGet("filterproducts")]
        public IActionResult FilterProducts([FromQuery] string[] categories = null, [FromQuery] int maxPrice = 0, [FromQuery] int minPrice = 0)
        {
            ProductRepository repo = new ProductRepository();
            List<ProductViewModel> filteredProducts = new List<ProductViewModel>();

            using (SqlConnection connection = new SqlConnection(repo.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand("sp_FilterProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    var categoriesString = categories != null ? string.Join(",", categories) : null;


                    command.Parameters.AddWithValue("@Categories", (object)categoriesString ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    command.Parameters.AddWithValue("@MinPrice", minPrice);

                    connection.Open();


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filteredProducts.Add(new ProductViewModel
                            {
                                ProductID = reader["ProductID"] != DBNull.Value ? Convert.ToInt32(reader["ProductID"]) : 0,
                                ProductName = reader["ProductName"] != DBNull.Value ? Convert.ToString(reader["ProductName"]) : string.Empty,
                                ProductPrice = reader["ProductPrice"] != DBNull.Value ? Convert.ToInt32(reader["ProductPrice"]) : 0,
                                GenderCategory = reader["GenderCategory"] != DBNull.Value ? Convert.ToString(reader["GenderCategory"]) : string.Empty,
                                ProductImageUrl = reader["ProductImageUrl"] != DBNull.Value ? Convert.ToString(reader["ProductImageUrl"]) : string.Empty,
                                ProductSize = reader["ProductSize"] != DBNull.Value ? Convert.ToInt32(reader["ProductSize"]) : 0,
                                ProductColor = reader["ProductColor"] != DBNull.Value ? Convert.ToString(reader["ProductColor"]) : string.Empty,
                                ProductCategoryID = reader["ProductCategoryID"] != DBNull.Value ? Convert.ToInt32(reader["ProductCategoryID"]) : 0,
                                ProductSubCategoryID = reader["ProductSubCategoryID"] != DBNull.Value ? Convert.ToInt32(reader["ProductSubCategoryID"]) : 0,
                            });
                        }
                    }
                }
            }


            return Ok(filteredProducts);
        }


    }
}
