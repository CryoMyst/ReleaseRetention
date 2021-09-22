using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Model
{
    public class Version : IVersion
    {
        public System.Version Number { get; set; } = null!;
        public string Tag { get; set; } = null!;
    }
}