using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.IHelpers;

namespace TCG.PostService.Persistence.Helpers
{
    public class PictureHelper : IPictureHelper
    {
        public static string DossierRacine { get; } = "C:/TCGPlace";
        public static string DossierPhoto { get; } = DossierRacine + "/PHOTO";

        public void PrepareToDownload()
        {
            // Créer dossier racine s'il n'existe pas
            if (!Directory.Exists(DossierRacine))
            {
                Directory.CreateDirectory(DossierRacine);
            }

            // Créer dossier photo s'il n'existe pas
            if (!Directory.Exists(DossierPhoto))
            {
                Directory.CreateDirectory(DossierPhoto);
            }

        }

        // Créer un dossier par user
        public void SetUpExtensionDirectory(string userId)
        {
            if (!Directory.Exists(DossierPhoto + "/" + userId))
            {
                Directory.CreateDirectory(DossierPhoto + "/" + userId);
            }
        }

        // Enregistrement photo
        public void SavePicture(string nomFichier, string base64String)
        {
            PrepareToDownload();
            byte[] imageBytes = Convert.FromBase64String(base64String);
            string cheminComplet = DossierPhoto + "/" + nomFichier + ".webp";
            File.WriteAllBytes(cheminComplet, imageBytes);
        }

        public string GetDossierPhoto()
        {
            return DossierPhoto;
        }

    }
}
