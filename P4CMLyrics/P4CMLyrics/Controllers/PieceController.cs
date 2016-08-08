using AngularJSWebApiEmpty.Models;
using AngularJSWebApiEmpty.Models.Entity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AngularJSWebApiEmpty.Controllers
{
    public class PieceController : ApiController
    {
        static CloudStorageAccount account;
        static CloudTableClient t_client;
        static CloudTable t_reference;
        static CloudBlobClient b_client;
        static CloudBlobContainer container;
        static string blobUriPrefix = "http://p4cmlyrics.blob.core.windows.net/pieces/";

        static PieceController()
        {
            account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountKey"]);
            t_client = account.CreateCloudTableClient();
            t_reference= t_client.GetTableReference("piece");
            t_reference.CreateIfNotExists();
            b_client = account.CreateCloudBlobClient();
            container = b_client.GetContainerReference("pieces");
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                }
            );
        }

        [HttpPost]
        public Piece savePiece(Piece piece)
        {
            piece.uploadDate = DateTime.Now;
            PieceEntity entity = EntityMediator.getTableEntity(piece);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(piece.title);
            blockBlob.UploadTextAsync(JsonConvert.SerializeObject(piece));
            TableOperation insertOperation = TableOperation.InsertOrReplace(entity);
            t_reference.Execute(insertOperation);
            return piece;
        }

        [HttpGet]
        public async Task<Piece> getPiece(string pieceId)
        {
            Piece piece = new Piece() { };
            if (pieceId.Equals("sample"))
            {
                piece = getSamplePiece();
                // PieceEntity entity = getTableEntity(piece);
                // TableOperation insertOperation = TableOperation.InsertOrReplace(entity);
                // t_reference.Execute(insertOperation);
                TableQuery<PieceEntity> query = new TableQuery<PieceEntity>().Where(TableQuery.GenerateFilterCondition("blobName", QueryComparisons.Equal, pieceId));

                foreach (PieceEntity entity in t_reference.ExecuteQuery(query))
                {
                    string pieceBlob = await getPieceBlob(pieceId);
                    piece = JsonConvert.DeserializeObject<Piece>(pieceBlob);
                }
                return piece;
            }
            else
            {
                TableQuery<PieceEntity> query = new TableQuery<PieceEntity>().Where(TableQuery.GenerateFilterCondition("blobName", QueryComparisons.Equal, pieceId));

                // Print the fields for each customer.
                foreach (PieceEntity entity in t_reference.ExecuteQuery(query))
                {
                    string pieceBlob = await getPieceBlob(pieceId);
                    piece = JsonConvert.DeserializeObject<Piece>(pieceBlob);
                }
                return piece;
            }
        }

        [HttpGet]
        public async Task<string> getPieceBlob(string blobName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            return await blockBlob.DownloadTextAsync();
        }

        public Piece getSamplePiece()
        {
            return new Piece() { artist = "Ezekiel Azonwu", owner = "j388923r", title = "Dear Man", uploadDate = DateTime.Now, blobName = "sample", p4cmlink = "v=rLpbuoxfaGU", allowAnonymousSuggestions = true };
        }
    }
}
