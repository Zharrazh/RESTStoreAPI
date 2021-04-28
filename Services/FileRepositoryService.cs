using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RESTStoreAPI.Setup.Config.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IFileRepositoryService
    {
        Task<SavedFileInfo> SaveFileAsync(IFormFile file, string dirPath, string fileName, string ext);

        void ValidateAndThrow(IFormFile file, string[] extentions = null);

        void DeleteFileAsync(string filePath);
    }
    public class FileRepositoryService : IFileRepositoryService
    {
        private readonly FileRepoConfigModel m_config;
        private readonly IWebHostEnvironment m_appEnv;
        private readonly IHashService m_hashService;
        public FileRepositoryService(IOptionsSnapshot<FileRepoConfigModel> configModelAcc, IWebHostEnvironment appEnv, IHashService hashService)
        {
            m_config = configModelAcc.Value;
            m_appEnv = appEnv;
            m_hashService = hashService;

        }

        public void ValidateAndThrow(IFormFile file, string[] permittedExtensions = null)
        {
            var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(permittedExtensions is not null) {
                if (string.IsNullOrEmpty(fileExt) || !permittedExtensions.Contains(fileExt))
                    throw new UnavailableFileExtensionException();
            }

            if (file.Length > m_config.MaxFileSizeByte)
            {
                throw new UnavailableFileSizeException();
            }

            
        }

        public async Task<SavedFileInfo> SaveFileAsync(IFormFile file, string dirPath, string fileName, string ext)
        {
            string fileHash = m_hashService.Hash(file);
            string uploadFilePath =  Path.Combine(m_appEnv.WebRootPath, dirPath );

            var dirInfo = new DirectoryInfo(uploadFilePath);
            if (!dirInfo.Exists)
                dirInfo.Create();
            string fullFileName = $"{fileName}-{fileHash}.{ext}";
            string filePathWithName = Path.Combine(uploadFilePath, fullFileName);
            using var stream = File.Create(filePathWithName);
            await file.CopyToAsync(stream);

            dirPath = dirPath.Replace(@"\\", "/");
            return new SavedFileInfo {
                FileName = fullFileName,
                Path = $@"{dirPath}/{fullFileName}"
            };
        }

        public void DeleteFileAsync(string filePath)
        {
            File.Delete(Path.Combine(m_appEnv.WebRootPath, filePath));
        }

        
    }

    public class SavedFileInfo
    {
        public string Path { get; set; }
        public string FileName { get; set; }
    }

    public class UnavailableFileExtensionException : Exception { }
    public class UnavailableFileSizeException : Exception { }
}
