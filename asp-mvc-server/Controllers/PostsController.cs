using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using asp_mvc_server.Properties;
using asp_mvc_server.Models;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;


namespace asp_mvc_server.Controllers
{

    public class PostsController : ApiController
    {

        public MongoDatabase MongoDatabase;
        public MongoCollection PostsCollection;

        public PostsController()
        {
            var client = new MongoClient(Settings.Default.ConnectionStr);
            var server = client.GetServer();
            MongoDatabase = server.GetDatabase(Settings.Default.DbName);
            PostsCollection = MongoDatabase.GetCollection<Post>("posts");
        }
        // GET api/posts

        public JObject Get()
        {
            List<Post> posts = PostsCollection.FindAllAs<Post>().ToList<Post>();
            JObject j_posts = new JObject();
            j_posts["posts"] = JArray.FromObject(posts);

            return j_posts;

        }

        // GET api/posts/5
        public JObject Get(string id)
        {
            var postCursor = PostsCollection.FindOneByIdAs<Post>(ObjectId.Parse(id));

            JObject post = new JObject();
            post["post"] = JObject.FromObject(postCursor);
            return post;
        }

        // POST api/posts
        public JObject Post(JObject postObject)
        {
            var post = postObject["post"];
            Post postInstance = post.ToObject<Post>();
            var new_post = new BsonDocument {
                {"title", postInstance.title},
                {"description", postInstance.description},
                {"created_at", postInstance.created_at},
                {"updated_at", postInstance.updated_at},
                {"user", ObjectId.Parse(postInstance.user)}
            };
            PostsCollection.Insert(new_post);

            JObject res = new JObject();

            post["_id"] = new_post.GetValue("_id").ToString();
            res["post"] = post;

            return res;
        }

        // PUT api/posts/5
        public JObject Put(string id, JObject postObject)
        {
            var post = postObject["post"];
            Post postInstance = post.ToObject<Post>();

            //Update just that field
            var query = Query.EQ("_id", ObjectId.Parse(id));
            var update = Update
                .Set("title", postInstance.title)
                .Set("description", postInstance.description)
                .Set("created_at", postInstance.created_at)
                .Set("updated_at", postInstance.updated_at)
                .Set("user", ObjectId.Parse(postInstance.user));
            PostsCollection.Update(query, update);
            return postObject;
        }

        // DELETE api/posts/5
        public JObject Delete(string id)
        {
            var query = Query.EQ("_id", ObjectId.Parse(id));
            PostsCollection.Remove(query);

            return new JObject();
        }
    }
}
