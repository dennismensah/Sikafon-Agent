using System;
using System.Collections.Generic;
using System.Text;

namespace Sikafon.Models
{
    public class Activation
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int AgentId { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
