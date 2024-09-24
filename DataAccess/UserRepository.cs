using System.Data.SqlClient;
using System.Data;
using FootwearPointWebApi.Models;
using FootwearPointWebApi.Helpers;
using NuGet.Protocol.Plugins;
using System.Reflection;
namespace FootwearPointWebApi.DataAccess
{
    public class UserRepository:Repository
    {
        public UserRepository()
        {
            ConnectionString = "Server=G1-5CD40140QW-L\\SQLEXPRESS;Database=ShoesManagementDB;Integrated Security= true;";
            Connect(ConnectionString);
        }
        public UserViewModel GetUser(UserLoginViewModel model)
        {
            UserViewModel user = new UserViewModel();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;

            command.CommandText = model.Password!="" ? "SELECT * FROM USERS WHERE Email = @Email AND Password=@Password": "SELECT * FROM USERS WHERE Email = @Email";

            command.Parameters.AddWithValue("@Email", model.Email);
            if (model.Password != "")
            {
            command.Parameters.AddWithValue("@Password", model.Password);
            }
            SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Email = Convert.ToString(reader["Email"]);
                    user.RoleID = Convert.ToInt32(reader["RoleID"]);
                    user.FirstName = Convert.ToString(reader["FirstName"]);
                    user.LastName = Convert.ToString(reader["LastName"]);
                    user.Phone = Convert.ToString(reader["Phone"]);
                    user.Address = Convert.ToString(reader["Address"]);
                    user.Password = "";
                    user.HashedPassword = "";
                    user.CreatedAt = Convert.ToString(reader["CreatedAt"]);
                    user.ModifiedAt = Convert.ToString(reader["ModifiedAt"]);

                    return user;
                }

            }
            return null;
        }

        public int Insert(UserRegistrationViewModel model)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            string HashedPassword = PasswordHasher.HashPassword(model.Password);
            command.CommandText = "INSERT INTO Users (FirstName, LastName, RoleID, Phone, Address, Password, HashedPassword, Email) VALUES" +
                "(@FirstName, @LastName, @RoleID, @Phone, @Address, @Password, @HashedPassword, @Email);";

            // Add parameters
                command.Parameters.AddWithValue("@FirstName", model.FirstName);
                command.Parameters.AddWithValue("@LastName", model.LastName);
                command.Parameters.AddWithValue("@RoleID", model.RoleID);
                command.Parameters.AddWithValue("@Phone", model.Phone);
                command.Parameters.AddWithValue("@Address", model.Address);
                command.Parameters.AddWithValue("@Password", model.Password);
                command.Parameters.AddWithValue("@HashedPassword", HashedPassword);
                command.Parameters.AddWithValue("@Email", model.Email);

                // Execute the command
                int effRow = command.ExecuteNonQuery();
            int UserID = 0;
            command.CommandText = "SELECT UserID FROM USERS WHERE Email = @UserEmail AND Phone = @UserPhone";

            command.Parameters.AddWithValue("@UserEmail", model.Email);
            command.Parameters.AddWithValue("@UserPhone", model.Phone);

           SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    UserID = Convert.ToInt32(reader["UserID"]);
                    
                }

            }
            command.CommandText = "INSERT INTO CART (UserID,Email) VALUES(@CartUserID,@CartEmail);";
            command.Parameters.AddWithValue("@CartUserID", UserID);
            command.Parameters.AddWithValue("@CartEmail", model.Email);
            reader.Close();
            if (UserID != 0)
            {
            command.ExecuteNonQuery();
            }

            return UserID;
            
        }

        public int Update(UserViewModel model)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE USERS SET FirstName = @FirstName , LastName = @LastName, Address = @Address , Phone = @Phone where UserId = @UserID";
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@LastName", model.LastName);
            command.Parameters.AddWithValue("@Address", model.Address);
            command.Parameters.AddWithValue("@Phone", model.Phone);
            command.Parameters.AddWithValue("@UserID", model.UserID);

            int efRows = command.ExecuteNonQuery();
            return efRows;
        }
    }
}






