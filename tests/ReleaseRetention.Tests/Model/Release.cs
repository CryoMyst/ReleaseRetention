using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ReleaseRetention.Abstractions.Model;
using ReleaseRetention.Tests.Converters;

namespace ReleaseRetention.Tests.Model
{
    public class Release : IRelease
    {
        [JsonIgnore]
        public ModelContext Context { get; set; } = null!;
        
        [JsonPropertyName("Id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("ProjectId")]
        public string ProjectId { get; set; } = null!;

        [JsonPropertyName("Version")]
        [JsonConverter(typeof(VersionJsonConverter))]
        public IVersion? Version { get; set; } = null!;
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default;

        [Newtonsoft.Json.JsonIgnore]
        public IProject? Project => Context.Projects.FirstOrDefault(p => p.Id == ProjectId);
        [Newtonsoft.Json.JsonIgnore]
        public ICollection<IDeployment> Deployments => 
            Context.Deployments.Where(d => d.ReleaseId == Id).ToList();
    }
}