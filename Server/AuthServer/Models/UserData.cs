using System;
using Newtonsoft.Json;

namespace AuthServer.Models
{
    public class UserData
    {
        public string Username { get; set; }
        public int Coins { get; set; }
        public string Password { get; set; }
    }
}