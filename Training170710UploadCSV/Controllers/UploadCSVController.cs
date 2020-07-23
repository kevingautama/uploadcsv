using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Training170710UploadCSV.DataContexts;
using Training170710UploadCSV.Models;
using LINQtoCSV;

namespace Training170710UploadCSV.Controllers
{
    public class UploadCSVController : Controller
    {
        // GET: UploadCSV
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadOnlyFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadOnlyFile(HttpPostedFileBase file)
        {
            // Upload File to Folder
            //using (var reader = new BinaryReader(file.InputStream))
            //{
            //    var path = "~/App_Data/";

            //    string _savePath = Path.Combine(Server.MapPath(path), file.FileName);
            //    file.SaveAs(_savePath);
            //}


            // UploadFile to DB
            //var newFile = new FlatFile
            //{
            //    FileName = file.FileName,
            //    ContentType = file.ContentType
            //
            //using (var reader = new BinaryReader(file.InputStream))
            //{
            //    newFile.Content = reader.ReadBytes(file.ContentLength);

            //    using(var context = new DBContext())
            //    {
            //        context.FlatFiles.Add(newFile);
            //        context.SaveChanges();
            //    }
            //}

            var path = "~/App_Data/";
            string _savePath = Path.Combine(Server.MapPath(path), file.FileName);

            using (var reader = new BinaryReader(file.InputStream))
            {
                file.SaveAs(_savePath);
            }

            string fileHeader = "Name,Description";
            char separator = ',';
            string[] lines = System.IO.File.ReadAllLines(_savePath);
            string[] newLines = new string[lines.Length + 1];
            newLines[0] = fileHeader;
            for(int i = 0; i < lines.Length; i++)
            {
                newLines[i + 1] = lines[i];
            }
            System.IO.File.WriteAllLines(_savePath, newLines);

            CsvContext cc = new CsvContext();
            List<ViewModel> list = cc.Read<ViewModel>(_savePath, new CsvFileDescription
            {
                SeparatorChar = separator,
                FirstLineHasColumnNames = true,
                FileCultureName = "en-US", // default is the current culture
                EnforceCsvColumnAttribute = false
            }).ToList();
            
            using(var context= new DBContext())
            {
                context.UploadedDatas.AddRange(list);
                context.SaveChanges();
            }

            return RedirectToAction("UploadOnlyFile");
        }


    }
}