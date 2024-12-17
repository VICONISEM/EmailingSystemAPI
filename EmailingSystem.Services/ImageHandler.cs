using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Services
{
    public static class ImageHandler
    {
        public static async Task<string> SaveImage(string ImageKey,string ImageFolder,IFormFile Image)
        {
            string ImageName = await ImageNameConstructor(ImageKey);
            string ImagePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",ImageFolder,ImageName);

            using (var Fs=new FileStream(ImagePath,FileMode.Create))
            {
                await Image.CopyToAsync(Fs);
            }
            return Path.Combine(ImageFolder, ImageName);
        }

        public static async Task<bool> DeleteImage(string ImagePath)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImagePath);
            if (File.Exists(imagePath))
            {

                try
                {
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(imagePath),$"{Guid.NewGuid()}_{Path.GetFileName(imagePath)}");
                    File.Move(imagePath, tempFilePath);

                    File.Delete(tempFilePath);
                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                    return false;
                }

               
            }
            else { return false; }
        }
        private static async Task<string> ImageNameConstructor(string ImageKey)
        => $"{ImageKey}-{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid()}";
    }
}
