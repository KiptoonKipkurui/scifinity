using Scifinity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Scifinity.Core.Infrastructure
{
    public class DeploymentPipelineFactory
    {
        ILogger<DeploymentPipelineFactory> logger;
        ILogger<DeploymentPipeline> pipelineLogger;
        public DeploymentPipelineFactory(ILogger<DeploymentPipelineFactory> logger,
            ILogger<DeploymentPipeline> pipelineLogger)
        {
            this.logger = logger;
            this.pipelineLogger = pipelineLogger;
        }
        public DeploymentPipeline Create(SSHLogin login)
        {
            var connection = GetConnectionInfo(login);
            var sftpClient = GetConnectedSftpClient(connection);
            var sshClient = GetConnectedSSHClient(connection);


            var pipeline = new DeploymentPipeline(sftpClient, sshClient, pipelineLogger);

            return pipeline;
        }


        private ConnectionInfo GetConnectionInfo(SSHLogin loginInfo)
        {
            var privateKeyFile = new PrivateKeyFile(loginInfo.PrivateKeyFilePath);

            var connection = new ConnectionInfo(loginInfo.IPOrHostFQDN, loginInfo.User,
                new PrivateKeyAuthenticationMethod(loginInfo.User, privateKeyFile));

            return connection;
        }

        private SftpClient GetConnectedSftpClient(ConnectionInfo connection)
        {
            var client = new SftpClient(connection);
            client.Connect();
            logger.LogInformation($"Connected to host {connection.Host} for sftp");

            return client;
        }

        private SshClient GetConnectedSSHClient(ConnectionInfo connection)
        {
            var client = new SshClient(connection);
            client.Connect();
            logger.LogInformation($"Connected to host {connection.Host} for ssh commands");

            return client;
        }
    }
}
