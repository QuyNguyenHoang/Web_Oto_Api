﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.Auth
{
    public class EmailSettings
    {
        public string? FromEmail { get; set; }
        public string? Password { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
    }
}
