using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Contracts
{
    public record OrderCreatedIntegrationEvent(Guid Id, string OrderId);
   
}
