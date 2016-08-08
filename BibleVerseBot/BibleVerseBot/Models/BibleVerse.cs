using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibleVerseBot.Models
{
    public class BibleVerse
    {
        public string Book { get; set; }
        public int Chapter { get; set; }
        public int StartVerse { get; set; }
        public int EndVerse { get; set; }
        public string Text { get; set; }
    }
}