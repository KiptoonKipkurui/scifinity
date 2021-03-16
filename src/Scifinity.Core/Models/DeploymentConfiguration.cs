using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Scifinity.Core.Models
{
    public record DeploymentConfiguration
    {
        [JsonPropertyName("packaging_steps")]
        public List<Command> PackagingSteps { get; init; }
        [JsonPropertyName("ssh_login")]
        public SSHLogin SSHLogin { get; init; }
        [JsonPropertyName("code_upload")]
        public CodeUpload CodeUpload { get; init; }
        [JsonPropertyName("deployment_steps")]
        public List<Command> DeploymentSteps { get; init; }
    }
}
