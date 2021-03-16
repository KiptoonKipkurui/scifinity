using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using Scifinity.Core.Models;
using System.Text.Json;

namespace Scifinity.Core.Infrastructure
{
    public class DeploymentConfigurationBuilder
    {
        ILogger<DeploymentConfigurationBuilder> logger;

        public DeploymentConfigurationBuilder(ILogger<DeploymentConfigurationBuilder> logger)
        {
            this.logger = logger;
        }

        public DeploymentConfiguration GetConfiguration(string path)
        {
            var content = LoadConfiguration(path);
            DeploymentConfiguration configuration = ParseConfiguration(content);

            return configuration;
        }

        public DeploymentConfiguration ParseConfiguration(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new Exception("Unable to parse empty content");

            DeploymentConfiguration config = JsonSerializer.Deserialize<DeploymentConfiguration>(content);

            return config;
        }

        public string LoadConfiguration(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Config file not found: {path}");

            string content = File.ReadAllText(path);
            return content;
        }
    }
}
