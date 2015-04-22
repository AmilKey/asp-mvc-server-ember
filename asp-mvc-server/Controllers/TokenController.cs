using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Collections.Specialized;
using MongoDB.Driver;
using asp_mvc_server.Properties;
using asp_mvc_server.Models;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace asp_mvc_server.Controllers
{
    public class TokenController : ApiController
    {
        
        public MongoDatabase MongoDatabase;
        public MongoCollection UserCollection;
        public MongoCollection ApikeyCollection;

        public TokenController()
        {
            var client = new MongoClient(Settings.Default.ConnectionStr);
            var server = client.GetServer();
            MongoDatabase = server.GetDatabase(Settings.Default.DbName);
            UserCollection = MongoDatabase.GetCollection<User>("users");
            ApikeyCollection = MongoDatabase.GetCollection<Apikey>("apikeys");
        }

        // GET api/token
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/token/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/token

        public class TokenReq
        {
            public string grant_type { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        public JObject Post(TokenReq tokenReq)
        {
            JObject res = new JObject();
            string username   = tokenReq.username;
            string grant_type = tokenReq.grant_type;
            string password   = tokenReq.password;

            if (grant_type == "password")
            {
                var query = Query.And(
                    Query.EQ("login", username),
                    Query.EQ("password", password)
                );
                var user = UserCollection.FindOneAs<User>(query);
                if (user != null)
                {
                    var apikey_q = Query.EQ("user", ObjectId.Parse(user._id));
                    var apikey = ApikeyCollection.FindOneAs<Apikey>(apikey_q);
                    if (apikey != null)
                    {
                        res["access_token"] = apikey.token;
                        res["user_id"] = apikey.user;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return res;
        }

        // PUT api/token/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/token/5
        public void Delete(int id)
        {
        }
    }
}
