using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using Modells.Models;

namespace Modells.Controllers
{
    public class PicturesController : Controller
    {
        private picturesModells db = new picturesModells();

       /// <summary>
       /// List of pictures.
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        { 
            // Picture list :
            var picture = db.picture.Include(p => p.category);
      
            return View(picture.ToList());
        }

        /// <summary>
        /// Pictures list with pagination.
        /// </summary>
        /// <param name="pageToDisplay"></param>
        /// <returns></returns>
        public ActionResult pictureCollection(int? pageToDisplay)
        {
            #region Get Pictures List
            // Picture list :
            var picture = db.picture.Include(p => p.category);
            #endregion

            #region Generate pagination
            // Total number of pictures :
            var numberOfTotalPics = picture.Count();

            // Display 8 pictures per page :
            int numberOfPicsToDisplayPerPage = 8;

            // Number of pages :
            var n = (decimal)numberOfTotalPics / numberOfPicsToDisplayPerPage;

            // Nombre de pages arrondi au plafond supérieur :
            var numberOfPages = Math.Ceiling(n);

            // Element to display according to the page number :
            var elementToDisplay = (pageToDisplay == null) ? 0 : (int)pageToDisplay;

            // Total element :
            var pictures = picture.ToList().Skip((elementToDisplay - 1) * 8).Take(8);

            // Total element per page :
            var numberOfPicturesCurrentPage = pictures.Count();

            // Viewbags to pass data in the view :
            ViewBag.totalPictures = numberOfPicturesCurrentPage;
            ViewBag.totalPages = numberOfPages;
            #endregion

            return View(pictures);
        }

        // GET: pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            picture picture = db.picture.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: pictures/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName");
            return View();
        }

        // GET: picturesCreate
        public ActionResult pictureCreate()
        {
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName");
            return View();
        }

        /// <summary>
        /// Create a new picture - Save the source in the directory & properties in the database.
        /// </summary>
        /// <param name="newPicture"></param>
        /// <param name="newPictureToUpload"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pictureCreate([Bind(Include = "pictureId,pictureTitle,pictureAlternateTitle,pictureDescription,pictureStandardUrl,categoryId")] picture newPicture, HttpPostedFileBase newPictureToUpload)
        {
            #region Generate a dropdownlist
            // Display a dropdownlist for the picture categories in the view :
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", newPicture.categoryId);
            #endregion

            #region Save picture in the directory

            // Add & save picture file to directory :

            // Get size in octets of uploaded picture :
            var uploadedPictureLength = newPictureToUpload?.ContentLength;
            
            // Check if the size is correct with the limit:
            var isTooHigh = (uploadedPictureLength >= pictureControls.pictureFileToUploadMaxSize) ? true : false;
            
            // Add error message if size is over limitations :
            if (isTooHigh)
            {
                ModelState.AddModelError("newPictureToUpload", pictureControls.errorMessageForPictureOutOfSize);
            }

            // Get extension of uploaded picture :
            var uploadedPictureExtension = newPictureToUpload?.ContentType;

            // Check if the extension is authorized :
            var isOutExt = (!pictureControls.pictureFileToUploadExtension.Contains(uploadedPictureExtension)) ? true : false;

            // Add error message if extension is not enable :
            if (isOutExt)
            {
                ModelState.AddModelError("newPictureToUpload", pictureControls.errorMessageForPictureOutOfExt);
            }

            // Save uploaded picture if all limitations are ok :
            if (newPictureToUpload != null && !isTooHigh && !isOutExt)
            {
                // Picture name - Get input value by user or default filename :
                var newPictureSourceName = (!string.IsNullOrEmpty(newPicture?.pictureStandardUrl)) ?
                                           newPicture.pictureStandardUrl :
                                           Path.GetFileName(newPictureToUpload.FileName);

                // Combine directory path & picture name to make the source picture :
                var newPictureSourcePath = Path.Combine(Server.MapPath("~/Content/Images/Pictures/"), newPictureSourceName);

                // Test Exifs :
                //GetExifs(newPictureSourcePath);

                // Picture Source is uploaded and saved in the directory :
                newPictureToUpload.SaveAs(newPictureSourcePath);
            }
            #endregion

            #region Save picture in the database

            // Add & Save picture entity to database :
            if (ModelState.IsValid)
            {
                db.picture.Add(newPicture);
                db.SaveChanges();
                return RedirectToAction("pictureSuccess");
            }
            #endregion

            return View(newPicture);
        }

