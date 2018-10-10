using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RealEstate.Properties;
using RealEstate.Rentals;

namespace RealEstate.App_Start
{
    public class RealEstateContext
    {
        public IMongoDatabase Database { get; private set; }
        public GridFSBucket ImagesBucket { get; }

        public RealEstateContext()
        {
            var client = new MongoClient(Settings.Default.RealEstateConnectionString);
            Database = client.GetDatabase(Settings.Default.RealEstateDatabaseName);
            ImagesBucket = new GridFSBucket(Database);
        }

        public IMongoCollection<Rental> Rentals
        {
            get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }
    }
}