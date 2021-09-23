namespace ReleaseRetention
{
    /// <summary>
    ///     Used as a key to store specific state keys
    /// </summary>
    /// <param name="State">
    ///     The state in the key.
    /// </param>
    /// <param name="Id">
    ///     The id in the key.
    /// </param>
    /// <typeparam name="T">
    ///     The id type.
    /// </typeparam>
    internal record StatePolicyKey<T>(object State, T Id);
}