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
    public class SuggestionController : ApiController
    {
        static CloudStorageAccount account;
        static CloudTableClient t_client;
        static CloudTable t_reference;
        static CloudBlobClient b_client;
        static CloudBlobContainer container;
        static string blobUriPrefix = "http://p4cmlyrics.blob.core.windows.net/suggestions/";

        static SuggestionController()
        {
            /*account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountKey"]);
            t_client = account.CreateCloudTableClient();
            t_reference= t_client.GetTableReference("suggestion");
            t_reference.CreateIfNotExists();
            b_client = account.CreateCloudBlobClient();
            container = b_client.GetContainerReference("suggestions");
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                }
            );*/
        }

        [HttpPost]
        public Suggestion saveSuggestion(Suggestion s)
        {
            Suggestion suggestion = new Suggestion();

            return suggestion;
        }

        [ActionName("piece")]
        [HttpGet]
        public List<Suggestion> getSuggestions(string pieceId)
        {
            Suggestion s = new Suggestion()
            {
                source = "saffron",
                pieceId = "Dear Man",
                type = "correction",
                content = "Should be \"Dear mannnh\" instead of \"Dear Man\"",
                pieceIndex = 0
            };

            List<Suggestion> suggestions = new List<Suggestion>() { s };

            return suggestions;
        }

        [ActionName("from")]
        [HttpGet]
        public List<Suggestion> getSuggestionsFrom(string froUmser)
        {
            return new List<Suggestion>();
        }

        [ActionName("to")]
        [HttpGet]
        public List<Suggestion> getSuggestionsTo(string toUser)
        {
            return new List<Suggestion>();
        }
    }
}
