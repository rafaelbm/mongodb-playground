using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RealEstate.Properties;
using RealEstate.Rentals;

namespace RealEstate.App_Start
{
    public partial class RealEstateContext
    {
        public IMongoDatabase Database { get; private set; }
        public GridFSBucket ImagesBucket { get; }

        public RealEstateContext()
        {
            var connectionString = Settings.Default.RealEstateConnectionString;
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.ClusterConfigurator = builder =>
            {
                builder.Subscribe(new Log4NetMongoEvents());
            };
            var client = new MongoClient(settings);
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