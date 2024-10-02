using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FootwearPointWebApi.DataAccess

{
    public class ProductRepository : Repository
    {
        public ProductRepository() {
            ConnectionString = "Server=G1-5CD40140QW-L\\SQLEXPRESS;Database=ShoesManagementDB;Integrated Security= true;";
            Connect(ConnectionString);
        }  
        public virtual int Delete(int ID)
        {
            ProductViewModel product = new ProductViewModel();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "Delete From PRODUCTS WHERE ProductID = @ProductID";

            command.Parameters.AddWithValue("@ProductID", ID);
            int reader = command.ExecuteNonQuery();
            return reader;
        }

        public virtual IList<ShoeViewModel> getAll()
        {
            IList<ShoeViewModel> products = new List<ShoeViewModel>();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM PRODUCTS";

            //if(Connection.State != System.Data.ConnectionState.Open)
            //{
            //    Connection.Open();
            //}
            
            SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    products.Add(new ShoeViewModel
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = Convert.ToString(reader["ProductName"]),
                        ProductCategoryID = Convert.ToInt32(reader["ProductCategoryID"]),
                        ProductColor = Convert.ToString(reader["ProductColor"]),
                        ProductPrice = Convert.ToInt32(reader["ProductPrice"]),
                        ProductQuantityInStock = Convert.ToInt32(reader["ProductQuantityInStock"]),
                        ProductSize = Convert.ToInt32(reader["ProductSize"]),
                        GenderCategory = Convert.ToString(reader["GenderCategory"]),
                        ProductImageUrl = Convert.ToString(reader["ProductImageUrl"]),
                        ProductImage = null,
                        ProductSubCategoryID = Convert.ToInt32(reader["ProductSubCategoryID"]),
                        ProductQuantityAdded = Convert.ToInt32(reader["ProductQuantityAdded"]),
                    });
                }
                return products.ToList();

            }
            return null;
        }

        //public virtual ProductViewModel getById(int ID)
        //{
        //    ProductViewModel product = new ProductViewModel() ;
        //    SqlCommand command = new SqlCommand();
        //    command.Connection = Connection;
        //    command.CommandType = CommandType.Text;
        //    command.CommandText = "SELECT * FROM PRODUCTS WHERE ProductID = @ProductID";

        //    command.Parameters.AddWithValue("@ProductID", ID);
        //    SqlDataReader reader = command.ExecuteReader();
        //    if (reader != null)
        //    {
        //        while (reader.Read())
        //        {
        //            product.ProductID = Convert.ToInt32(reader["ProductID"]);
        //            product.ProductName = Convert.ToString(reader["ProductName"]);
        //            product.ProductCategoryID = Convert.ToInt32(reader["ProductCategoryID"]);
        //            product.ProductColor = Convert.ToString(reader["ProductColor"]);
        //            product.ProductPrice = Convert.ToInt32(reader["ProductPrice"]);
        //            product.ProductQuantityLeft = Convert.ToInt32(reader["ProductQuantityInStock"]);
        //            product.ProductSize = Convert.ToInt32(reader["ProductSize"]);
        //            product.GenderCategory = Convert.ToString(reader["GenderCategory"]);
        //            product.ProductImageUrl = Convert.ToString(reader["ProductImageUrl"]);
        //            product.ProductImage = null;
        //            product.ProductSubCategoryID = Convert.ToInt32(reader["ProductSubCategoryID"]);
        //            product.ProductQuantityAdded = 0;

        //        return product;
        //        }

        //    }
        //    return null;
        //}


        public  ProductViewModel getById(int Id)
        {
            ProductViewModel product = null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", Id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // Check if there are any rows
                        {
                            while (reader.Read())
                            {
                                product = new ProductViewModel
                                {
                                    ProductID = Convert.ToInt32(reader["ProductID"]),
                                    ProductName = Convert.ToString(reader["ProductName"]),
                                    ProductCategoryID = Convert.ToInt32(reader["ProductCategoryID"]),
                                    ProductColor = Convert.ToString(reader["ProductColor"]),
                                    ProductPrice = Convert.ToInt32(reader["ProductPrice"]),
                                    ProductQuantityLeft = Convert.ToInt32(reader["ProductQuantityInStock"]),
                                    ProductSize = Convert.ToInt32(reader["ProductSize"]),
                                    GenderCategory = Convert.ToString(reader["GenderCategory"]),
                                    ProductImageUrl = Convert.ToString(reader["ProductImageUrl"]),
                                    ProductImage = null,
                                    ProductSubCategoryID = Convert.ToInt32(reader["ProductSubCategoryID"]),
                                    ProductQuantityAdded = 0,
                                };
                            }
                        }
                    }
                }
            }

            return product;
        }
        public int Insert(AddShoeModel data)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    // Updated SQL query to insert all the new columns
                    command.CommandText = @"
            INSERT INTO Products 
            (ProductName, ProductPrice, ProductQuantityInStock, ProductImageUrl, ProductSize, GenderCategory, ProductSubCategoryID, ProductCategoryID, ProductColor,ProductQuantityAdded)
            VALUES 
            (@ProductName, @ProductPrice, @ProductQuantityInStock, @ProductImageUrl, @ProductSize, @GenderCategory, @ProductSubCategoryID, @ProductCategoryID, @ProductColor, @ProductQuantityAdded)";

                    command.Parameters.AddWithValue("@ProductName", data.ProductName);
                    command.Parameters.AddWithValue("@ProductPrice", data.ProductPrice);
                    command.Parameters.AddWithValue("@ProductQuantityInStock", data.ProductQuantityInStock);
                    command.Parameters.AddWithValue("@ProductImageUrl", data.ProductImageUrl);
                    command.Parameters.AddWithValue("@ProductSize", data.ProductSize);
                    command.Parameters.AddWithValue("@GenderCategory", data.GenderCategory);
                    command.Parameters.AddWithValue("@ProductSubCategoryID", data.ProductSubCategoryID);
                    command.Parameters.AddWithValue("@ProductCategoryID", data.ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductColor", data.ProductColor);
                    command.Parameters.AddWithValue("@ProductQuantityAdded", data.ProductQuantityAdded);
                    connection.Open();
                    int affectedRows = command.ExecuteNonQuery();

                    return affectedRows;
                }
            }
        }

        //public int Insert(ProductViewModel model)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.Connection = Connection;
        //    command.CommandType = CommandType.Text;
        //    command.CommandText = "INSERT INTO PRODUCTS (ProductName,ProductCategoryID,ProductColor," +
        //        "ProductPrice,ProductQuantityInStock,ProductSize,GenderCategory,ProductImageUrl,ProductQuantityAdded,ProductSubCategoryID) VALUES" +
        //        "(@ProductName,@ProductCategoryID,@ProductColor,@ProductPrice,@ProductQuantityInStock," +
        //        "@ProductSize,@GenderCategory,@ProductImageUrl,@ProductQuantityAdded,@ProductSubCategoryID)";

        //    command.Parameters.AddWithValue("@ProductName", model.ProductName);
        //    command.Parameters.AddWithValue("@ProductCategoryID", model.ProductCategoryID);
        //    command.Parameters.AddWithValue("@ProductColor", model.ProductColor);
        //    command.Parameters.AddWithValue("@ProductPrice", model.ProductPrice);
        //    command.Parameters.AddWithValue("@ProductQuantityInStock", model.ProductQuantityLeft);
        //    command.Parameters.AddWithValue("@ProductSize", model.ProductSize);
        //    command.Parameters.AddWithValue("@GenderCategory", model.GenderCategory);
        //    command.Parameters.AddWithValue("@ProductImageUrl", model.ProductImageUrl);
        //    command.Parameters.AddWithValue("@ProductQuantityAdded", model.ProductQuantityAdded);
        //    command.Parameters.AddWithValue("@ProductSubCategoryID", model.ProductSubCategoryID);

        //    int effRos = command.ExecuteNonQuery();
        //    return effRos;
        //}

        //public virtual int Update(ProductViewModel model,int id)
        //{
        //    SqlCommand command = new SqlCommand();
        //    command.Connection = Connection;
        //    command.CommandType = CommandType.Text;
        //    command.CommandText = "UPDATE PRODUCTS SET ProductName = @ProductName,ProductCategoryID=@ProductCategoryID ,ProductColor = @ProductColor," +
        //        "ProductPrice=@ProductPrice,ProductQuantityInStock=@ProductQuantityInStock,ProductSize=@ProductSize,GenderCategory=@GenderCategory,ProductImageUrl=@ProductImageUrl,ProductQuantityAdded=@ProductQuantityAdded,ProductSubCategoryID=@ProductSubCategoryID where ProductID=@ProductID" ;

        //    command.Parameters.AddWithValue("@ProductName", model.ProductName);
        //    command.Parameters.AddWithValue("@ProductCategoryID", model.ProductCategoryID);
        //    command.Parameters.AddWithValue("@ProductColor", model.ProductColor);
        //    command.Parameters.AddWithValue("@ProductPrice", model.ProductPrice);
        //    command.Parameters.AddWithValue("@ProductQuantityInStock", model.ProductQuantityLeft);
        //    command.Parameters.AddWithValue("@ProductSize", model.ProductSize);
        //    command.Parameters.AddWithValue("@GenderCategory", model.GenderCategory);
        //    command.Parameters.AddWithValue("@ProductImageUrl", model.ProductImageUrl);
        //    command.Parameters.AddWithValue("@ProductQuantityAdded", model.ProductQuantityAdded);
        //    command.Parameters.AddWithValue("@ProductSubCategoryID", model.ProductSubCategoryID);
        //    command.Parameters.AddWithValue("@ProductID", id);

        //    int effRos = command.ExecuteNonQuery();
        //    return effRos;
        //}

        public  int Update(int id, ProductViewModel model)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var query = @"
                    UPDATE Products 
                    SET 
                        ProductName = @ProductName, 
                        ProductPrice = @ProductPrice, 
                        ProductQuantityInStock = @ProductQuantityInStock, 
                        ProductImageUrl = @ProductImageUrl, 
                        ProductSize = @ProductSize, 
                        GenderCategory = @GenderCategory, 
                        ProductSubCategoryID = @ProductSubCategoryID, 
                        ProductCategoryID = @ProductCategoryID, 
                        ProductColor = @ProductColor 
                    WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters for the updated fields
                    command.Parameters.AddWithValue("@ProductName", model.ProductName);
                    command.Parameters.AddWithValue("@ProductPrice", model.ProductPrice);
                    command.Parameters.AddWithValue("@ProductQuantityInStock", model.ProductQuantityLeft);
                    command.Parameters.AddWithValue("@ProductImageUrl", model.ProductImageUrl);
                    command.Parameters.AddWithValue("@ProductSize", model.ProductSize);
                    command.Parameters.AddWithValue("@GenderCategory", model.GenderCategory);
                    command.Parameters.AddWithValue("@ProductSubCategoryID", model.ProductSubCategoryID);
                    command.Parameters.AddWithValue("@ProductCategoryID", model.ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductColor", model.ProductColor);
                    command.Parameters.AddWithValue("@ProductID", id);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        
        public IList<CategoryCount> getbyCategory()
        {
            IList<CategoryCount> categoryCount = new List<CategoryCount>();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = @"
        SELECT c.ProductCategoryName, COUNT(p.ProductCategoryID) AS ProductCount
        FROM Category c
        LEFT JOIN Products p ON c.ProductCategoryID = p.ProductCategoryID
        GROUP BY c.ProductCategoryName;
    ";

            //if (Connection.State != System.Data.ConnectionState.Open)
            //{
            //    Connection.Open();
            //}

            SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    categoryCount.Add(new CategoryCount
                    {
                        ProductCategoryName = reader["ProductCategoryName"] != DBNull.Value ? Convert.ToString(reader["ProductCategoryName"]) : string.Empty,
                        ProductCategoryCount = reader["ProductCount"] != DBNull.Value ? Convert.ToInt32(reader["ProductCount"]) : 0 // Changed to match the SQL query alias
                    });
                }
                return categoryCount.ToList();
            }
            return null;
        }

        public IList<ProductCategory> GetCategory()
        {
            IList<ProductCategory> categories = new List<ProductCategory>();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Category";

            //try
            //{
                //if (Connection.State != ConnectionState.Open)
                //{
                //    Connection.Open();
                //}

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            categories.Add(new ProductCategory
                            {
                                ProductCategoryID = reader["ProductCategoryID"] != DBNull.Value ? Convert.ToInt32(reader["ProductCategoryID"]) : 0,
                                ProductCategoryName = reader["ProductCategoryName"] != DBNull.Value ? Convert.ToString(reader["ProductCategoryName"]) : string.Empty
                            });
                        }
                    }
                }
            //}
            //finally
            //{
            //    if (Connection.State == ConnectionState.Open)
            //    {
            //        Connection.Close();
            //    }
            //}

            return categories;
        }

        public IList<ProductSubCategory> GetSubCategory()
        {
            IList<ProductSubCategory> subcategories = new List<ProductSubCategory>();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM SubCategory";

            //try
            //{
                //if (Connection.State != ConnectionState.Open)
                //{
                //    Connection.Open();
                //}

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            subcategories.Add(new ProductSubCategory
                            {
                                ProductSubCategoryID = reader["ProductSubCategoryID"] != DBNull.Value ? Convert.ToInt32(reader["ProductSubCategoryID"]) : 0,
                                ProductSubCategoryName = reader["ProductSubCategoryName"] != DBNull.Value ? Convert.ToString(reader["ProductSubCategoryName"]) : string.Empty
                            });
                        }
                    }
                }
            //}
            //finally
            //{
            //    if (Connection.State == ConnectionState.Open)
            //    {
            //        Connection.Close();
            //    }
            //}

            return subcategories;
        }

        public async Task<IList<PastOrderViewModel>> PastOrders()
        {
            var orders = new List<PastOrderViewModel>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT o.OrderID, o.UserID, o.OrderDate, o.OrderAmount, o.OrderAddress, o.PaymentStatus, o.OrderStatus, 
                   od.ProductID, od.ProductCount, od.ProductAmount 
            FROM Orders o 
            JOIN OrderDetails od ON o.OrderID = od.OrderID 
            ORDER BY o.OrderDate DESC";

                //if (Connection.State != ConnectionState.Open)
                //{
                //    await Connection.OpenAsync();
                //}

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var orderID = reader.GetInt32(0);
                        var existingOrder = orders.FirstOrDefault(o => o.OrderID == orderID);

                        if (existingOrder == null)
                        {
                            existingOrder = new PastOrderViewModel
                            {
                                //Change to values["OrderAddress"]
                                OrderID = orderID,
                                CustomerID = reader.GetInt32(1),
                                OrderDate = reader.GetDateTime(2),
                                TotalAmount = reader.GetInt32(3),
                                ShippingAddress = reader.GetString(4),
                                PaymentStatus = reader.GetString(5),
                                OrderStatus = reader.GetString(6),
                                OrderDetails = new List<OrderDetailViewModel>()
                            };
                            orders.Add(existingOrder);
                        }

                        existingOrder.OrderDetails.Add(new OrderDetailViewModel
                        {
                            ProductID = reader.GetInt32(7),
                            ProductCount = reader.GetInt32(8),
                            ProductAmount = reader.GetInt32(9)
                        });
                    }
                }

                //if (Connection.State == ConnectionState.Open)
                //{
                //    await Connection.CloseAsync();
                //}
            }

            return orders;
        }
    }
}
