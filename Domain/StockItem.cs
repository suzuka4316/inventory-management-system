using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A class that describes stock item's information.
    /// </summary>
    public class StockItem
    {
        /// <summary>
        /// property that gets and sets a stock item id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// property that gets and sets stocks of an item.
        /// </summary>
        public int InStock { get; set; }
        /// <summary>
        /// property that gets and sets a stock item's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// property that gets and sets a stock item's price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="inStock">Current stocks of an item</param>
        public StockItem(int id, string name, decimal price, int inStock)
        {
            Id = id;
            InStock = inStock;
            Name = name;
            Price = price;
        }
    }
}
