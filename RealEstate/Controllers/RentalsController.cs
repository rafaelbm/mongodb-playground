using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using RealEstate.App_Start;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class RentalsController : Controller
    {
        private readonly RealEstateContext _context = new RealEstateContext();

        public ActionResult Index(RentalsFilter filters)
        {
            List<Rental> rentals = FilterRentals(filters);
            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filters
            };
            return View(model);
        }

        private List<Rental> FilterRentals(RentalsFilter filters)
        {
            IQueryable<Rental> rentals = _context.Rentals.AsQueryable()
                .OrderBy(x => x.Price);

            if (filters.MinimumRooms.HasValue)
            {
                rentals = rentals.Where(x => x.NumberOfRooms >= filters.MinimumRooms);
            }

            if (filters.PriceLimit.HasValue)
            {
                rentals = rentals.Where(x => x.Price <= filters.PriceLimit);
            }

            return rentals.ToList();
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            _context.Rentals.InsertOne(rental);

            return RedirectToAction("Index");
        }

        public ActionResult AdjustPrice(string id)
        {
            Rental rental = GetRental(id);

            return View(rental);
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            _context.Rentals.ReplaceOne(x => x.Id == rental.Id, rental, new UpdateOptions { IsUpsert = true });

            return RedirectToAction("Index");

        }

        public ActionResult Delete(string id)
        {
            _context.Rentals.DeleteOne(x => x.Id == id);
            return RedirectToAction("Index");

        }

        public string PriceDistribution()
        {
            return new QueryPriceDistribuition()
                 .Run(_context.Rentals)
                 .ToJson();
        }

        public ActionResult AttachImage(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = GetRental(id);

            if (rental.HasImage())
            {
                DeleteImage(rental);
            }

            StoreImage(file, rental);

            return RedirectToAction("Index");
        }

        private void DeleteImage(Rental rental)
        {
            _context.ImagesBucket.DeleteAsync(ObjectId.Parse(rental.ImageId));
            SetRentalImageId(rental.Id, null);
        }

        private void StoreImage(HttpPostedFileBase file, Rental rental)
        {
            GridFSUploadOptions options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument("contentType", file.ContentType)
            };

            var imageId = _context.ImagesBucket.UploadFromStream(file.FileName, file.InputStream, options);
            SetRentalImageId(rental.Id, imageId.ToString());
        }

        public ActionResult GetImage(string id)
        {
            try
            {
                var stream = _context.ImagesBucket.OpenDownloadStream(ObjectId.Parse(id));
                var contentType = stream.FileInfo.Metadata["contentType"].AsString;
                return File(stream, contentType);
            }
            catch (GridFSFileNotFoundException)
            {
                return HttpNotFound();
            }
        }

        private void SetRentalImageId(string rentalId, string imageId)
        {
            var setRentalImageId = Builders<Rental>.Update.Set(r => r.ImageId, imageId);
            _context.Rentals.UpdateOne(r => r.Id == rentalId, setRentalImageId);
        }

        private Rental GetRental(string id)
        {
            var filterId = Builders<Rental>.Filter.Eq("_id", ObjectId.Parse(id));
            return _context.Rentals.Find(filterId).FirstOrDefault();
        }
    }
}