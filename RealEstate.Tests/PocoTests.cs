using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace RealEstate.Tests
{
    [TestFixture]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        public class Person
        {
            public string FirstName { get; set; }
            public int Age { get; set; }
            public List<string> Address { get; set; } = new List<string>();
            public Contact Contact { get; set; } = new Contact();
            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")]
            public string Old { get; set; }
            [BsonElement]
            private string _encapsulated;
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [Test]
        public void SerializationAttributes()
        {
            var person = new Person();
            Console.WriteLine(person.ToJson());

        }

        [Test]
        public void Automatic()
        {
            var person = new Person
            {
                Age = 55,
                FirstName = "bob"
            };
            person.Address.Add("101 Some Road");
            person.Address.Add("Unit 501");
            person.Contact.Email = "email@email.com";
            person.Contact.Phone = "123-456-789";


            Console.WriteLine(person.ToJson());
        }
    }
}
