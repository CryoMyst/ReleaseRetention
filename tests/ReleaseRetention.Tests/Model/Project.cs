using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Model
{
    public class Project : IProject
    {
        [JsonIgnore]
        public ModelContext Context { get; set; } = null!;
        
        [JsonPropertyName("Id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<IRelease> Releases => Context.Releases.Where(r => r.ProjectId == Id).ToList();
    }
}