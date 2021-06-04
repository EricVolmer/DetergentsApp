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

            CreateViewListSheetType();
            CreateViewListVendor();
            return View();
        }
        
        public void CreateViewListVendor()
        {
            try
            {
                var result = db.Vendor;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new ProductViewModel
                    {
                        vendorName = entity.vendorName,
                        vendorID = entity.vendorID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                    {
                        Text = productViewModel.vendorName + " - " + productViewModel.vendorID,
                        Value = productViewModel.vendorID.ToString()
                    });
                ViewBag.Vendor = containerList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void CreateViewListSheetType()
        {
            try
            {
                var result = db.SheetTypes;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new ProductViewModel
                    {
                        sheetTypeName = entity.sheetTypeName,
                        sheetTypeID = entity.sheetTypeID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                        {Text = productViewModel.sheetTypeName, Value = productViewModel.sheetTypeID.ToString()});

                ViewBag.SheetType = containerList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                            adminApproved = false
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AdminFileUpdate([DataSourceRequest] DataSourceRequest request,
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

        public ActionResult FilesRead([DataSourceRequest] DataSourceRequest request, int productID, int sheetTypeID,
            string productName)
        {
            var db = new DetergentsEntities();
            try
            {
                var userFiles = db.UserFiles.Where(x => x.productID == productID && x.adminApproved == true).Select(
                    f => new UserFileViewModel
                    {
                        Id = f.fileID,
                        Name = f.fileName,
                        productID = f.productID,
                        sheetTypeID = f.sheetTypeID
                    });
                return Json(userFiles.ToDataSourceResult(request));
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
                var userFiles = db.UserFiles.Select(
                    f => new UserFileViewModel
                    {
                        Id = f.fileID,
                        Name = f.fileName,
                        productID = f.productID,
                        productDescription = f.productID.ToString(),
                        sheetTypeName = f.SheetType.sheetTypeName,
                        sheetTypeID = f.sheetTypeID,
                        vendorID = f.vendorID,
                        adminApproved = f.adminApproved,
                        oldFile = f.oldFile,
                        languageType = f.languageType,
                    });
                return Json(userFiles.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        //
        public ActionResult CategoryRead([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCategory().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private static IQueryable<ProductViewModel> GetCategory()
        {
            var northwind = new DetergentsEntities();
            var products = northwind.Products.Select(product => new ProductViewModel
            {
                categoryID = product.categoryID,
                categoryName = product.Category.categoryName
            });

            return products;
        }
        //

        public ActionResult FilesReadAll([DataSourceRequest] DataSourceRequest request)
        {
            var db = new DetergentsEntities();
            try
            {
                var result = db.UserFiles;
                var fileList = new List<UserFileViewModel>();
                foreach (var item in result)
                {
                    var userFile = new UserFileViewModel()
                    {
                        Id = item.fileID,
                        Name = item.fileName,
                        productID = item.productID,
                        sheetTypeName = item.SheetType.sheetTypeName,
                        sheetTypeID = item.sheetTypeID,
                        adminApproved = item.adminApproved
                    };

                    var fileListName = db.UserFiles
                        .Where(file => file.productID == item.productID).ToList();

                    if (fileListName != null)
                    {
                        userFile.listOfFiles = new List<UserFile>();

                        foreach (var file in fileListName)
                        {
                            var userFiles = new UserFile
                            {
                                fileName = file.fileName,
                                fileID = file.fileID,

                                SheetType = new SheetType
                                {
                                    sheetTypeID = file.SheetType.sheetTypeID,
                                    sheetTypeName = file.SheetType.sheetTypeName
                                }
                            };
                            userFiles.SheetType.sheetTypeName = file.SheetType.sheetTypeName;

                            userFile.listOfFiles.Add(userFiles);
                        }
                    }

                    fileList.Add(userFile);
                }

                return Json(fileList.ToTreeDataSourceResult(request), JsonRequestBehavior.AllowGet);
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