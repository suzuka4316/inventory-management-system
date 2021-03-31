using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// Class for an order that has been processed and rejected.
    /// </summary>
    public class OrderRejected : OrderState
    {
        /// <summary>
        /// enum to change an order state to Reject.
        /// </summary>
        public override OrderStateEnum State => OrderStateEnum.Reject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderHeader">OrderHeader object</param>
        public OrderRejected(OrderHeader orderHeader) : base(orderHeader) { }

        /// <summary>
        /// Method to change an order's state to Submit.
        /// </summary>
        public override void Submit()
        {
            throw new InvalidStateException("Already rejected");
        }

        /// <summary>
        /// Method to change an order's state to Complete.
        /// </summary>
        public override void Complete()
        {
            throw new InvalidStateException("Already rejected");
        }

        /// <summary>
        /// Method to change an order's state to Reject.
        /// </summary>
        public override void Reject()
        {
            throw new InvalidStateException("Already rejected");
        }
    }
}
