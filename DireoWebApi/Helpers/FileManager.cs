﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Helpers
{
    public static class FileManager
    {
        public static string Upload(string FileRaw,string Filename)
        {

            string data = "";
            try
            {
                data = FileRaw.Substring(0, 5);
            }
            catch (Exception)
            {

            }
            string ext = "";

            /*Define mimetype*/
            switch (data.ToUpper())
            {
                case "IVBOR":
                    ext = "png";
                    break;
                case "/9J/4":
                    ext = "jpg";
                    break;
                case "AAABA":
                    ext = "ico";
                    break;
                default:
                    return null;
            }

            var imageDataByteArray = Convert.FromBase64String(FileRaw);

            var imageDataStream = new MemoryStream(imageDataByteArray)
            {
                Position = 0
            };

            int i = Filename.LastIndexOf('.');
            Filename = Filename.Substring(0, i);
            string newfilename =Filename + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + ext;

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/uploads", newfilename);

            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

            imageDataStream.WriteTo(file);

            return newfilename;
        }

        public static bool Delete(string FileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public static void FileSave(string filename, IFormFile file)
        {
            var stream = file.OpenReadStream();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);

            FileStream filestream = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write);
            stream.CopyTo(filestream);
        }
    }
}
