﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Validator.Interface
{
    public interface ITokenValidator
    {
        ClaimsPrincipal ValidateToken(string token);
    }
}
