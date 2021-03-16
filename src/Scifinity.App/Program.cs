using Scifinity.Core.Infrastructure;
using System;
using Microsoft.Extensions.Logging;
using Scifinity.Core.Models;
using Scifinity.Core.FileUploadHandlers;
using Scifinity.Core;

namespace Scifinity.App
{
    class Program
    {
        static DeploymentConfigurationBuilder configurationBuilder;
        static ILoggerFactory loggerFactory = LoggerFactory.Create((config)=>
        {
            config.SetMinimumLevel(LogLevel.Information);
        });
        static ILogger<Program> logger;
        static ILogger<DeploymentConfigurationBuilder> deploymentConfigLogger;
        static ILogger<DeploymentPipelineFactory> deploymentFactoryLogger;
        static ILogger<DeploymentPipeline> pipelineLogger;
        static FileUploadHandlerFactory fileUploadHandlerFactory;
        static Program()
        {
            logger = loggerFactory.CreateLogger<Program>();
            deploymentConfigLogger = loggerFactory.CreateLogger<DeploymentConfigurationBuilder>();
            deploymentFactoryLogger = loggerFactory.CreateLogger<DeploymentPipelineFactory>();
            pipelineLogger = loggerFactory.CreateLogger<DeploymentPipeline>();
            fileUploadHandlerFactory = new FileUploadHandlerFactory();
        }
        static void Main(string[] args)
        {
            configurationBuilder = new DeploymentConfigurationBuilder(deploymentConfigLogger);
            DeploymentPipelineFactory pipelineFactory = new DeploymentPipelineFactory(deploymentFactoryLogger, pipelineLogger);
            string defaultFile = string.Empty;

            if (args.Length == 0)
            {
                defaultFile = "sample_deployment_config.json"; 
            }

            DeploymentConfiguration deploymentConfiguration = configurationBuilder.GetConfiguration(defaultFile);

            DeploymentService deploymentService = 
                new DeploymentService(deploymentConfiguration, pipelineFactory, fileUploadHandlerFactory);

            deploymentService.Deploy();
        }
    }
}
