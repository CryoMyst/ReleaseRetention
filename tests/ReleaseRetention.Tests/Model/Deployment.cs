using System;
using System.Linq;
using System.Text.Json.Serialization;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Model
{
    public class Deployment : IDeployment
    {
        [JsonIgnore]
        public ModelContext Context { get; set; } = null!;
        
        [JsonPropertyName("Id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("ReleaseId")]
        public string ReleaseId { get; set; } = null!;
        [JsonPropertyName("EnvironmentId")]
        public string EnvironmentId { get; set; } = null!;
        [JsonPropertyName("DeployedAt")]
        public DateTime DeployedAt { get; set; } = default;


        [JsonIgnore]
        public IRelease? Release => Context.Releases.FirstOrDefault(r => r.Id == ReleaseId);
        [JsonIgnore]
        public IEnvironment? Environment => Context.Environments.FirstOrDefault(e => e.Id == EnvironmentId);
    }
}