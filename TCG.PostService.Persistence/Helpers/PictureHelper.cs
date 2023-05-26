using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TCG.PostService.Application.IHelpers;

namespace TCG.PostService.Persistence.Helpers
{
    public class PictureHelper : IPictureHelper
    {
        private string cheminPhoto = "https://tcgaccount.blob.core.windows.net/tcg-container/";
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

    }
}
