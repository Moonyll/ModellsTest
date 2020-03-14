using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        public SelectList Selection()
        {
            SelectList selectionList = new SelectList(db.category, "categoryId", "categoryName");

            return ViewBag.categoryId = selectionList;
        }

        #region PICTURE COLLECTION

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
            #region Get pictures list

            // Picture list :
            var pictureList = db.picture.Include(p => p.category);

            #endregion

            #region Generate pagination

            // Total number of pictures :
            var numberOfTotalPics = pictureList.Count();

            // Display 8 pictures per page :
            int numberOfPicsToDisplayPerPage = 8;

            // Number of pages :
            var decimalNumberOfPages = (decimal)numberOfTotalPics / numberOfPicsToDisplayPerPage;

            // Nombre de pages arrondi au plafond supérieur :
            var numberOfPages = Math.Ceiling(decimalNumberOfPages);

            // Element to display according to the page number :
            var elementToDisplay = (pageToDisplay == null) ? 0 : (int)pageToDisplay;

            // Total element :
            var pictures = pictureList.ToList()
                                      .Skip((elementToDisplay - 1) * numberOfPicsToDisplayPerPage)
                                      .Take(numberOfPicsToDisplayPerPage);

            // Total element per page :
            var numberOfPicturesCurrentPage = pictures.Count();

            // Index limit :
            var indexLimit = numberOfPicturesCurrentPage - 1;

            #endregion

            #region Generate picture table

            // Table of pictures to display :
            var pictureElements = new string[8, 6];

            for (int index = 0; index <= 7; index++)
            {
                var pictureUrlToLoad = (index <= indexLimit) ?
                                       pictures.ElementAt(index)
                                               .pictureStandardUrl :
                                       null;

                // Load the picture of database at index :

                if (pictureUrlToLoad != null)
                {
                    // Picture Url :
                    pictureElements[index, 0] = pictureControls.pictureFileDirectory +
                                                pictures.ElementAt(index)
                                                        .pictureStandardUrl
                                                        .ToString();
                    // Picture Title :
                    pictureElements[index, 1] = pictures.ElementAt(index)
                                                        .pictureTitle
                                                        .ToString();
                    // Picture Alternative title :
                    pictureElements[index, 2] = pictures.ElementAt(index)
                                                        .pictureAlternateTitle
                                                        .ToString();
                    // Picture Description :
                    pictureElements[index, 3] = pictures.ElementAt(index)
                                                        .pictureDescription
                                                        .ToString();
                    // Picture Category :
                    pictureElements[index, 4] = pictures.ElementAt(index)
                                                        .category
                                                        .categoryName
                                                        .ToString();
                    // Picture Id :
                    pictureElements[index, 5] = pictures.ElementAt(index)
                                                        .pictureId
                                                        .ToString();
                }

                else
                {
                    // Load the default picture if database ones not exists :
                    pictureElements[index, 0] = "/Content/Images/Pictures/smile.jpg";
                    pictureElements[index, 1] = "Default picture main title";
                    pictureElements[index, 2] = "Default picture alt title";
                    pictureElements[index, 3] = "Default picture description";
                    pictureElements[index, 4] = "Default picture category";
                    pictureElements[index, 5] = "Default picture id";
                }
            }

            #endregion

            #region Passing data to view

            // Set viewbag & viewdata :

            ViewData["pictureElements"] = pictureElements;

            ViewBag.totalPages = numberOfPages;

            #endregion

            return View();
        }

        #endregion

        #region PICTURE CREATION

        /// <summary>
        /// Http Get - Picture creation.
        /// </summary>
        /// <returns></returns>
        public ActionResult pictureCreate()
        {
            Selection();

            return View();
        }

        /// <summary>
        /// Http Post - Create a new picture - Save the source in the directory & properties in the database.
        /// </summary>
        /// <param name="newPicture"></param>
        /// <param name="newPictureToUpload"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pictureCreate
        (
        [Bind
            (Include =
                      "pictureId," +
                      "pictureTitle," +
                      "pictureAlternateTitle," +
                      "pictureDescription," +
                      "pictureStandardUrl," +
                      "categoryId"
            )
        ]
        picture newPicture,
        HttpPostedFileBase newPictureToUpload
        )
        {
            #region Generate a dropdownlist

            Selection();

            #endregion

            #region Save picture in the directory

            // Get size in octets of uploaded picture :
            var uploadedPictureLength = newPictureToUpload?.ContentLength;

            // Check if the size is correct with the limit:
            var isTooHigh = (uploadedPictureLength >= pictureControls.pictureFileToUploadMaxSize) || false;

            // Add error message if size is over limitations :
            if (isTooHigh)
            {
                ModelState.AddModelError
                (
                    "newPictureToUpload",
                    pictureControls.errorMessageForPictureOutOfSize
                );
            }

            // Get extension of uploaded picture :
            var uploadedPictureExtension = newPictureToUpload?.ContentType;

            // Check if the extension is authorized :
            var isOutExt = (!pictureControls.pictureFileToUploadExtension
                                            .Contains(uploadedPictureExtension)) || false;

            // Add error message if extension is not enable :
            if (isOutExt)
            {
                ModelState.AddModelError
                (
                    "newPictureToUpload",
                    pictureControls.errorMessageForPictureOutOfExt
                );
            }

            // Save uploaded picture if all limitations are ok :
            if (newPictureToUpload != null && !isTooHigh && !isOutExt)
            {
                // Picture name - Get input value by user or default filename :
                var newPictureSourceName = (!string.IsNullOrEmpty(newPicture?.pictureStandardUrl)) ?
                                           newPicture?.pictureStandardUrl :
                                           Path.GetFileName(newPictureToUpload.FileName);

                // Combine directory path & picture name to make the source picture :
                var newPictureSourcePath = Path.Combine
                                           (
                                                Server.MapPath(pictureControls.pictureFileDirectory),
                                                newPictureSourceName
                                           );

                // Picture Source is uploaded and saved in the directory :
                newPictureToUpload.SaveAs(newPictureSourcePath);

                // Set relative picture path :
                var relativePicturePath = newPictureSourcePath
                                          .Replace
                                          (
                                            Server.MapPath("~/"),
                                            "/"
                                          )
                                          .Replace(@"\", "/");

                // Set a viewbag to keep & display preview picture when submit :
                ViewBag.picturePreviewSrc = (!ModelState.IsValid) ?
                                            relativePicturePath :
                                            pictureGlobalLabels.DefaultPictureUrl;
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

        #endregion

        #region PICTURE EDITION

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
                Selection();

                return View(picture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pictureEdit
        (
        [Bind
            (Include =
                      "pictureId," +
                      "pictureTitle," +
                      "pictureAlternateTitle," +
                      "pictureDescription," +
                      "pictureStandardUrl," +
                      "pictureRatingValue," +
                      "pictureViewsNumber," +
                      "categoryId"
            )
        ]
        picture updatePicture
        )
        {
            if (ModelState.IsValid)
            {
                db.Entry(updatePicture).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("pictureCollection");
            }

            Selection();

            return View(updatePicture);
        }

        #endregion

        #region PICTURE REMOVING

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

        #endregion

        #region PICTURE VIEWS

        // Display home view :
        public ActionResult pictureHome()
        {
            return View();
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

        #endregion

        #region PROTECTION

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #endregion

        #region EXIFS Management

        /// <summary>
        /// Get Exifs Informations.
        /// </summary>
        /// <param name="pictureFile"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetExifs(string pictureFile)
        {
            pictureExifMetaData pictureExifs = new pictureExifMetaData();

            // Retrieve picture file path :
            var pathPictureFile = Server.MapPath(pictureFile);

            // Check if file path exists :
            if (System.IO.File.Exists(pathPictureFile))
            {

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
                var datetimeString = GetExifData(subIfdDirectory, ExifDirectoryBase.TagDateTimeOriginal);

                var picutreDateTimeValues = (!string.IsNullOrEmpty(datetimeString)) ? GetDateTimeValues(datetimeString)
                                                                                    : new string[]
                                                                                      {
                                                                                            pictureExifMetaData.EmptyValue,
                                                                                            pictureExifMetaData.EmptyValue
                                                                                      };

                pictureExifs.pictureOriginalDateTime = pictureExifMetaData.SpaceTabulation + picutreDateTimeValues[0] + pictureExifMetaData.SpaceTabulation + picutreDateTimeValues[1];

                // Get aperture value :
                pictureExifs.pictureApertureValue = GetExifData(subIfdDirectory, ExifDirectoryBase.TagAperture);

                // Get exposure time :
                pictureExifs.pictureExposureTime = GetExifData(subIfdDirectory, ExifDirectoryBase.TagExposureTime);

                // Get iso speed ratings :

                var isoSpeedValues = GetExifData(subIfdDirectory, ExifDirectoryBase.TagIsoEquivalent);
                
                pictureExifs.pictureIsoSpeedRatings = (isoSpeedValues != pictureExifMetaData.TabEmpty) ? isoSpeedValues + pictureExifMetaData.ISO
                                                                                                       : isoSpeedValues;
                // Get picture flash :
                pictureExifs.pictureFlash = GetExifData(subIfdDirectory, ExifDirectoryBase.TagFlash);

                // Get focal length :
                pictureExifs.pictureFocalLength = GetExifData(subIfdDirectory, ExifDirectoryBase.TagFocalLength);

                // Get picture width :
                pictureExifs.pictureWidth = DisplayPictureDimension(GetExifData(subIfdDirectory, ExifDirectoryBase.TagExifImageWidth));

                // Get picture height :
                pictureExifs.pictureHeight = DisplayPictureDimension(GetExifData(subIfdDirectory, ExifDirectoryBase.TagExifImageHeight));

                // Get picture dimensions :
                pictureExifs.pictureDimensions = (pictureExifs.pictureWidth != pictureExifMetaData.TabEmpty
                                                 && pictureExifs.pictureHeight != pictureExifMetaData.TabEmpty) ?
                                                    (
                                                        pictureExifMetaData.SpaceTabulation + pictureExifs.pictureWidth +
                                                        "   x   " + pictureExifs.pictureHeight + pictureExifMetaData.Pixels
                                                    )
                                                    : pictureExifMetaData.TabEmpty;
                // Get picture file size :
                pictureExifs.pictureFileSize = DisplayPictureSize((GetExifData(subMetadataDirectory, FileMetadataDirectory.TagFileSize)));

                return Json(pictureExifs, JsonRequestBehavior.AllowGet);

            }
                return Json(pictureExifs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Extract exif data from picture directories.
        /// </summary>
        /// <param name="pictureDirectory"></param>
        /// <param name="pictureExiftag"></param>
        /// <returns></returns>
        public string GetExifData(MetadataExtractor.Directory pictureDirectory, int pictureExiftag)
        {
            var exifData = pictureDirectory?.GetDescription(pictureExiftag);

            var pictureExifData = (!string.IsNullOrEmpty(exifData)) ? (pictureExifMetaData.SpaceTabulation + exifData)
                                                                    : (pictureExifMetaData.TabEmpty);

            return pictureExifData;
        }

        /// <summary>
        /// Convert bytes to octets for a friendly picture file size display.
        /// </summary>
        /// <param name="exifDataTagFileSize"></param>
        /// <returns></returns>
        public string DisplayPictureSize(string exifDataTagFileSize)
        {
            // Extract bytes numbers of picture file size :
            var extractBytesNumbers = string.Join("", exifDataTagFileSize.ToCharArray().Where(Char.IsDigit));

            // Convert bytes to Ko :
            var convertNumbersToKo = Math.Round(Convert.ToDecimal(extractBytesNumbers) / 1000);

            // Display picture file size in Ko :
            var displayPictureFileSize = pictureExifMetaData.SpaceTabulation + convertNumbersToKo + pictureExifMetaData.KiloOctets;

            // Diplay picture file size in Mo : 
            if (convertNumbersToKo >= 1000)
            {
                var convertNumbersToMo = Math.Round(convertNumbersToKo / 1000);
                return displayPictureFileSize = pictureExifMetaData.SpaceTabulation + convertNumbersToMo + pictureExifMetaData.MegaOctets;
            }

            return displayPictureFileSize;
        }

        /// <summary>
        ///  Display dimension value of picture width & height.
        /// </summary>
        /// <param name="exifDataTagDimension"></param>
        /// <returns></returns>
        public string DisplayPictureDimension(string exifDataTagDimension)
        {

            if (exifDataTagDimension != pictureExifMetaData.TabEmpty)
            {
                // Extract pixels dimension numbers of picture width or height :
                var displayDimensionNumbers = string.Join("", exifDataTagDimension.ToCharArray().Where(Char.IsDigit));

                return displayDimensionNumbers;
            }

            return exifDataTagDimension;
        }

        public string TestDateTimeString(Regex patternFormat, string datetimeString)

        {
            if (string.IsNullOrEmpty(datetimeString))
            {
                return null;
            }

            var stringFromRegexMatching = patternFormat.Match(datetimeString).ToString();

            return stringFromRegexMatching;
        }

        public string[] GetDateTimeValues(string datetimeString)
        {
            string[] datetimeArrayValues = new string[2] { pictureExifMetaData.EmptyValue, pictureExifMetaData.EmptyValue };

            Dictionary<string, string> datetimeDico = new Dictionary<string, string>()
            {
                {"testDateFormatA", TestDateTimeString(pictureControls.PatternOrigDtFA, datetimeString)},
                {"testDateFormatB", TestDateTimeString(pictureControls.PatternOrigDtFB, datetimeString)},
                {"testDateFormatC", TestDateTimeString(pictureControls.PatternOrigDtFC, datetimeString)},
                {"testDateFormatD", TestDateTimeString(pictureControls.PatternOrigDtFD, datetimeString)},
                {"testTimeFormatA", TestDateTimeString(pictureControls.PatternOrigTmFA, datetimeString)}
            };

            // Get date & time expressions values from dico :
            var datetimeValues = datetimeDico.Values.Where(value => value.Length > 0).OrderByDescending(x => x.Length).ToList();

            if (datetimeValues.Count > 0)
            {

                // 1. Manage date value ;
                var dateValue = datetimeValues.ElementAt(0);

                if (dateValue.Contains(":"))
                {
                    dateValue = dateValue.Replace(":", "/");
                }
                if (dateValue.Contains("-"))
                {
                    dateValue = dateValue.Replace("-", "/");
                }

                DateTime dateValueFormated;

                bool isDateValueBeenFormated = DateTime.TryParse(dateValue, out dateValueFormated);

                var finalDateValue = (isDateValueBeenFormated) ? dateValueFormated.ToString("dd/MM/yyyy")
                                                               : dateValue.ToString();
                // 2. Manage time value :

                var timeValue = datetimeValues.ElementAt(1);

                DateTime timeValueFormated;

                bool isTimeValueBeenFormated = DateTime.TryParse(timeValue, out timeValueFormated);

                var finalTimeValue = (isTimeValueBeenFormated) ? timeValueFormated.ToString("hh:mm:ss")
                                                               : timeValue.ToString();
                // Fill the array :
                datetimeArrayValues[0] = finalDateValue;
                datetimeArrayValues[1] = finalTimeValue;
            }

            return datetimeArrayValues;
        }

        #endregion
    }
}