namespace BlazorEffects.Core.Interop;

/// <summary>
/// Interface for JS effect bridge modules implementing the init/update/dispose contract.
/// </summary>
public interface IJsEffectBridge
{
    /// <summary>
    /// Initialize the effect on the given canvas element with the provided config.
    /// Returns a unique instance ID.
    /// </summary>
    Task<string> InitAsync(ElementReference canvas, object config);

    /// <summary>
    /// Update an existing instance with new config values.
    /// </summary>
    Task UpdateAsync(string instanceId, object config);

    /// <summary>
    /// Dispose an instance, stopping animation and cleaning up resources.
    /// </summary>
    Task DisposeInstanceAsync(string instanceId);
}
