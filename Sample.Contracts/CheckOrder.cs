using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Contracts
{
    public interface CheckOrder
    {
        Guid OrderId { get; }
    }
}
