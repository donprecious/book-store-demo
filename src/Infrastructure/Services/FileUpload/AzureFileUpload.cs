using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BookStore.Application.Common.Models;
using BookStore.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services.FileUpload;

 public class AzureFileUpload : IFileUpload
 {
     private IConfiguration _configuration;
     private string connectionString = "";
     private string path = "";
     private ILogger<AzureFileUpload> _logger;
        public AzureFileUpload(IConfiguration configuration, ILogger<AzureFileUpload> logger)
        {
            _configuration = configuration;
            _logger = logger;
            connectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString");
            path = _configuration.GetValue<string>("AzureStorage:Path");
           
        }
     
        

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        public async Task<Result<FileUploadResponse>> UploadFile(string strFileName, Stream stream)
        {
            try
            { 
                
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient =  blobServiceClient.GetBlobContainerClient("plute");
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                string fileName = this.GenerateFileName(strFileName);

                var result = await containerClient.UploadBlobAsync(fileName, stream);
                return Result<FileUploadResponse>.Success( new FileUploadResponse()
                {
                    Url = path + fileName,
                    FileName = fileName
                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to upload file");
               return Result<FileUploadResponse>.Failure(ex.Message);
            }
        }

    }