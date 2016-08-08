using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AngularJSWebApiEmpty.Models;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using AngularJSWebApiEmpty.Models.Entity;
using Newtonsoft.Json;

namespace AngularJSWebApiEmpty.Controllers
{
    public class RegisterController : ApiController
    {
        static CloudStorageAccount account;
        static CloudTableClient t_client;
        static CloudTable t_reference;
        static CloudBlobClient b_client;
        static CloudBlobContainer container;
        static string blobUriPrefix = "http://p4cmlyrics.blob.core.windows.net/users/";

        static RegisterController()
        {
            account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountKey"]);
            t_client = account.CreateCloudTableClient();
            t_reference= t_client.GetTableReference("user");
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
        public User Register(User user)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("username", QueryComparisons.Equal, user.username));

            // Print the fields for each customer.
            foreach (UserEntity entity in t_reference.ExecuteQuery(query))
            {
                return new User() { username = "Already Taken" };
            }

            if (!String.IsNullOrWhiteSpace(user.username) && !String.IsNullOrWhiteSpace(user.password))
            {
                user.Name = (!String.IsNullOrWhiteSpace(user.Name)) ? user.Name : user.username;
                TableOperation insertOperation = TableOperation.InsertOrReplace(EntityMediator.getTableEntity(user));
                t_reference.Execute(insertOperation);
                user.password = "";
                CloudBlockBlob b_reference = container.GetBlockBlobReference(user.username);
                b_reference.UploadText(JsonConvert.SerializeObject(user));
            }
            else
            {
                return new User() { username = "Invalid Username/Password" };
            }

            user.password = "";

            return user;
        }
    }
}
