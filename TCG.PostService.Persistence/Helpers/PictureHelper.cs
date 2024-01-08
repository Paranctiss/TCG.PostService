using Amazon.S3;
using Amazon.S3.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TCG.PostService.Application.IHelpers;

namespace TCG.PostService.Persistence.Helpers
{
    public class PictureHelper : IPictureHelper
    {
        //private string cheminPhoto = "https://tcgaccount.blob.core.windows.net/tcg-container/";
        private string cheminPhoto = "https://tcgplacebucket.s3.eu-north-1.amazonaws.com/";
        private string blobStorageContainerName = "tcg-container";
        private string blobStorageConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=tcgaccount;AccountKey=zW1swQgpCjxVotb8RbDsjWUAI/z0VMF0xptY51iBROp8fKri2DwKqyPXZ9xK8pfaw2ENT4qnponz+AStCikRZg==;EndpointSuffix=core.windows.net";
        public async Task SavePictureToAzure(string nomFichier, string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);

            // If the blob already exists, this will overwrite it.
            var blob = container.GetBlobClient(nomFichier + ".png");
            using (var stream = new MemoryStream(imageBytes))
            {
                var blobUploadOptions = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders()
                    {
                        ContentType = "image/png"
                    }
                };
                await blob.UploadAsync(stream, blobUploadOptions);
            }
        }
        
        public string GetDossierPhoto()
        {
            return cheminPhoto;
        }

        public async Task SavePictureToAWS(string nomFichier, string base64String)
        {
            // Remplacez par vos clés d'accès AWS.
            string bucketName = "tcgplacebucket";
            string accessKey = "AKIAZFLO6JSTD2J5PZGG";
            string secretKey = "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";
            string serviceUrl = "https://s3.eu-north-1.amazonaws.com"; // Remplacez par l'URL de service correspondant à votre région AWS.

            byte[] imageBytes = Convert.FromBase64String(base64String);

            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                // La région d'authentification peut être différente en fonction de votre bucket
                AuthenticationRegion = "eu-north-1",
            };
            using var client = new AmazonS3Client(accessKey, secretKey, config);

            try
            {
                using var stream = new MemoryStream(imageBytes);
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = nomFichier + ".png",
                    InputStream = stream,
                    ContentType = "image/png"
                };

                // Exécutez l'opération d'upload
                await client.PutObjectAsync(putRequest);

                // Construisez l'URL de l'image. Cette URL dépendra de votre configuration de bucket S3
                string imgName = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{nomFichier}.png";
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
