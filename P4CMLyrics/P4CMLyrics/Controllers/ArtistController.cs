using AngularJSWebApiEmpty.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJSWebApiEmpty.Controllers
{
    public class ArtistController : ApiController
    {
        static CloudStorageAccount account;
        static CloudTableClient t_client;
        static CloudBlobClient b_client;
        static CloudBlobContainer container;

        static ArtistController()
        {
            account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountKey"]);
            t_client = account.CreateCloudTableClient();
            b_client = account.CreateCloudBlobClient();
            container = b_client.GetContainerReference("artists");
            container.CreateIfNotExists();
        }

        [HttpGet]
        public Artist getArtist(string artistId)
        {
            if (artistId.Equals("sample"))
                return getSampleArtist();
            return new Artist() { };
        }

        public Artist getSampleArtist()
        {
            return new Artist() { };
        }
    }
}
