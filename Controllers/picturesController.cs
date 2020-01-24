﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Index2(int? pageToDisplay)
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
            #region Dropdownlist
            // Display a dropdownlist for the picture categories in the view :
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", newPicture.categoryId);
            #endregion

            #region Directory Save
            // Add & save picture file to directory :
            if (newPictureToUpload != null)
            {
                // Picture name :
                string newPictureSourceName = Path.GetFileName(newPictureToUpload.FileName);

                // Combine directory path & picture name to make the source picture :
                string newPictureSourcePath = Path.Combine(Server.MapPath("~/Content/Images/Pictures/"), newPictureSourceName);

                // Picture Source is uploaded and saved in the directory :
                newPictureToUpload.SaveAs(newPictureSourcePath);
            }
            #endregion

            #region Database Save
            // Add & Save picture entity to database :
            if (ModelState.IsValid)
            {
                db.picture.Add(newPicture);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pictureId,pictureTitle,pictureAlternateTitle,pictureDescription,pictureStandardUrl,pictureRatingValue,pictureViewsNumber,categoryId")] picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.category, "categoryId", "categoryName", picture.categoryId);
            return View(picture);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
