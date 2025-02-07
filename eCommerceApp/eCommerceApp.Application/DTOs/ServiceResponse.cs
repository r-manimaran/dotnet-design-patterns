using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.DTOs;

public record ServiceResponse(bool Success=false, string? Message=null);
