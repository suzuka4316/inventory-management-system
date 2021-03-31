 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A class that describes ordered item's information.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Property that gets and sets a description of an item.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Property that gets and sets a price of an item.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Property that gets and sets a quantity of an item.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Read-only property that returns total price of an item.
        /// </summary>
        public double Total {
            get
            {
                return (double)Price * Quantity;
            }
        }

        /// <summary>
        /// Read-only property that returns OrderHeader object
        /// </summary>
        public OrderHeader OrderHeader { get; }
        /// <summary>
        /// Read-only property that returns OrderHeader id.
        /// </summary>
        public int OrderHeaderId {
            get
            {
                return OrderHeader.Id;
            }
        }
        /// <summary>
        /// Read-only property that returns stock item id.
        /// </summary>
        public int StockItemId { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderHeader"></param>
        /// <param name="stockItemId"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        public OrderItem(OrderHeader orderHeader, int stockItemId, string description, decimal price, int quantity)
        {
            Description = description;
            OrderHeader = orderHeader;
            Price = price;
            Quantity = quantity;
            StockItemId = stockItemId;
        }
    }
}
