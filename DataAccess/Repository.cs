using FootwearPointWebApi.Models;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
namespace FootwearPointWebApi.DataAccess
{
    public  class Repository
    {
        bool returnValue = false;
        private SqlConnection _connection;
        protected SqlConnection Connection { get { return _connection; } }
        public string ConnectionString {  
            get { return connectionstring; } 
            set { connectionstring = value; }
        }
        private string connectionstring= String.Empty;
        public virtual bool Connect(string connectionstring)
        {
            if (_connection == null) {
                _connection = new SqlConnection();
            }
            if (connectionstring != String.Empty) { 
                _connection.ConnectionString = connectionstring;
            }
            try
            {
            _connection.Open();
                returnValue = true;
            }
            catch (Exception ex) {
                returnValue = false;
            }
            return returnValue;
        }

        //public IList getAll() { return null; }
        //public T getById<T>(int ID) { return default(T); }

        //public int? Insert<T>(T data) { return null; }
        //public int Update<T>(T data, int id) { return 0; }
        //public int Delete(int ID) { return 0; }

    }
}
