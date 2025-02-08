using AutoMapper;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Mappings;

public class UserProfile :Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, AppUser>();
        CreateMap<LoginRequest, AppUser>();
    }
}
