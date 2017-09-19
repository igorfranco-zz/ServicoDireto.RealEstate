using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.Core.Helpers;
using System.IO;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{

    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(FormCollection collection)
        {
            return View();
        }

        private UploadedFile RetrieveFileFromRequest()
        {
            string filename = null;
            string fileType = null;
            byte[] fileContents = null;

            if (Request.Files.Count > 0)
            { //they're uploading the old way
                var file = Request.Files[0];
                fileContents = new byte[file.ContentLength];
                fileType = file.ContentType;
                filename = file.FileName;
            }
            else if (Request.ContentLength > 0)
            {
                fileContents = new byte[Request.ContentLength];
                Request.InputStream.Read(fileContents, 0, Request.ContentLength);
                filename = Request.Headers["X-File-Name"];
                fileType = Request.Headers["X-File-Type"];
            }

            return new UploadedFile()
            {
                Filename = filename,
                ContentType = fileType,
                FileSize = fileContents != null ? fileContents.Length : 0,
                Contents = fileContents
            };
        }

        public void SaveFile(UploadedFile file)
        {
            string imgBasePath = String.Format("{0}/{1}", SiteContext.UploadPath, "xxxx");
            string newFileName = String.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(file.Filename));
            string imagePath = Server.MapPath(String.Format(@"{0}/{1}", imgBasePath, newFileName));
            string thumbImagePath = Server.MapPath(String.Format(@"{0}/Thumb/{1}", imgBasePath, newFileName));
            IOHelper.CreateFile(file.Contents, imagePath);

            ////TODO:Salvar o thumb já redimensionado
            if (file.ContentType.Contains("image"))
            {
                IOHelper.CreateFile(file.Contents, thumbImagePath);
                ImageHelper.ResizeImage(imagePath, thumbImagePath, 100, 100, true);
            }
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            UploadedFile file = this.RetrieveFileFromRequest();
            SaveFile(file);
            return View("Create");
        }
        
    }
}
