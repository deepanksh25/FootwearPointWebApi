using FootwearPointWebApi.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FootwearPointWebApi.DataAccess
{
    public class CartRepository:Repository
    {
        public CartRepository() {
            ConnectionString = "Server=G1-5CD40140QW-L\\SQLEXPRESS;Database=ShoesManagementDB;Integrated Security= true;";
            Connect(ConnectionString);
        }
        public string GetCartId(string Email)
        {

            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT CartID,UserID FROM Cart Where Email = @Email";
            command.Parameters.AddWithValue("@Email", Email);
            int CartID = 0;
            int UserID = 0;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                CartID = Convert.ToInt32(reader["CartID"]);
                UserID = Convert.ToInt32(reader["UserID"]);
            }
            reader.Close();
            return CartID.ToString()+"/"+UserID.ToString();
        }
        public IList<CartListViewModel> GetCartDetails(string Email)
        {
            IList<CartListViewModel> cartList = new List<CartListViewModel>();
            SqlCommand command = new SqlCommand();
            ProductRepository repo = new ProductRepository();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            
           int CartID = Convert.ToInt32(GetCartId(Email).Split("/")[0]);
            int UserId = Convert.ToInt32(GetCartId(Email).Split("/")[1]);

            command.CommandText = "SELECT * FROM CartDetails WHERE CartID = @CartID";
            command.Parameters.AddWithValue("@CartID", CartID);
            SqlDataReader reader = command.ExecuteReader();

            if (reader != null)
            {
                while (reader.Read())
                {
                    ProductViewModel model = repo.getById(Convert.ToInt32(reader["ProductID"]));
                    cartList.Add(new CartListViewModel
                    {
                        CartID = Convert.ToInt32(reader["CartID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductQuantity = Convert.ToInt32(reader["ProductQuantity"]),
                        ProductName = model.ProductName,
                        ProductCategoryID = model.ProductCategoryID,
                        ProductSubCategoryID = model.ProductSubCategoryID,
                        ProductColor = model.ProductColor,
                        ProductSize = model.ProductSize,
                        ProductImageUrl = model.ProductImageUrl,
                        ProductPrice = model.ProductPrice,
                        ProductImage = model.ProductImage,
                        ProductQuantityLeft = model.ProductQuantityLeft,

                    });
                }
                return cartList.ToList();

            }
            return cartList.ToList();
        }
        public int AddToCart(AddToCartViewModel model) {
            int CartID = Convert.ToInt32(GetCartId(model.Email).Split("/")[0]);
            int UserId = Convert.ToInt32(GetCartId(model.Email).Split("/")[1]);
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            
            command.CommandText = "SELECT * FROM CartDetails WHERE ProductID = @ProductID AND CartID = @CartID AND UserID = @UserID";
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@UserID", UserId);
            command.Parameters.AddWithValue("@ProductID", model.ProductID);
            SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                int previousQuantity = 0;
                while (reader.Read())
                {
                    previousQuantity = Convert.ToInt32(reader["ProductQuantity"]);
                }
                    reader.Close();
                if (previousQuantity > 0)
                { 
                command.CommandText = "UPDATE CartDetails Set ProductQuantity = @newQuantity Where ProductID = @iProductID AND UserID = @iUserID AND CartID = @iCartID";
                int newQuantity = previousQuantity + model.quantity;
                command.Parameters.AddWithValue("@iCartID", CartID);
                command.Parameters.AddWithValue("@iUserID", UserId);
                command.Parameters.AddWithValue("@iProductID", model.ProductID);
                command.Parameters.AddWithValue("@newQuantity", model.quantity);
                return command.ExecuteNonQuery();
                }

            }
            command.CommandText = "INSERT INTO CartDetails VALUES(@nCartID,@nUserID,@nProductID,@nProductQuantity);";
            command.Parameters.AddWithValue("@nCartID", CartID);
            command.Parameters.AddWithValue("@nUserID", UserId);
            command.Parameters.AddWithValue("@nProductID", model.ProductID);
            command.Parameters.AddWithValue("@nProductQuantity", model.quantity);
            return command.ExecuteNonQuery();

        }

        public int Delete(AddToCartViewModel model)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            int CartID = Convert.ToInt32(GetCartId(model.Email).Split("/")[0]);
            int UserId = Convert.ToInt32(GetCartId(model.Email).Split("/")[1]);
            command.CommandText = "Delete from CartDetails where ProductID = @ProductID AND UserID = @UserID AND CartID = @CartID" ;
            command.Parameters.AddWithValue("@ProductID", model.ProductID);
            command.Parameters.AddWithValue("@UserID", UserId);
            command.Parameters.AddWithValue("@CartID", CartID);
            return command.ExecuteNonQuery();
        }

        public int Delete(int id)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "Delete from CartDetails where ProductID = @ProductID ";
            command.Parameters.AddWithValue("@ProductID", id);
            return command.ExecuteNonQuery();
        }
    }
}
