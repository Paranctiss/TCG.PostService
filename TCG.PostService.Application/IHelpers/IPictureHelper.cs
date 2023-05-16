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
        void PrepareToDownload();
        void SetUpExtensionDirectory(string userId);
        void SavePicture(string name, string base64);
        string GetDossierPhoto();
    }
}
