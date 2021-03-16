using Scifinity.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scifinity.Core.FileUploadHandlers
{
    public class FileUploadHandlerFactory
    {
        public IFileUploadHandler GetHandler(DeploymentPipeline pipeline)
        {
            IFileUploadHandler fileUploadHandler = new ZipPackageFileUploadHandler(pipeline);

            return fileUploadHandler;
        }
    }
}
