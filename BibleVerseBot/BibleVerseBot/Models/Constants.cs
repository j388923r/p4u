using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BibleVerseBot.Models
{
    public class Constants
    {
        private static string direct_line_key;

        static Constants()
        {
            direct_line_key = ConfigurationManager.AppSettings["DirectLineSecret"];
        }

        public static string DirectLineKey {
            get
            {
                return direct_line_key;
            }
        }
    }
}