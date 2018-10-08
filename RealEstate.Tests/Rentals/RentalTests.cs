using MongoDB.Bson;
using NUnit.Framework;
using RealEstate.Rentals;
using Shouldly;

namespace RealEstate.Tests.Rentals
{
    [TestFixture]
    public class RentalTests
    {
        [Test]
        public void ToDocument_RentalWithPrice_PriceRepresentedAsDouble()
        {
            var rental = new Rental();
            rental.Price = 1;

            var document = rental.ToBsonDocument();

            document["Price"].BsonType.ShouldBe(BsonType.Double);
        }

        [Test]
        public void ToDocument_RentalWithId_IdIsRepresentedAsAnObjectId()
        {
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();

            var document = rental.ToBsonDocument();

            document["_id"].BsonType.ShouldBe(BsonType.ObjectId);
        }
    }
}
