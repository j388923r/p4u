using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibleVerseBot.Models
{
    public class Conversation
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public string eTag { get; set; }
    }
}