using System;
using System.Collections.Generic;
using System.Text;

namespace ecp_messenger.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int fromId { get; set; }
        public int toId { get; set; }
        public string text { get; set; }
        public string sendTime { get; set; }
    }
}
