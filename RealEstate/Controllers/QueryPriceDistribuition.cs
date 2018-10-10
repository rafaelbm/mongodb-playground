﻿using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Rentals;
using System.Collections.Generic;

namespace RealEstate.Controllers
{
    public class QueryPriceDistribuition
    {
        public IEnumerable<BsonDocument> Run(IMongoCollection<Rental> rentals)
        {
            var priceRange = new BsonDocument(
                "$subtract",
                new BsonArray
                {
                    "$Price",
                    new BsonDocument(
                        "$mod",
                        new BsonArray {"$Price", 500})
                });

            var grouping = new BsonDocument(
                "$group",
                new BsonDocument
                {
                    {"_id", priceRange },
                    {"count", new BsonDocument("$sum", 1)}
                });

            var sort = new BsonDocument(
                "$sort",
                new BsonDocument("_id", 1)
                );

            var pipeline = new BsonDocument[] { grouping, sort };
         
            return rentals.Aggregate<BsonDocument>(pipeline).ToList();
        }
    }
}   