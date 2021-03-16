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
        public DeploymentService(DeploymentConfiguration configuration, DeploymentPipelineFactory pipelineFactory,
            FileUploadHandlerFactory fileUploadHandler)
        {
            this.configuration = configuration;
            this.pipeline = pipelineFactory.Create(configuration.SSHLogin);
            this.fileUploadHandler = fileUploadHandler;
        }

        public void Deploy()
        {
            Package();
            Upload();
            PostUpload();
        }

        private void ExecuteCommands(List<Command> unorderedCommands)
        {
            unorderedCommands.OrderBy(x => x.Position)
                .ToList().ForEach(command =>
                {
                    pipeline.RunCommand(command.CommandText);
                });
        }

        public void Package()
        {
            var packageCommands = configuration.PackagingSteps;

            ExecuteCommands(packageCommands);
        }

        public void Upload()
        {
            var fileHandler = fileUploadHandler.GetHandler(pipeline);
            fileHandler.UploadAsync(configuration.CodeUpload);
        }

        public void PostUpload()
        {
            var deploymentSteps = configuration.DeploymentSteps;

            ExecuteCommands(deploymentSteps);
        }
    }
}
