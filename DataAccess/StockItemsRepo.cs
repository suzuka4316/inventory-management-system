using Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// A class that is responsible for inserting, updating, deleting and retrieving data from the database.
    /// </summary>
    public class StockItemsRepo
    {

        private string _connectionString;

        /// <summary>
        /// Null constructor that assigns connection string stored in App.config file to the field.
        /// </summary>
        public StockItemsRepo()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["OrderManagementDb"].ConnectionString;
        }

        /// <summary>
        /// Method to get all the records from StockItems table.
        /// </summary>
        /// <returns>IEnumerable of StockItem objects</returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            List<StockItem> itemsList = new List<StockItem>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_SelectStockItems", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            StockItem s = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    int inStock = reader.GetInt32(3);
                    s = new StockItem(id, name, price, inStock);
                    itemsList.Add(s);
                }
            }
            connection.Close();

            return itemsList;
        }

        /// <summary>
        /// Method to get a record from StockItems table.
        /// </summary>
        /// <param name="id">StockItem id</param>
        /// <returns>StockItem</returns>
        public StockItem GetStockItem(int id)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_SelectStockItemById", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader reader = command.ExecuteReader();

            StockItem s = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    int inStock = reader.GetInt32(3);
                    s = new StockItem(id, name, price, inStock);
                }
            }
            connection.Close();

            return s;
        }

        /// <summary>
        /// Method to update the stock level of all order items or none of them.
        /// </summary>
        /// <param name="order">OrderHeader object</param>
        public void UpdateStockItemAmount(OrderHeader order)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("sp_UpdateStockItemAmount", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var transaction = connection.BeginTransaction("UpdateStockAmountTransaction");
            command.Transaction = transaction;
            try
            {
                foreach (var oi in order.OrderItems)
                {
                    command.Parameters.Add(new SqlParameter("@id", oi.StockItemId));
                    command.Parameters.Add(new SqlParameter("@amount", -oi.Quantity));
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw ex;
            }
            connection.Close();
        }
    }
}
