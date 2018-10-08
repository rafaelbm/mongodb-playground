using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using System;

namespace RealEstate.Tests
{
    [TestFixture]
    public class BsonDocumentTests
    {
        public BsonDocumentTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [Test]
        public void EmptyDocument()
        {
            var document = new BsonDocument();
            Console.WriteLine(document);
        }

        [Test]
        public void AddElements()
        {
            var person = new BsonDocument()
            {
                { "age", new BsonInt32(52) },
                { "isAlive", true }
            };
            person.Add("firstName", new BsonString("bob"));
            Console.WriteLine(person);
        }

        [Test]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("address", new BsonArray(new[] { "101 Some Road", "Unit 501" }));
            Console.WriteLine(person);
        }

        [Test]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument
            {
                {
                    "contact", new BsonDocument
                    {
                        { "phone", "123-456-789" },
                        { "email", "whatever@email.com" }
                    }
                }
            };

            Console.WriteLine(person);
        }

        [Test]
        public void BsonValueConvertions()
        {
            var person = new BsonDocument
            {
                { "age", 44 }
            };

            Console.WriteLine(person["age"].ToDouble() + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);
        }

        [Test]
        public void ToBson()
        {
            var person = new BsonDocument
            {
                { "FirstName", "bob" }
            };

            var bson = person.ToBson();

            Console.WriteLine(BitConverter.ToString(bson));

            var deserializedPerson = BsonSerializer.Deserialize<BsonDocument>(bson);
            Console.WriteLine(deserializedPerson);
        }
    }
}