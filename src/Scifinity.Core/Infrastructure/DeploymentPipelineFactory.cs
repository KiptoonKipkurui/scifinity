using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Common;
using Scifinity.Core.Models;
using System;

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


        /// <summary>
        /// get connection info
        /// </summary>
        /// <param name="loginInfo"><see cref="SSHLogin"/></param>
        /// <returns></returns>
        private ConnectionInfo GetConnectionInfo(SSHLogin loginInfo)
        {
            var privateKeyFile = new PrivateKeyFile(loginInfo.PrivateKeyFilePath);

            var connection = new ConnectionInfo(loginInfo.IPOrHostFQDN, loginInfo.User,
                new PrivateKeyAuthenticationMethod(loginInfo.User, privateKeyFile));

            return connection;
        }

        /// <summary>
        /// get sftp client
        /// </summary>
        /// <param name="connection"><see cref="ConnectionInfo"/></param>
        /// <returns></returns>
        private SftpClient GetConnectedSftpClient(ConnectionInfo connection)
        {
            try
            {
                var client = new SftpClient(connection);
                client.Connect();
                logger.LogInformation($"Connected to host {connection.Host} for sftp");

                return client;
            }
            catch (SshConnectionException ex)
            {
                logger.LogError("Ssh connection was terminated with error message: ", ex.Message);
            }

            catch (SshAuthenticationException ex)
            {
                logger.LogError("SSH authentication error: ", ex.Message);
            }
            catch (Exception ex)
            {

                logger.LogError("Encountered exception: ", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// get SSH client
        /// </summary>
        /// <param name="connection"><see cref="ConnectionInfo"/></param>
        /// <returns></returns>
        private SshClient GetConnectedSSHClient(ConnectionInfo connection)
        {
            try
            {
                var client = new SshClient(connection);
                client.Connect();
                logger.LogInformation($"Connected to host {connection.Host} for ssh commands");

                return client;
            }

            catch (SshConnectionException ex)
            {
                logger.LogError("Ssh connection was terminated with error message: ", ex.Message);
            }

            catch (SshAuthenticationException ex)
            {
                logger.LogError("SSH authentication error: ", ex.Message);
            }
            catch (Exception ex)
            {

                logger.LogError("Encountered exception: ", ex.Message);
            }

            return null;
        }
    }
}
