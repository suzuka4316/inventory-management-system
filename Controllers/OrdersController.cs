using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using DataAccess;
using System.Configuration;
using System.Data.SqlClient;

namespace Controllers
{
    /// <summary>
    /// A class for controlling the application flow and passing messages (commands, queries, domain objects) between the data access and User Interface layers. 
    /// </summary>
    public class OrdersController
    {
        private OrdersRepo ordersRepo;
        private StockItemsRepo stockItemsRepo;

        //singleton pattern
        private static OrdersController _instance;

        /// <summary>
        /// Null constructor.
        /// </summary>
        public OrdersController(){
            ordersRepo = new OrdersRepo();
            stockItemsRepo = new StockItemsRepo();
        }

        /// <summary>
        /// Static method to instantiate an OrdersController object.
        /// </summary>
        public static OrdersController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrdersController();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Method to get all the order header objects.
        /// </summary>
        /// <returns>IEnumerable of OrderHeader objects</returns>
        public IEnumerable<OrderHeader> GetOrderHeaders()
        {
            try
            {
                //call OrderRepo method - GetOrderHeaders() to get OrderHeader objects
                IEnumerable<OrderHeader> oh = ordersRepo.GetOrderHeaders();
                //return the Order Headers
                return oh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to create a new OrderHeader object.
        /// </summary>
        /// <returns>OrderHeader</returns>
        public OrderHeader CreateNewOrderHeader()
        {
            try
            {
                //call OrderRepo method - InsertOrderHeader() to get OrderHeader object
                OrderHeader oh = ordersRepo.InsertOrderHeader();
                //return OrderHeader
                return oh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to update or insert an quantity for an OrderItem object.
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        /// <param name="stockItemId">StockItem id</param>
        /// <param name="quantity">quantity that will be added for the order item</param>
        /// <returns></returns>
        public OrderHeader UpsertOrderItem(int orderHeaderId, int stockItemId, int quantity)
        {
            try
            {
                //call StockItemRepo method - GetStockItem to get StockItem object
                StockItem si = stockItemsRepo.GetStockItem(stockItemId);
                //call OrderRepo method - GetOrderHeader to get OrderHeader object
                OrderHeader oh = ordersRepo.GetOrderHeader(orderHeaderId);
                OrderItem oi = oh.AddOrderItem(si.Id, si.Price, si.Name, quantity);
                ordersRepo.UpsertOrderItem(oi);
                return oh;

                //StockItem si = stockItemsRepo.GetStockItem(stockItemId);
                //OrderHeader oh = ordersRepo.GetOrderHeader(orderHeaderId);
                //List<OrderItem> oiList = oh.OrderItems;
                ////Find the index from oiList where the StockItemId is equal to stockItemId that's passed as argument
                //int index = oiList.FindIndex(oi => oi.StockItemId == stockItemId);
                //if (index >= 0) //if index >= 0, the item exists inside oiList
                //{
                //    oiList[index].Quantity = quantity;
                //    ordersRepo.UpsertOrderItem(oiList[index]);
                //}
                //else //if the item does not exist
                //{
                //    OrderItem oi = new OrderItem(oh, si.Id, si.Name, si.Price, quantity);
                //    oh.AddOrderItem(oi);
                //    ordersRepo.UpsertOrderItem(oi);
                //}
                //return oh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to change the state of newly added order into "Pending".
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        /// <returns>OrderHeader</returns>
        public OrderHeader SubmitOrder(int orderHeaderId)
        {
            try
            {
                //call OrderRepo method - GetOrderHeader to get OrderHeader object
                OrderHeader oh = ordersRepo.GetOrderHeader(orderHeaderId);
                //call OrderHeader method - submit
                oh.Submit();
                //call OrderRepo method - UpdateOrderState
                ordersRepo.UpdateOrderState(oh);
                //return OrderHeader object
                return oh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to process a pending order.
        /// If any of the quantities entered for order items is greater than stock, then the order state will be "Reject".
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        /// <returns>OrderHeader</returns>
        public OrderHeader ProcessOrder(int orderHeaderId)
        {
            //call OrderRepo method - GetOrderHeader to get OrderHeader object
            OrderHeader oh = ordersRepo.GetOrderHeader(orderHeaderId);
            //call StockItemRepo method - UpdateStockItemAmount. If no exception thrown, call OrderHeader method - Complete. If exception thrown, call OrderHeader method - Reject.
            try
            {
                stockItemsRepo.UpdateStockItemAmount(oh);
                oh.Complete();
            }
            catch (SqlException ex)
            {
                oh.Reject();
            }
            //call OrderRepo method - UpdateOrderState
            ordersRepo.UpdateOrderState(oh);
            //return OrderHeader object
            return oh;
        }

        /// <summary>
        /// Method to delete an order and any order items added for this order.
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            try
            {
                //call OrderRepo method - DeleteOrderHeaderAndOrderItems
                ordersRepo.DeleteOrderHeaderAndOrderItems(orderHeaderId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to delete an order item from an order.
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        /// <param name="stockItemId">StockItem id</param>
        /// <returns>OrderHeader</returns>
        public OrderHeader DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            try
            {
                //call OrderRepo method - DeleteOrderItem
                ordersRepo.DeleteOrderItem(orderHeaderId, stockItemId);
                //call OrderRepo method - GetOrderHeader to get OrderHeader object
                OrderHeader oh = ordersRepo.GetOrderHeader(orderHeaderId);
                //return OrderHeader object
                return oh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
