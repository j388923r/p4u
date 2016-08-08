using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models.Entity
{
    public class EntityMediator
    {
        public static UserEntity getTableEntity(User user)
        {
            return new UserEntity()
            {
                Username = user.username,
                passwordHash = user.password.GetHashCode().ToString(),
                Name = ( !String.IsNullOrWhiteSpace(user.Name) ) ? user.Name : user.username,
                ETag = "*"
            };
        }

        public static PieceEntity getTableEntity(Piece piece)
        {
            return new PieceEntity()
            {
                Title = piece.title,
                p4cmlink = piece.p4cmlink,
                Artist = piece.artist,
                blobName = piece.title.Replace("'", String.Empty),
                Owner = piece.owner,
                uploadDate = DateTime.Now,
                ETag = "*"
            };
        }
    }
}