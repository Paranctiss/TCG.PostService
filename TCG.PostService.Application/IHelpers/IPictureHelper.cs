using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.Contracts;

namespace TCG.PostService.Application.IHelpers
{
    public interface IPictureHelper
    {
        Task SavePictureToAzure(string nomFichier, string base64String);
        string GetDossierPhoto();
    }
}
