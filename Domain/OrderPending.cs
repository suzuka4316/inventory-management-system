using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// Class for an order that has been submitted.
    /// </summary>
    public class OrderPending : OrderState
    {
        /// <summary>
        /// enum to change an order state to Pending.
        /// </summary>
        public override OrderStateEnum State => OrderStateEnum.Pending;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderHeader">OrderHeader object</param>
        public OrderPending(OrderHeader orderHeader):base(orderHeader){}

        /// <summary>
        /// Method to change an order's state to Submit.
        /// </summary>
        public override void Submit()
        {
            throw new InvalidStateException("Already pending");
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
