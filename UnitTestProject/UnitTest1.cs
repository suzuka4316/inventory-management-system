using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using DataAccess;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    /// <summary>
    /// A class to test all the method in Controller project.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private OrdersRepo ordersRepo = new OrdersRepo();
        private StockItemsRepo stockItemsRepo = new StockItemsRepo();

        /// <summary>
        /// Method that tests GetOrderHeaders method from OrdersController.
        /// </summary>
        [TestMethod]
        public void GetOrderHeaders()
        {
            IEnumerable<OrderHeader> ohList = OrdersController.Instance.GetOrderHeaders();
            //Check if returned list contains more than 1 object.
            Assert.IsTrue(ohList.Count() >= 1);
        }

        /// <summary>
        /// Method that tests CreateNewOrderHeader method from OrdersController.
        /// </summary>
        [TestMethod]
        public void CreateNewOrderHeader()
        {
            OrderHeader oh = OrdersController.Instance.CreateNewOrderHeader();
            //Check if returned value type is OrderHeader.
            Assert.IsTrue(oh is OrderHeader);
        }

        /// <summary>
        /// Method that tests UpsertOrderItem method from OrdersController.
        /// </summary>
        [TestMethod]
        public void UpsertOrderItem()
        {
            //OrderHeaderId is 6, StockItemId is 9 (King Bed)
            OrderHeader oh = ordersRepo.GetOrderHeader(6);
            StockItem si = stockItemsRepo.GetStockItem(9);

            oh = OrdersController.Instance.UpsertOrderItem(oh.Id, si.Id, 2);

            foreach (var orderItem in oh.OrderItems)
            {
                if (orderItem.StockItemId == si.Id)
                {
                    //Check if quantity is greater than 0.
                    Assert.IsTrue(orderItem.Quantity > 0);
                }
            }
        }

        /// <summary>
        /// Method that tests SubmitOrder method from OrdersController.
        /// </summary>
        [TestMethod]
        public void SubmitOrder()
        {
            OrderHeader oh = OrdersController.Instance.CreateNewOrderHeader();
            oh = ordersRepo.GetOrderHeader(oh.Id);
            
            //adding 2 tables in the created order
            oh = OrdersController.Instance.UpsertOrderItem(oh.Id, 1, 2);

            oh = OrdersController.Instance.SubmitOrder(oh.Id);
            //Check if an order's state is Pending.
            Assert.IsTrue(oh.State is OrderPending);
        }

        /// <summary>
        /// Method that tests ProcessOrder method from OrdersController.
        /// </summary>
        [TestMethod]
        public void ProcessOrder()
        {
            OrderHeader oh = OrdersController.Instance.CreateNewOrderHeader();
            oh = ordersRepo.GetOrderHeader(oh.Id);
            //adding 2 tables in the created order
            oh = OrdersController.Instance.UpsertOrderItem(oh.Id, 1, 2);
            oh = OrdersController.Instance.SubmitOrder(oh.Id);

            oh = OrdersController.Instance.ProcessOrder(oh.Id);
            if (oh.State is OrderRejected)
            {
                //Check if an order's state is Reject.
                Assert.IsTrue(oh.State is OrderRejected);
            }
            else
            {
                //Check if an order's state is Complete.
                Assert.IsTrue(oh.State is OrderComplete);
            }
        }

        /// <summary>
        /// Method that tests DeleteOrderHeaderAndOrderItems method from OrdersController.
        /// </summary>
        [TestMethod]
        public void DeleteOrderHeaderAndOrderItems()
        {
            IEnumerable<OrderHeader> ohList = OrdersController.Instance.GetOrderHeaders();

            bool doesExist = ohList.Any(order => order.Id == 6);
            //if OrderHeaderId 6 exists in OrderHeader list
            if (doesExist == true)
            {
                ordersRepo.DeleteOrderHeaderAndOrderItems(6);
                ohList = OrdersController.Instance.GetOrderHeaders();
                doesExist = ohList.Any(order => order.Id == 6);
                //Check if OrderHeader(id 6) does not exist.
                Assert.IsFalse(doesExist);
            }
        }

        /// <summary>
        /// Method that tests DeleteOrderItem method from OrdersController.
        /// </summary>
        [TestMethod]
        public void DeleteOrderItem()
        {
            IEnumerable<OrderHeader> ohList = OrdersController.Instance.GetOrderHeaders();

            bool doesExistOrder = ohList.Any(order => order.Id == 8);
            //if OrderHeaderId 8 exists in OrderHeader list
            if (doesExistOrder == true)
            {
                OrderHeader oh = ordersRepo.GetOrderHeader(8);
                bool doesExistItem = oh.OrderItems.Any(item => item.StockItemId == 5);
                if (doesExistItem == true)
                {
                    StockItem si = stockItemsRepo.GetStockItem(5);
                    oh = OrdersController.Instance.DeleteOrderItem(oh.Id, si.Id);
                    doesExistOrder = ohList.Any(order => order.Id == 8);
                    doesExistItem = oh.OrderItems.Any(item => item.StockItemId == 5);
                    //Check if OrderHeader(id 8) still exists
                    Assert.IsTrue(doesExistOrder);
                    //Check if OrderItem(stock item 5) does not exist.
                    Assert.IsFalse(doesExistItem);
                }
            }
        }

        /// <summary>
        /// Method that tests GetStockItems method from StockItemsController.
        /// </summary>
        [TestMethod]
        public void GetStockItems()
        {
            IEnumerable<StockItem> siList = StockItemsController.Instance.GetStockItems();
            //Check if returned list contains more than 1 object.
            Assert.IsTrue(siList.Count() >= 1);
        }
    }
}
