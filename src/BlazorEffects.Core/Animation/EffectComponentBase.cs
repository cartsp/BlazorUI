namespace BlazorEffects.Core.Animation;

/// <summary>
/// Abstract base class for all BlazorEffects canvas-based effect components.
/// Provides shared infrastructure: JS module loading, config-hash diffing,
/// canvas element reference, lifecycle management, and disposal.
/// </summary>
public abstract class EffectComponentBase : ComponentBase, IAsyncDisposable
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Target frames per second. Clamped 1-120. Lower = less CPU/GPU.
    /// </summary>
    [Parameter] public int TargetFps { get; set; } = EffectDefaults.DefaultFps;

    /// <summary>
    /// CSS height of the effect container. Any valid CSS value.
    /// </summary>
    [Parameter] public string Height { get; set; } = EffectDefaults.DefaultHeight;

    /// <summary>
    /// Optional CSS class on the content overlay.
    /// </summary>
    [Parameter] public string? CssClass { get; set; }

    /// <summary>
    /// Optional child content rendered over the effect.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    // Shared infrastructure
    protected ElementReference CanvasElement { get; set; }
    protected IJSObjectReference? Module { get; set; }
    protected string? InstanceId { get; set; }
    private string? _previousConfigHash;

    // Subclasses implement these
    protected abstract string ModulePath { get; }
    protected abstract object BuildConfig();
    protected abstract string ComputeConfigHash();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        Module = await JS.InvokeAsync<IJSObjectReference>("import", ModulePath);
        InstanceId = await Module.InvokeAsync<string>("init", CanvasElement, BuildConfig());
        _previousConfigHash = ComputeConfigHash();
    }

    protected override async Task OnParametersSetAsync()
    {
        TargetFps = Math.Max(EffectDefaults.MinFps, Math.Min(TargetFps, EffectDefaults.MaxFps));

        if (Module is null || InstanceId is null) return;

        var currentHash = ComputeConfigHash();
        if (currentHash == _previousConfigHash) return;
        _previousConfigHash = currentHash;

        try
        {
            await Module.InvokeVoidAsync("update", InstanceId, BuildConfig());
        }
        catch (JSDisconnectedException) { }
    }

    public async ValueTask DisposeAsync()
    {
        if (Module is not null && InstanceId is not null)
        {
            try { await Module.InvokeVoidAsync("dispose", InstanceId); }
            catch (JSDisconnectedException) { }
        }
        if (Module is not null)
        {
            await Module.DisposeAsync();
        }
    }
}
