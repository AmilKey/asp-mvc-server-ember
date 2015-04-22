using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asp_mvc_server.Models
{
    public class User
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }

        [JsonIgnore]
        public string login { get; set; }
        [JsonIgnore]
        public string password { get; set; }
        public string created_at { get; set; }
        public DateTime updated_at { get; set; }

        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string apikey { get; set; }

        public List<String> posts { get; set; }
    }
}