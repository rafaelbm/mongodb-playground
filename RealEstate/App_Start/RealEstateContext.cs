using MongoDB.Driver;
using RealEstate.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.App_Start
{
    public class RealEstateContext
    {
        public IMongoDatabase Database { get; private set; }

        public RealEstateContext()
        {
            var client = new MongoClient(Settings.Default.RealEstateConnectionString);
            Database = client.GetDatabase(Settings.Default.RealEstateDatabaseName);
        }
    }
}