using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Scifinity.Core.Models
{
    public record Command
    {
        [JsonPropertyName("position")]
        public int Position { get; init; }
        [JsonPropertyName("command_text")]
        public string CommandText { get; init; }
    }
}
