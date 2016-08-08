using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularJSWebApiEmpty.Models.Entity;

namespace AngularJSWebApiEmpty.Models
{
    public class Piece
    {
        public Piece() { }

        public Piece(PieceEntity entity)
        {
            title = entity.Title;
            p4cmlink = entity.p4cmlink;
            artist = entity.Artist;
            uploadDate = entity.uploadDate;
            blobName = entity.blobName;
            owner = entity.Owner;
        }

        public string title { get; set; }

        public string p4cmlink { get; set; }

        public string artist { get; set; }

        public string blobName { get; set; }

        public DateTime uploadDate { get; set; }

        public string owner { get; set; }

        [JsonProperty]
        public List<PiecePlain> plainComponents { get; set; }

        [JsonProperty]
        public List<PieceHighlight> highlightComponents { get; set; }

        public bool allowAnonymousSuggestions { get; set; }
    }
}