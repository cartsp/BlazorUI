using System.Collections.Concurrent;

namespace BlazorEffects.Core.Interop;
public sealed class JsModuleLoader(IJSRuntime jsRuntime)
{
    private static readonly ConcurrentDictionary<string, Task<IJSObjectReference>> _moduleCache = new();

    /// <summary>
    /// Import a JS module by path. Results are cached per path.
    /// </summary>
    public Task<IJSObjectReference> ImportAsync(string modulePath)
    {
        return _moduleCache.GetOrAdd(modulePath, path => jsRuntime.InvokeAsync<IJSObjectReference>("import", path).AsTask());
    }
}
