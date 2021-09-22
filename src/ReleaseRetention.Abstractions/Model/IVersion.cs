using System;

namespace ReleaseRetention.Abstractions.Model
{
    public interface IVersion
    {
        Version Number { get; }
        string Tag { get; }
    }
}