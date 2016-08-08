using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models.Entity
{
    public class UserEntity : TableEntity
    {
        public string username { get; set; }
        public string passwordHash { get; set; }

        public string Username { 
            get
            {
                return username;
            }
            set {
                username = value;
                PartitionKey = "User";
                RowKey = value;
            }
        }

        public string Name { get; set; }
    }
}