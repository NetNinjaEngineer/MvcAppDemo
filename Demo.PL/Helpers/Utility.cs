using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class Utility
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // Get located folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            // Get filename and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            // Get file path
            string filePath = Path.Combine(folderPath, fileName);
            // save file as streams
            using var fileStream = new FileStream(filePath, FileMode.Create);
            // Return fileName
            return fileName;
        }
    }
}
