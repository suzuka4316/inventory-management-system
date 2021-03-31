using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A class that describes an order's information.
    /// </summary>
    public class OrderHeader
    {
        //field
        private OrderState _state;
        //list of order item
        private List<OrderItem> _orderItems = new List<OrderItem>();

        /// <summary>
        /// A property that gets and sets DateTime
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// A property that gets and sets an id of an order.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// A read-only property that calculates total price of an order.
        /// </summary>
        public double Total {
            get
            {
                double total = 0;
                foreach (OrderItem i in _orderItems)
                {
                    total += i.Total;
                }
                return total;
            }
        }

        /// <summary>
        /// A read-only property that returns a list of OrderItem objects.
        /// </summary>
        public List<OrderItem> OrderItems
        {
            get { return _orderItems; }
        }


        /// <summary>
        /// Method that sets a state of an order.
        /// </summary>
        /// <param name="stateNum">integer from 1 to 4</param>
        public void setState(int stateNum)
        {
            switch (stateNum)
            {
                case 1:
                    _state = new OrderNew(this);
                    break;
                case 2:
                    _state = new OrderPending(this);
                    break;
                case 3:
                    _state = new OrderRejected(this);
                    break;
                case 4:
                    _state = new OrderComplete(this);
                    break;
                default:
                    throw new InvalidStateException($"Invalid State Id: {stateNum}");
            }
        }

        /// <summary>
        /// A read-only property that returns an order's state.
        /// </summary>
        public OrderState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Constructor that takes order's id, state, and datetime.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stateNum"></param>
        /// <param name="dateTime"></param>
        public OrderHeader(int id, int stateNum, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
            _orderItems = new List<OrderItem>();
            setState(stateNum);
        }

        /// <summary>
        /// Method that changes an order's state to Submit (Pending).
        /// </summary>
        public void Submit()
        {
            _state.Submit();
        }
        /// <summary>
        /// Method that changes an order's state to Complete.
        /// </summary>
        public void Complete()
        {
            _state.Complete();
        }
        /// <summary>
        /// Method that changes an order's state to Reject.
        /// </summary>
        public void Reject()
        {
            _state.Reject();
        }

        /// <summary>
        /// Method that adds order items information to an order.
        /// </summary>
        /// <param name="stockItemId"></param>
        /// <param name="price">price of an item</param>
        /// <param name="description">name of an item</param>
        /// <param name="quantity"></param>
        /// <returns>OrderItem</returns>
        public OrderItem AddOrderItem(int stockItemId, decimal price, string description, int quantity)
        {
            OrderItem item = null;
            //check to see if there is already an existing order item for the selected stock item
            foreach (var i in OrderItems)
            {
                if (i.StockItemId == stockItemId)
                {
                    item = i;
                }
            }
            //if there isn't create a new instance and add it to the collection of order items
            if (item == null)
            {
                item = new OrderItem(this, stockItemId, description, price, quantity);
                OrderItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            //return the order item object
            return item;
        }
    }
}
