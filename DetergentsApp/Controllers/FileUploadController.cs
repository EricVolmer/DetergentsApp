using System;
using System.Collections.Generic;
using System.Data.Entity;
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


        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, string language)
        {
            var productID = (int) TempData["productID"];
            var sheetTypeID = (int) TempData["sheetTypeID"];

            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                        db.UserFiles.Add(new UserFile
                        {
                            fileName = fileName,
                            productID = productID,
                            sheetTypeID = sheetTypeID,
                            languageType = language,
                            adminApproved = true
                        });
                        db.SaveChanges();
                        file.SaveAs(physicalPath);
                    }
                }

                // Return an empty string to signify success
                return View("UploadedFiles");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VendorFileUpdate([DataSourceRequest] DataSourceRequest request,
            UserFileViewModel userFile)
        {
            if (ModelState.IsValid)
            {
                var sheetType = db.SheetTypes.Find(userFile.sheetTypeID);
                var sheetTypes = db.SheetTypes.ToList();

                
                    var entity = db.UserFiles.Find(userFile.Id);
                    if (entity != null)
                    {
                        if (entity.oldFile == false && db.Products.Find(entity.productID).Equals(userFile.productID))
                        {
                            entity.oldFile = true;
                        }
                        else
                        {
                            entity.fileID = userFile.Id;
                            entity.fileName = userFile.Name;
                            entity.productID = userFile.productID;
                            entity.vendorID = userFile.vendorID;
                            entity.SheetType = sheetType;
                            entity.adminApproved = userFile.adminApproved;
                        }
                        
                        
                        try
                        {
                            //    var existingProduct = db.Products.Find(product.productID);
                            db.Entry(entity).State = EntityState.Modified;
                            db.SaveChanges();
                            return Json(new[] {userFile}.ToDataSourceResult(request, ModelState));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                
            }

            return Json(new[] {userFile}.ToDataSourceResult(request, ModelState));
        }
        
        public ActionResult FilesRead([DataSourceRequest] DataSourceRequest request, int productID, int sheetTypeID)
        {
            var db = new DetergentsEntities();
            try
            {
                if (sheetTypeID == 0)
                {
                    var userFiles = db.UserFiles.Where(x => x.productID == productID && x.adminApproved == true).Select(
                        f => new UserFileViewModel
                        {
                            Id = f.fileID,
                            Name = f.fileName,
                            productID = f.productID,
                            sheetTypeID = f.sheetTypeID,
                        });
                    return Json(userFiles.ToDataSourceResult(request));
                }
                else
                {
                    var userFiles = db.UserFiles.Where(x => x.productID == productID && x.sheetTypeID == sheetTypeID && x.adminApproved == true )
                        .Select(
                            f => new UserFileViewModel
                            {
                                Id = f.fileID,
                                Name = f.fileName,
                                productID = f.productID,
                                sheetTypeID = f.sheetTypeID,
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

        public ActionResult FilesReadAdmin([DataSourceRequest] DataSourceRequest request)
        {
            var db = new DetergentsEntities();
            try
            {
                
                var userFiles = db.UserFiles.Where(x => x.adminApproved == false)
                    .Select(
                        f => new UserFileViewModel
                        {
                            Id = f.fileID,
                            Name = f.fileName,
                            productID = f.productID,
                            sheetTypeName = f.SheetType.sheetTypeName,
                            sheetTypeID = f.sheetTypeID,
                            vendorID = f.vendorID,
                            adminApproved = f.adminApproved,
                            oldFile = f.oldFile
                        });
                return Json(userFiles.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ActionResult FilesReadAll([DataSourceRequest] DataSourceRequest request)
        {
            var db = new DetergentsEntities();
            try
            {
                
                var userFiles = db.UserFiles.Where(x => x.adminApproved == x.adminApproved)
                    .Select(
                        f => new UserFileViewModel
                        {
                            Id = f.fileID,
                            Name = f.fileName,
                            productID = f.productID,
                            sheetTypeName = f.SheetType.sheetTypeName,
                            sheetTypeID = f.sheetTypeID,
                            adminApproved = f.adminApproved
                        });
                return Json(userFiles.ToDataSourceResult(request));
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
        public ActionResult FilesDestroy([DataSourceRequest] DataSourceRequest request, UserFileViewModel file)
        {
            try
            {
                if (file != null)
                {
                    db.UserFiles.Remove(db.UserFiles.FirstOrDefault(f => f.productID == file.productID));

                    db.SaveChanges();
                }

                return Json(new[] {file}.ToDataSourceResult(request, ModelState));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult FilesDestroyAdmin([DataSourceRequest] DataSourceRequest request, UserFileViewModel file)
        {
            try
            {
                if (file != null)
                {
                    db.UserFiles.Remove(db.UserFiles.FirstOrDefault(f => f.vendorID == file.vendorID));

                    db.SaveChanges();
                }

                return Json(new[] {file}.ToDataSourceResult(request, ModelState));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}