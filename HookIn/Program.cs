using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers;

namespace HookIn
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Testing states
            //OrderHeader o = new OrderHeader(1,1,DateTime.Now);

            //try
            //{
            //    o.Submit();
            //    Console.WriteLine($"State: {o.State.GetType().Name}");
            //    o.Complete();
            //    Console.WriteLine($"State: {o.State.GetType().Name}");
            //    o.Reject();
            //    Console.WriteLine($"State: {o.State.GetType().Name}");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            ////Testing OrderItem class
            //OrderHeader order = new OrderHeader(1);
            //OrderItem item1 = new OrderItem("fancy chair", order, 25.99, 3, 1);
            //OrderItem item2 = new OrderItem("fancy curtain", order, 20.99, 4, 2);
            //order.AddOrderItem(item1);
            //order.AddOrderItem(item2);
            //Console.WriteLine($"{item1.Description}, Total: {item1.Total}\n{item2.Description}, Total: {item2.Total}\nOrderTotal: {order.Total}");



            /********************OrdersRepo*********************/
            OrdersRepo repo = new OrdersRepo();
            //StockItemsRepo stockItems = new StockItemsRepo();

            //Testing GetOrderHeader
            OrderHeader o1 = repo.GetOrderHeader(11);
            Console.WriteLine($"{o1.Id} {o1.State} {o1.DateTime} {o1.OrderItems} {o1.Total}");

            ////Testing InsertOrderHeader
            //OrderHeader o2 = repo.InsertOrderHeader();
            //Console.WriteLine($"{o2.Id} {o2.State} {o2.DateTime}");

            //Testing GetOrderHeaders
            IEnumerable<OrderHeader> o3 = repo.GetOrderHeaders();
            foreach (var i in o3)
            {
                Console.WriteLine($"header id: {i.Id} header state: {i.State} header datetime: {i.DateTime} header total: {i.Total}");
                List<OrderItem> itemList = i.OrderItems;
                foreach (var item in itemList)
                {
                    Console.WriteLine($"description: {item.Description} header id: {item.OrderHeaderId} item quantity: {item.Quantity} item price: {item.Price} stock item id: {item.StockItemId}");
                }
                Console.WriteLine();
            }

            ////Testing UpsertOrderItem
            ////existing record
            //OrderItem orderItem = new OrderItem("Single Bed", o1, 120, 4, 6);
            //repo.UpsertOrderItem(orderItem);
            ////new record
            //OrderItem orderItem2 = new OrderItem("Table", o2, 100, 1, 1);
            //repo.UpsertOrderItem(orderItem2);

            ////Testing UpdateOrderState
            //o1.setState(1);
            //repo.UpdateOrderState(o1);

            ////Testing DeleteOrderHeaderAndOrderItems
            //repo.DeleteOrderHeaderAndOrderItems(11);

            ////Testing DeleteOrderItem
            //repo.DeleteOrderItem(13, 6);


            /********************StockItemsRepo*********************/
            ////Testing GetStockItems
            //IEnumerable<StockItem> list = stockItems.GetStockItems();
            //foreach (var i in list)
            //{
            //    Console.WriteLine($"{i.Id} {i.Name} {i.Price} {i.InStock}");
            //}

            ////Testing GetStockItem
            //StockItem s = stockItems.GetStockItem(1);
            //Console.WriteLine($"{s.Id} {s.Name} {s.Price} {s.InStock}");

            ////Testing UpdateStockItemAmount
            //OrderHeader o4 = repo.GetOrderHeader(11);
            //stockItems.UpdateStockItemAmount(o4);


            /********************OrdersController*********************/
            OrdersController oc = new OrdersController();

            ////Testing GetOrderHeaders() 
            //IEnumerable <OrderHeader> ohList = oc.GetOrderHeaders();
            //foreach (var i in ohList)
            //{
            //    Console.WriteLine($"header id: {i.Id} header state: {i.State} header datetime: {i.DateTime} header total: {i.Total}");
            //    List<OrderItem> itemList = i.OrderItems;
            //    foreach (var item in itemList)
            //    {
            //        Console.WriteLine($"description: {item.Description} header id: {item.OrderHeaderId} item quantity: {item.Quantity} item price: {item.Price} stock item id: {item.StockItemId}");
            //    }
            //    Console.WriteLine();
            //}

            ////Testing CreateNewOrderHeader()
            //OrderHeader oh = oc.CreateNewOrderHeader();
            //Console.WriteLine($"header id: {oh.Id} header state: {oh.State} header datetime: {oh.DateTime} header total: {oh.Total}");

            //Testing UpsertOrderItem
            OrderHeader oh = oc.UpsertOrderItem(11, 4, 10); //changing the number of Wardrobe order to 10 from 8 for order id 11
            oc.UpsertOrderItem(11, 2, 1); //adding a new order(Chair) to order id 11
            Console.WriteLine($"header id: {oh.Id} header state: {oh.State} header datetime: {oh.DateTime} header total: {oh.Total}");

            ////Testing SubmitOrder()
            //OrderHeader oh = oc.SubmitOrder(91);
            //Console.WriteLine($"header id: {oh.Id} header state: {oh.State} header datetime: {oh.DateTime} header total: {oh.Total}");

            ////Testing ProcessOrder()
            //OrderHeader oh = oc.ProcessOrder(13); //orderheaderid 13 is currently pending(2), and will change to complete(4)
            //Console.WriteLine($"header id: {oh.Id} header state: {oh.State} header datetime: {oh.DateTime} header total: {oh.Total}");

            ////Testing DeleteOrderHeaderAndOrderItems()
            //oc.DeleteOrderHeaderAndOrderItems(11); //orderheaderid 11 currently has 3 line items: table, wardrobe, and single bed, and they will be also deleted

            ////Testing DeleteOrderItem
            //OrderHeader oh = oc.DeleteOrderItem(6, 8); //Deleting Queen Bed(8) from orderheaderid 6


            /********************StockItemsController*********************/
            //StockItemsController sc = new StockItemsController();

            ////Testing GetStockItems()
            //IEnumerable<StockItem> siList = sc.GetStockItems();
            //foreach (var stockItem in siList)
            //{
            //    Console.WriteLine($"ID: {stockItem.Id} Name: {stockItem.Name} Price: {stockItem.Price} InStock: {stockItem.InStock}");
            //}

            Console.ReadKey();
        }
    }
}
