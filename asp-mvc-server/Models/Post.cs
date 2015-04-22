using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asp_mvc_server.Models
{
    [BsonKnownTypes(typeof(Post))]
    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string user { get; set; }

    }
}