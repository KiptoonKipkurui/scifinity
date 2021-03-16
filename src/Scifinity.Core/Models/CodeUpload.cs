using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Scifinity.Core.Models
{
    public record CodeUpload
    {
        [JsonPropertyName("version")]
        public string Version { get; init; }
        [JsonPropertyName("package_type")]
        public CodePackageType PackageType { get; init; }
        [JsonPropertyName("source_path")]
        public string SourcePath { get; set; }
        [JsonPropertyName("destination_path")]
        public string DestinationPath { get; set; }
    }
}
