using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A class to throw an error for order states.
    /// </summary>
    public class InvalidStateException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">error message</param>
        public InvalidStateException(string msg) : base(msg) { }
    }
}
