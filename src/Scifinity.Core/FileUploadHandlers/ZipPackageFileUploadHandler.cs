using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Scifinity.Core.Infrastructure;
using Scifinity.Core.Models;
using Microsoft.Extensions.Logging;

namespace Scifinity.Core.FileUploadHandlers
{
    public class ZipPackageFileUploadHandler : AbstractFileUploadHandler
    {
        DeploymentPipeline pipeline;
        
        public ZipPackageFileUploadHandler(DeploymentPipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public override async Task UploadAsync(CodeUpload codeUpload)
        {
            if (File.Exists(codeUpload.SourcePath))
            {
                await Task.CompletedTask;
                using (FileStream fs = File.Create(codeUpload.SourcePath))
                {
                    pipeline.UploadFile(fs, codeUpload.DestinationPath);
                }
            }
            else
            {
                Console.WriteLine($"Path not found {codeUpload.SourcePath}");
            }
        }
    }
}
