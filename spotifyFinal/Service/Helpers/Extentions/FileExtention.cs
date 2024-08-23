using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace spotifyFinal.Helpers.Extentions
{
    public static class FileExtention
    {
        public static bool CheckFileSize(this IFormFile file, int size)
        {
            return file.Length / 1024 < size;
        }

        public static bool CheckFileFormat(this IFormFile file, string pattern)
        {
            return file.ContentType.Contains(pattern);
        }

        public async static Task SaveFileToLocalAsync(this IFormFile file, string path)
        {
            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);
        }

        public static void DeleteFileFromLocalAsync(string path, string image)
        {
            if (File.Exists(Path.Combine(path, image)))
                File.Delete(Path.Combine(path, image));
        }
        public static async void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static string SaveImage(this IFormFile file, IWebHostEnvironment env, string root, string fileName)
        {
            string fullPath = Path.Combine(env.WebRootPath, root, fileName);

            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
        public static bool IsAudio(this IFormFile file)
        {
            return file.FileName.EndsWith(".mp3");
        }
        public static bool CheckAudioSize(this IFormFile file, int size)
        {
            return file.Length / 1024 > size;
        }
        public static string SaveAudio(this IFormFile file, IWebHostEnvironment env, string root, string fileName)
        {
            string fullPath = Path.Combine(env.WebRootPath, root, fileName);

            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

    }
}
