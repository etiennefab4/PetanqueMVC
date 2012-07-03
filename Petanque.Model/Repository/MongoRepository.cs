using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace Petanque.Model.Repository
{
    public class MongoRepository<T> where T : AbstractMongoEntity
    {
        private readonly MongoDatabase _db;

        public MongoRepository(MongoDatabase db)
        {
            _db = db;
        }

        public virtual IQueryable<T> QueryAll()
        {
            return  MongoCollection.AsQueryable();
        }

        private MongoCollection<T> MongoCollection
        {
            get { return _db.GetCollection<T>(typeof(T).FullName); }
        }

       

        public virtual void Save(T entity)
        {
            MongoCollection.Save(entity);
        }

        public T Find(ObjectId objectId)
        {
            return QueryAll().FirstOrDefault(x => x.ObjectId == objectId);
        }

        public T Find(string id)
        {
            return QueryAll().FirstOrDefault(x => x.Id == id);
        }

        public void Delete(string id)
        {
            var objectId = ObjectId.Parse(id); 
             var query = Query.EQ("_id", objectId); 
            MongoCollection.Remove(query); 
        }
    }
}