        // POST: pictures/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pictureId,pictureTitle,pictureAlternateTitle,pictureDescription,pictureStandardUrl,pictureRatingValue,pictureViewsNumber,categoryId")] picture picture)
        {
            if (ModelState.IsValid)
            {
                db.picture.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", picture.categoryId);
            return View(picture);
        }

        // GET: pictures/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    picture picture = db.picture.Find(id);
        //    if (picture == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", picture.categoryId);
        //    return View(picture);
        //}

        // GET: pictures/Edit/5
        [HttpGet]
        public ActionResult pictureEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            picture picture = db.picture.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", picture.categoryId);
            return View(picture);
        }

        // POST: pictures/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "pictureId,pictureTitle,pictureAlternateTitle,pictureDescription,pictureStandardUrl,pictureRatingValue,pictureViewsNumber,categoryId")] picture picture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(picture).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", picture.categoryId);
        //    return View(picture);
        //}

        // POST: pictures/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pictureEdit([Bind(Include = "pictureId,pictureTitle,pictureAlternateTitle,pictureDescription,pictureStandardUrl,pictureRatingValue,pictureViewsNumber,categoryId")] picture updatePicture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(updatePicture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("pictureCollection");
            }
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", updatePicture.categoryId);
            return View(updatePicture);
        }


        // GET: pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            picture picture = db.picture.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            picture picture = db.picture.Find(id);
            db.picture.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Display success view :
        public ActionResult pictureSuccess()
        {
            return View();
        }

        // Display error view :
        public ActionResult pictureError()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Get Exifs Informations :
        [HttpGet]
        public JsonResult GetExifs(string pictureFile) 
        {
            pictureExifMetaData pictureExifs = new pictureExifMetaData();

            // Retrieve picture file path :
            var pathPictureFile = Server.MapPath(pictureFile);

            // Retrieve directories of picture file properties :
            var pictureDirectories = ImageMetadataReader.ReadMetadata(pathPictureFile).ToList();

            // 1° Read directories metadata files :

            // Read "Exif IFD0" directory file :
            var subIfd0Directory = pictureDirectories.OfType<ExifIfd0Directory>().FirstOrDefault();

            // Read "Exif SubIFD" directory file :
            var subIfdDirectory = pictureDirectories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            // Read "MetadataDirectory" directory file :
            var subMetadataDirectory = pictureDirectories.OfType<FileMetadataDirectory>().FirstOrDefault();

            // 2° Get Exifs data from read file :

            // Get the camera make :
            pictureExifs.pictureCameraMake = GetExifData(subIfd0Directory, ExifDirectoryBase.TagMake);

            // Get the camera model :
            pictureExifs.pictureCameraModel = GetExifData(subIfd0Directory, ExifDirectoryBase.TagModel);

            // Get original date time :
            pictureExifs.pictureOriginalDateTime = GetExifData(subIfdDirectory, ExifDirectoryBase.TagDateTimeOriginal);

            // Get aperture value :
            pictureExifs.pictureApertureValue = GetExifData(subIfdDirectory,ExifDirectoryBase.TagAperture);

            // Get exposure time :
            pictureExifs.pictureExposureTime = GetExifData(subIfdDirectory, ExifDirectoryBase.TagExposureTime);

            // Get iso speed ratings :
            pictureExifs.pictureIsoSpeedRatings = GetExifData(subIfdDirectory,ExifDirectoryBase.TagIsoEquivalent);

            // Get picture flash :
            pictureExifs.pictureFlash = GetExifData(subIfdDirectory,ExifDirectoryBase.TagFlash);

            // Get focal length :
            pictureExifs.pictureFocalLength = GetExifData(subIfdDirectory,ExifDirectoryBase.TagFocalLength);

            // Get picture width :
            pictureExifs.pictureWidth = GetExifData(subIfdDirectory,ExifDirectoryBase.TagExifImageWidth);

            // Get picture height :
            pictureExifs.pictureHeight = GetExifData(subIfdDirectory,ExifDirectoryBase.TagExifImageHeight);

            // Get picture file size :
            pictureExifs.pictureFileSize = GetExifData(subMetadataDirectory,FileMetadataDirectory.TagFileSize);

                return Json(pictureExifs, JsonRequestBehavior.AllowGet);

        }

        // Extract exif data from picture directories :
        public string GetExifData(MetadataExtractor.Directory pictureDirectory, int pictureExiftag)
        {
            var exifData = pictureDirectory?.GetDescription(pictureExiftag);

            var pictureExifData = (!string.IsNullOrEmpty(exifData)) ? (pictureExifMetaData.SpaceTabulation + exifData) : (pictureExifMetaData.SpaceTabulation + pictureExifMetaData.EmptyValue);

                return pictureExifData;
        }

    }
}
