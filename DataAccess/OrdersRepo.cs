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
    public class OrdersRepo
    {
        private string _connectionString;

        /// <summary>
        /// Null constructor that assigns connection string stored in App.config file to the field.
        /// </summary>
        public OrdersRepo()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["OrderManagementDb"].ConnectionString;
        }

        /// <summary>
        /// Method to insert a new record to OrderHeaders table.
        /// </summary>
        /// <returns>OrderHeader</returns>
        public OrderHeader InsertOrderHeader()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_InsertOrderHeader", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            //get the generated order header id
            //get the details of the new order header - search by id
            //return the order details
            int id = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            OrderHeader o = GetOrderHeader(id);
            return o;
        }

        /// <summary>
        /// Method to get an order by id from the OrderHeaders table.
        /// </summary>
        /// <param name="id">order header id</param>
        /// <returns>OrderHeader</returns>
        public OrderHeader GetOrderHeader(int id)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            //get the order details - search by id
            SqlCommand command = new SqlCommand("sp_SelectOrderHeaderById", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader reader = command.ExecuteReader();


            //initialise OrderHeader with null value
            OrderHeader o = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //if order header is null, instantiate the order header object
                    if (o == null)
                    {
                        //then read the order header details from sproc and store the information to the order header obj
                        id = reader.GetInt32(0);
                        int state = reader.GetInt32(1);
                        DateTime dateTime = reader.GetDateTime(2);
                        o = new OrderHeader(id, state, dateTime);
                    }
                    //if order header is not null, read the column 4-7 details and store them as order item obj
                    if (o != null && !reader.IsDBNull(3)) //if OrderHeader object is not null, and 4th column of the record is not null
                    {
                        //then add the order item obj to order header object
                        int stockItemId = reader.GetInt32(3);
                        string description = reader.GetString(4);
                        decimal price = reader.GetDecimal(5);
                        int quantity = reader.GetInt32(6);
                        o.AddOrderItem(stockItemId, price, description, quantity);
                    }
                }
            }
            connection.Close();
            //return the order header object
            return o;
        }

        /// <summary>
        /// Method to get orders which have order item(s).
        /// </summary>
        /// <returns>IEnumerable of OrderHeader</returns>
        public IEnumerable<OrderHeader> GetOrderHeaders()
        {
            List<OrderHeader> headers = new List<OrderHeader>();
            //read each order and the order item details related to the order
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_SelectOrderHeaders", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<int> stockIdList = new List<int>();
                List<string> descList = new List<string>();
                List<decimal> priceList = new List<decimal>();
                List<int> quantityList = new List<int>();
                int prevId = -1;
                int prevState = 0;
                DateTime prevDateTime = DateTime.Now;
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int state = reader.GetInt32(1);
                    DateTime dateTime = reader.GetDateTime(2);
                    int stockItemId = reader.GetInt32(3);
                    string description = reader.GetString(4);
                    decimal price = reader.GetDecimal(5);
                    int quantity = reader.GetInt32(6);

                    if (prevId != -1 && prevId != id)
                    {
                        OrderHeader currHeader = new OrderHeader(prevId, prevState, prevDateTime);
                        for (int i = 0; i < stockIdList.Count; i++)
                        {
                            currHeader.AddOrderItem(stockIdList[i], priceList[i], descList[i], quantityList[i]);
                        }
                        stockIdList.Clear();
                        descList.Clear();
                        priceList.Clear();
                        quantityList.Clear();
                        headers.Add(currHeader);
                    }
                    stockIdList.Add(stockItemId);
                    descList.Add(description);
                    priceList.Add(price);
                    quantityList.Add(quantity);

                    prevId = id;
                    prevState = state;
                    prevDateTime = dateTime;
                }

                OrderHeader lastHeader = new OrderHeader(prevId, prevState, prevDateTime);
                for (int i = 0; i < stockIdList.Count; i++)
                {
                    lastHeader.AddOrderItem(stockIdList[i], priceList[i], descList[i], quantityList[i]);
                }
                stockIdList.Clear();
                descList.Clear();
                priceList.Clear();
                quantityList.Clear();
                headers.Add(lastHeader);
            }
            connection.Close();
            return headers;
        }

        /// <summary>
        /// Method to update a quantity of OrderItems table, or to insert a new record in the table.
        /// </summary>
        /// <param name="orderItem">OrderItem object</param>
        public void UpsertOrderItem(OrderItem orderItem)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_UpsertOrderItem", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderHeaderId", orderItem.OrderHeaderId));
            command.Parameters.Add(new SqlParameter("@stockItemId", orderItem.StockItemId));
            command.Parameters.Add(new SqlParameter("@description", orderItem.Description));
            command.Parameters.Add(new SqlParameter("@price", orderItem.Price));
            command.Parameters.Add(new SqlParameter("@quantity", orderItem.Quantity));

            int rtn = Convert.ToInt32(command.ExecuteScalar());

            connection.Close();
        }

        /// <summary>
        /// Method to update a state of a record in OrderHeaders table.
        /// </summary>
        /// <param name="order">OrderHeader object</param>
        public void UpdateOrderState(OrderHeader order)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_UpdateOrderState", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderHeaderId", order.Id));

            if (order.State is OrderNew)
            {
                command.Parameters.Add(new SqlParameter("@stateId", 1));
            }
            else if (order.State is OrderPending)
            {
                command.Parameters.Add(new SqlParameter("@stateId", 2));
            }
            else if (order.State is OrderRejected)
            {
                command.Parameters.Add(new SqlParameter("@stateId", 3));
            }
            else if (order.State is OrderComplete)
            {
                command.Parameters.Add(new SqlParameter("@stateId", 4));
            }

            int rtn = Convert.ToInt32(command.ExecuteScalar());

            connection.Close();
        }

        /// <summary>
        /// Method to delete a record from OrderHeaders table, and record(s) from OrderItems table.
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_DeleteOrderHeaderAndOrderItems", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderHeaderId", orderHeaderId));

            int rtn = Convert.ToInt32(command.ExecuteScalar());

            connection.Close();
        }

        /// <summary>
        /// Method to delete a record from OrderItems table.
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        /// <param name="stockItemId">StockItem id</param>
        public void DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            SqlCommand command = new SqlCommand("sp_DeleteOrderItem", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderHeaderId", orderHeaderId));
            command.Parameters.Add(new SqlParameter("@stockItemId", stockItemId));

            int rtn = Convert.ToInt32(command.ExecuteScalar());

            connection.Close();
        }
    }
}
