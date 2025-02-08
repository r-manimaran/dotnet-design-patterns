﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty; // other properties come from IdentityUser
}
