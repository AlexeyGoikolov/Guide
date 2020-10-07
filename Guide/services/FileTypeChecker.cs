using System.Collections.Generic;
using System.IO;
using FileTypeInterrogator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Guide.Services
{
    public class FileTypeChecker
    {
        private static List<string> validVideoMimeTypes = new List<string> { "video/mp4", "video/quicktime", "video/x-msvideo", "video/mpeg" };
        private static List<string> validAudioMimeTypes = new List<string> { "audio/ogg", "audio/AMR", "audio/mpeg"};
        private static List<string> validImageMimeTypes = new List<string> { "image/png", "image/jpeg", "image/jpg", "image/bmp" };
        private static List<string> validDocumentMimeTypes = new List<string> { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel", "text/plain", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-excel", "application/msword", "application/x-7z-compressed", "application/zip", "application/vnd.rar" };

        public static bool IsValidVideo(IFormFile file)
        {
            return validVideoMimeTypes.Contains(GetFileMimeType(file));
        }
        public static bool IsValidAudio(IFormFile file)
        {
            return validAudioMimeTypes.Contains(GetFileMimeType(file));
        }

        public static bool IsValidImage(IFormFile file)
        {
            return validImageMimeTypes.Contains(GetFileMimeType(file));
        }
        
        public static bool IsValidDocument(IFormFile file)
        {
            return validDocumentMimeTypes.Contains(GetFileMimeType(file));
        }

        private static string GetFileMimeType(IFormFile file)
        {
            // You should have checked for null and file length before reaching here

            IFileTypeInterrogator interrogator = new FileTypeInterrogator.FileTypeInterrogator();

            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                fileBytes = stream.ToArray();
            }

            FileTypeInfo fileTypeInfo = interrogator.DetectType(fileBytes);
            return fileTypeInfo.MimeType.ToLower();
        }
    }
}