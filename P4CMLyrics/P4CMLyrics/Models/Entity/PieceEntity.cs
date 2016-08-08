using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models.Entity
{
    public class PieceEntity : TableEntity
    {
        private string title;
        private string owner;
        
        public string Title
        {
            get
            {
                return title;
            }
            set {
                title = value;
                RowKey = value;
            }
        }

        public string p4cmlink { get; set; }

        public string Artist { get; set; }

        public string blobName { get; set; }

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
                PartitionKey = value;
            }
        }

        public DateTime uploadDate { get; set; }
    }
}