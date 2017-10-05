using Instagram.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Cloud.Vision.V1;
using System.Text;
using Instagram.Site.Helpers;

namespace Instagram.Site.Controllers
{
    public class ImageController : Controller
    {
        private IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepo)
        {
            imageRepository = imageRepo;
        }

        public ActionResult Index()
        {
            var result = imageRepository.GetAllImages();
            return View(result);
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            bool isSavedSuccessfully = true;

            string fName = "";
            string path = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    if (file != null && file.ContentLength > 0)
                    {

                        var Directory = new DirectoryInfo(Server.MapPath(@"\Content\Images"));

                        path = System.IO.Path.Combine(Directory.ToString(), file.FileName);

                        string shortPath = $@"~/Content/Images/{file.FileName}";


                        string credential_path = Server.MapPath(@"~\Content\Instagram like solovey-0a7b4691e478.json");
                        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

                        var annotation = Annotator.AnotateImage(file);

                        var image = Annotator.DrawFaces(file, annotation.FaceAnnotations);

                        image.Save(path);

                        imageRepository.InsertOrUpdate(new Domain.Entities.Image { Path = shortPath, Annotation = String.Join(", ", annotation.LabelAnnotations.Select(x => $"{x.Description}({Math.Round(x.Score, 2) * 100}%)").ToArray()) });
                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                System.IO.File.Delete(path);
                return Json(new { Message = "Error in saving file" });
            }
        }
        [HttpDelete]
        public bool DeleteImage(int imageId)
        {
            try
            {
                var targetImage = imageRepository.GetImageById(imageId);

                string physicalFolder = Server.MapPath(targetImage.Path);

                if (System.IO.File.Exists(physicalFolder))
                {
                    System.IO.File.Delete(physicalFolder);
                }

                imageRepository.DeleteImage(imageId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}