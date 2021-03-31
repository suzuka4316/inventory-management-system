using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// Class for a new order.
    /// </summary>
    public class OrderNew : OrderState
    {
        /// <summary>
        /// enum to change an order state to New.
        /// </summary>
        public override OrderStateEnum State => OrderStateEnum.New;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderHeader">OrderHeader object</param>
        public OrderNew(OrderHeader orderHeader) : base(orderHeader) { }

        /// <summary>
        /// Method to change an order's state to Submit.
        /// </summary>
        public override void Submit()
        {
            //business rule: an order must has at least one order item
            if (!_orderHeader.OrderItems.Any())
            {
                throw new InvalidOperationException("A new order must have at least one item before it can be submitted");
            }
            _orderHeader.setState(2);
        }

        /// <summary>
        /// Method to change an order's state to Reject.
        /// </summary>
        public override void Reject()
        {
            _orderHeader.setState(3);
        }

        /// <summary>
        /// Method to change an order's state to Complete.
        /// </summary>
        public override void Complete()
        {
            _orderHeader.setState(4);
        }
    }
}
