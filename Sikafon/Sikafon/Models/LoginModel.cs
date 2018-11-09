using System;
using System.Collections.Generic;
using System.Text;

namespace Sikafon.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginReturn
    {
        public int Id { get; set; }
    }
}
