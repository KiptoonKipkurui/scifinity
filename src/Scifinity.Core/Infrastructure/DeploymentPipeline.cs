using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Scifinity.Core.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using Renci.SshNet.Common;

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
            this.sshClient = sshClient ?? throw new ArgumentNullException(nameof(sshClient));
            this.sftpClient = sftpClient ?? throw new ArgumentNullException(nameof(sftpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// run command
        /// </summary>
        /// <param name="commandText">command to be run in text form</param>
        public void RunCommand(string commandText)
        {
            //todo:validate commans
            var command = sshClient.RunCommand(commandText);

            if (!string.IsNullOrWhiteSpace(command.Error))
            {
                logger.LogError("Command excecution encountered error:", command.Error);
            }
            else
            {
                logger.LogInformation($"Command Result: {command.Result}");
            }
        }

        /// <summary>
        /// upload file to destination path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="destinationPath"></param>
        public void UploadFile(string path, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                Console.WriteLine("The destination path is missing. File Upload Operation Failed");

                return;
            }

            try
            {
                var sourceFileStream = File.OpenRead(path);
                sftpClient.UploadFile(sourceFileStream, destinationPath);
                //todo:handle exceptions
                logger.LogInformation($"Sucessfully uploaded file to {destinationPath}");
            }
            catch (SshConnectionException ex)
            {
                logger.LogError("Ssh connection was terminated with error message: ", ex.Message);
            }

            catch (SftpPermissionDeniedException ex)
            {
                logger.LogError("The Sftp operation failed with message: ", ex.Message);
            }

            catch (SshException ex)
            {
                logger.LogError("Ssh exceptions: ", ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Operation failed with exception: ", ex.Message);
            }
        }
    }
}
