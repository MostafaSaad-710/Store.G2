using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptions
{
    public class CreateOrderBadRequestException() : BadRequestException($"Invalid Operation When Create order !! ")
    {
    }
}
