using System;

namespace Client.Models.Interfaces
{
    internal interface IAccount
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastRequest { get; set; }
    }
}
