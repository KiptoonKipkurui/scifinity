using Scifinity.Core.FileUploadHandlers;
using Scifinity.Core.Infrastructure;
using Scifinity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scifinity.Core
{
    public class DeploymentService
    {
        DeploymentConfiguration configuration;
        DeploymentPipeline pipeline;
        FileUploadHandlerFactory fileUploadHandler;
        PowershellClient powershellClient;

        public DeploymentService(DeploymentConfiguration configuration, DeploymentPipelineFactory pipelineFactory,
            FileUploadHandlerFactory fileUploadHandler)
        {
            this.configuration = configuration;
            this.pipeline = pipelineFactory.Create(configuration.SSHLogin);
            this.fileUploadHandler = fileUploadHandler;
            powershellClient = new PowershellClient();
        }

        public void Deploy()
        {
            Console.WriteLine("Deployment starting");
            Package();
            Upload();
            PostUpload();
            Console.WriteLine("Deployment completed");
        }

        private void ExecuteCommands(List<Command> unorderedCommands)
        {
            unorderedCommands.OrderBy(x => x.Position)
                .ToList().ForEach(command =>
                {
                    Console.WriteLine($"Running command position {command.Position}. {command.CommandText}");
                    pipeline.RunCommand(command.CommandText);
                });
        }

        private void ExecutePowershellCommands(List<Command> unorderedCommands)
        {
            unorderedCommands.OrderBy(x => x.Position)
                .ToList().ForEach(command =>
                {
                    Console.WriteLine($"Running command position {command.Position}. {command.CommandText}");
                    powershellClient.RunCommand(command.CommandText);
                });
        }

        public void Package()
        {
            var packageCommands = configuration.PackagingSteps;

            ExecutePowershellCommands(packageCommands);
        }

        public void Upload()
        {
            var fileHandler = fileUploadHandler.GetHandler(pipeline);
            fileHandler.Upload(configuration.CodeUpload);
        }

        public void PostUpload()
        {
            var deploymentSteps = configuration.DeploymentSteps;

            ExecuteCommands(deploymentSteps);
        }
    }
}
