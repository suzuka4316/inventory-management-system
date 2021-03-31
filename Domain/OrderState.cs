using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// enum that represents a group of states.
    /// </summary>
    public enum OrderStateEnum { 
        /// <summary>
        /// New order
        /// </summary>
        New = 1, 
        /// <summary>
        /// Pending order
        /// </summary>
        Pending = 2, 
        /// <summary>
        /// Rejected order
        /// </summary>
        Reject = 3, 
        /// <summary>
        /// Completed order
        /// </summary>
        Complete = 4 
    }

    /// <summary>
    /// Abstract class that's inherited by 4 states classes.
    /// </summary>
    public abstract class OrderState
    {
        /// <summary>
        /// protected field of OrderHeader object
        /// </summary>
        protected OrderHeader _orderHeader;

        /// <summary>
        /// Read-only property that returns an order's state.
        /// </summary>
        public abstract OrderStateEnum State { get; }

        /// <summary>
        /// Constructor that assigns OrderHeader object to the field.
        /// </summary>
        /// <param name="orderHeader">OrderHeader object</param>
        public OrderState(OrderHeader orderHeader)
        {
            _orderHeader = orderHeader;
        }

        /// <summary>
        /// Abstruct method that's overridden by 4 states classes.
        /// </summary>
        public abstract void Submit();

        /// <summary>
        /// Abstruct method that's overridden by 4 states classes.
        /// </summary>
        public abstract void Complete();

        /// <summary>
        /// Abstruct method that's overridden by 4 states classes.
        /// </summary>
        public abstract void Reject();
    }
}
