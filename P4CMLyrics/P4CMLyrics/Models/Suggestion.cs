using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models
{
    public class Suggestion
    {
        public string source { get; set; }
        public string pieceId { get; set; }
        public int pieceIndex { get; set; }
        public string content { get; set; }
        public string type { get; set; }
    }
}