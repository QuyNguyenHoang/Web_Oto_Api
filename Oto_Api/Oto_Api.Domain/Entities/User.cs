﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Oto_Api.Domain.Entities
{
    public class User : IdentityUser
    {
        public virtual UserInfo? UserInfo { get; set; }
    }
}
