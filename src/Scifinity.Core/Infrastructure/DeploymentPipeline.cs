using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Scifinity.Core.Models;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Scifinity.Core.Infrastructure
{
    public class DeploymentPipeline
    {
        SshClient sshClient;
        SftpClient sftpClient;
        ILogger<DeploymentPipeline> logger;
        public DeploymentPipeline(SftpClient sftpClient, SshClient sshClient, 
            ILogger<DeploymentPipeline> logger)
        {
            this.sshClient = sshClient;
            this.sftpClient = sftpClient;
            this.logger = logger;
        }

        public void RunCommand(string commandText)
        {
            var command = sshClient.RunCommand(commandText);
            logger.LogInformation($"Command Result: {command.Result}");
        }

        public void UploadFile(string path, string destinationPath)
        {
            var sourceFileStream = File.OpenRead(path);
            sftpClient.UploadFile(sourceFileStream, destinationPath);
            logger.LogInformation($"Sucessfully uploaded file to {destinationPath}");
        }
    }
}
