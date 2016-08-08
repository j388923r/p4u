using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models
{
    public class PieceC
    {
        public string type { get; set; }

        public string text { get; set; }
    }
    
    public class PieceComponent
    {
        public string type { get; set; }
    }

    public class PiecePlain : PieceComponent
    {
        public string text { get; set; }

        public int index { get; set; }
    }

    public class PieceHighlight : PieceComponent
    {
        public string annotationHTML { get; set; }

        public string referenceType { get; set; }

        public string text { get; set; }

        public int index { get; set; }
    }
}