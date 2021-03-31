using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// A class for controlling the application flow and passing messages (commands, queries, domain objects) between the data access and User Interface layers. 
    /// </summary>
    public class StockItemsController
    {
        private StockItemsRepo stockItemsRepo;

        private static StockItemsController _instance;

        /// <summary>
        /// Null constructor.
        /// </summary>
        public StockItemsController() {
            stockItemsRepo = new StockItemsRepo();
        }

        /// <summary>
        /// Static method to instantiate an StockItemsController object.
        /// </summary>
        public static StockItemsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StockItemsController();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Method to get all the stock items.
        /// </summary>
        /// <returns>IEnumerable of StockItem objects</returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            try
            {
                //call StockRepo method - GetStockItems()
                IEnumerable<StockItem> siList = stockItemsRepo.GetStockItems();
                //return the StockItems
                return siList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
