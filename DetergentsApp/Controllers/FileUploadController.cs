﻿using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

// ReSharper disable All

namespace DetergentsApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult UploadedFiles(int productID, int sheetTypeID)
        {
            var product = db.Products.Find(productID);
            var userFiles = new List<UserFile>();

            ViewBag.ProductID = productID;
            ViewBag.SheetTypeID = sheetTypeID;

            if (sheetTypeID == 0)
            {
                if (product != null)
                    foreach (var productSheetType in product.SheetType)
                        userFiles.AddRange(productSheetType.UserFiles);
            }
            else
            {
                var sheetType = product?.SheetType.FirstOrDefault(s => s.sheetTypeID == sheetTypeID);
                if (sheetType != null) userFiles.AddRange(sheetType.UserFiles);
            }

            return View();
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            var productID = (int) TempData["productID"];
            var sheetTypeID = (int) TempData["sheetTypeID"];
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                        db.UserFiles.Add(new UserFile
                        {
                            fileName = Path.GetFileName(file.FileName),
                            fileData = GetFilesBytes(file),
                            productID = productID,
                            sheetTypeID = sheetTypeID
                        });
                    db.SaveChanges();
                }

                // Return an empty string to signify success
                return Content("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ActionResult FilesRead([DataSourceRequest] DataSourceRequest request, int productID, int sheetTypeID)
        {
            var db = new DetergentsEntities();
            try
            {
                if (sheetTypeID == 0)
                {
                    var userFiles = db.UserFiles.Where(x => x.productID == productID ).Select(
                        f => new UserFileViewModel
                        {
                            Id = f.fileID,
                            Name = f.fileName,
                            productID = f.productID,
                            sheetTypeID = f.sheetTypeID,
                            DataLength = SqlFunctions.DataLength(f.fileData)
                        });
                    return Json(userFiles.ToDataSourceResult(request));
                }
                else
                {
                    var userFiles = db.UserFiles.Where(x => x.productID == productID && x.sheetTypeID == sheetTypeID).Select(
                        f => new UserFileViewModel
                        {
                            Id = f.fileID,
                            Name = f.fileName,
                            productID = f.productID,
                            sheetTypeID = f.sheetTypeID,
                            DataLength = SqlFunctions.DataLength(f.fileData)
                        });
                    return Json(userFiles.ToDataSourceResult(request));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static byte[] GetFilesBytes(HttpPostedFileBase file)
        {
            var target = new MemoryStream();

            file.InputStream.CopyTo(target);

            return target.ToArray();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FilesDestroy([DataSourceRequest] DataSourceRequest request, ProductViewModel file)
        {
            if (file != null)
            {
                db.Products.Remove(db.Products.FirstOrDefault(f => f.productID == file.productID));

                db.SaveChanges();
            }

            return Json(new[] {file}.ToDataSourceResult(request, ModelState));
        }
    }
}