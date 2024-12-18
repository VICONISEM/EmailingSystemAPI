using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Services
{
    public static class FileHandler
    {
        public static async Task<string> SaveFile(string FileKey,string FileFolder,IFormFile File)
        {
            string FileName = await FileNameConstructor(FileKey);
            string FilePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",FileFolder,FileName);

            using (var Fs=new FileStream(FilePath,FileMode.Create))
            {
                await File.CopyToAsync(Fs);
            }
            return Path.Combine(FileFolder, FileName);
        }

        public static async Task<bool> DeleteFile(string filePath)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            if (File.Exists(FilePath))
            {

                try
                {
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(FilePath),$"{Guid.NewGuid()}_{Path.GetFileName(FilePath)}");
                    File.Move(FilePath, tempFilePath);

                    File.Delete(tempFilePath);
                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting File: {ex.Message}");
                    return false;
                }

               
            }
            else { return false; }
        }
        private static async Task<string> FileNameConstructor(string FileKey)
        => $"{FileKey}-{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid()}";
    }
}
