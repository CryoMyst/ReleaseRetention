using System;

namespace ReleaseRetention.Abstractions.Model
{
    /// <summary>
    ///     Used to store a Version along with an optional tag.
    /// </summary>
    public interface IVersion
    {
        Version Number { get; }
        string Tag { get; }
    }
}