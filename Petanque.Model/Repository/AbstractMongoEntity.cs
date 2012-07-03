using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Petanque.Model.Repository
{
    public abstract class AbstractMongoEntity
    {
        [BsonId]
        public ObjectId ObjectId;

        public String Id
        {
            get { return ObjectId.ToString(); }
            set { ObjectId = new ObjectId(value); }
        }
    }
}