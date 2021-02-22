using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Interfaces
{
    internal interface IAccount
    {
        string Login { get; }
        //string Email { get; }
        string Password { get; }
    }
}
