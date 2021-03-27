﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;

namespace VitoshaBank.Data.RequestModels
{
    public class UserRequestModel
    {
        public User User { get; set; }
        public string Username { get; set; }
        public string CurrentPassword { get; internal set; }
        public string Password { get; internal set; }
    }
}
