using System;
using System.Collections.Generic;
using System.Text;

namespace ecp_messenger.Models
{
    public class Dialog
    {
        public User User { get; set; }
        public List<Message> messages { get; set; }
        public Dialog() { }
    }
}
