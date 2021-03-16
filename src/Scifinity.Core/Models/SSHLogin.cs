using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Scifinity.Core.Models
{
    public record SSHLogin
    {
        [JsonPropertyName("method")]
        public LoginMethod LoginMethod { get; init; }
        [JsonPropertyName("ip_or_host_fqdn")]
        public string IPOrHostFQDN { get; init; }
        [JsonPropertyName("user")]
        public string User { get; init; }
        [JsonPropertyName("password")]
        public string Password { get; init; }
        [JsonPropertyName("private_key_file_path")]
        public string PrivateKeyFilePath { get; set; }
    }
}
