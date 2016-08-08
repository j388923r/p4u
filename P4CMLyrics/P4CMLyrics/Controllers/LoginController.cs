using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AngularJSWebApiEmpty.Models;
using AngularJSWebApiEmpty.Models.Entity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Newtonsoft.Json;

namespace AngularJSWebApiEmpty.Controllers
{
    public class LoginController : ApiController
    {
        static CloudStorageAccount account;
        static CloudTableClient t_client;
        static CloudTable t_reference;
        static CloudBlobClient b_client;
        static CloudBlobContainer container;
        static string blobUriPrefix = "http://p4cmlyrics.blob.core.windows.net/users/";

        static LoginController()
        {
            account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountKey"]);
            t_client = account.CreateCloudTableClient();
            t_reference = t_client.GetTableReference("user");
            t_reference.CreateIfNotExists();
            b_client = account.CreateCloudBlobClient();
            container = b_client.GetContainerReference("users");
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                }
            );
        }

        [HttpPost]
        public User Login(User user)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("username", QueryComparisons.Equal, user.username),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("passwordHash", QueryComparisons.Equal, user.password.GetHashCode().ToString())));

            foreach (UserEntity entity in t_reference.ExecuteQuery(query))
            {
                CloudBlockBlob b_reference = container.GetBlockBlobReference(entity.Username);
                User stored = JsonConvert.DeserializeObject<User>(b_reference.DownloadText());
                stored.password = "";
                return stored;
            }

            return new User() { Name = "Does not exist" };
        }
    }
}
