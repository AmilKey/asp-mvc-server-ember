using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asp_mvc_server.Models
{
    public class Apikey
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string token { get; set; }
        public string token_type { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string user { get; set; }
    }
}