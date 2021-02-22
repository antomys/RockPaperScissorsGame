using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public enum StringDestination
    {
        Login,
        Password,
        Email,
        PassportType //If firstname, lastname...(data without digits!)
    }
}
