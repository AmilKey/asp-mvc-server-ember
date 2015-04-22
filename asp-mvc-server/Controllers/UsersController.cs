using asp_mvc_server.Models;
using asp_mvc_server.Properties;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace asp_mvc_server.Controllers
{
    public class UsersController : ApiController
    {
        public MongoDatabase MongoDatabase;
        public MongoCollection UsersCollection;
        public MongoCollection ApikeyCollection;

        public UsersController()
        {
            var client = new MongoClient(Settings.Default.ConnectionStr);
            var server = client.GetServer();
            MongoDatabase = server.GetDatabase(Settings.Default.DbName);
            UsersCollection = MongoDatabase.GetCollection<User>("users");
            ApikeyCollection = MongoDatabase.GetCollection<Apikey>("apikeys");
        }

        // GET api/users
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/users/5
        public JObject Get(string id)
        {
            var userCursor = UsersCollection.FindOneByIdAs<User>(ObjectId.Parse(id));

            JObject user = new JObject();
            user["user"] = JObject.FromObject(userCursor);
            return user;
        }

        // POST api/users
        public void Post(JObject postObject)
        {
            var user = new BsonDocument {
                {"login", postObject["login"].ToString()},
                {"password", postObject["password"].ToString()},
                {"name", postObject["name"].ToString()},
                {"updated_at", BsonDateTime.Create(DateTime.Now)},
                {"created_at", DateTime.Now.ToString()}
            };
            UsersCollection.Insert(user);

            var apikey = new BsonDocument {
                {"user", user.GetValue("_id") },
                {"token", Guid.NewGuid().ToString()},
                {"token_type", "bearer"}
            };
            ApikeyCollection.Insert(apikey);

            var query = Query.EQ("_id", user.GetValue("_id"));
            var update = Update
                .Set("apikey", apikey.GetValue("_id"));
            UsersCollection.Update(query, update);

        }

        // PUT api/users/5
        public JObject Put(string id, JObject userObject)
        {
            var user = userObject["user"];
            User userInstance = user.ToObject<User>();

            //Update just that field
            var query = Query.EQ("_id", ObjectId.Parse(id));
            var update = Update
                .Set("name", userInstance.name)
                .Set("updated_at", userInstance.updated_at)
                .Set("posts", BsonArray.Create(userInstance.posts));
            UsersCollection.Update(query, update);
            return userObject;
        }

        // DELETE api/users/5
        public void Delete(int id)
        {
        }
    }
}
