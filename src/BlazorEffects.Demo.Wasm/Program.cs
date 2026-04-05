using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorEffects.Demo.Wasm;
using BlazorEffects.Playground;
using BlazorEffects.Noise;
using BlazorEffects.Blobs;
using BlazorEffects.Aurora;
using BlazorEffects.MatrixRain;
using BlazorEffects.Particles;
using BlazorEffects.GradientWaves;
using BlazorEffects.Starfield;
using BlazorEffects.FireEmbers;
using BlazorEffects.Ripple;
using BlazorEffects.VortexTunnel;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register BlazorEffects Playground with all effect descriptors
builder.Services.AddBlazorEffectsPlayground(b => b
    .AddEffect<NoiseFieldConfig, NoiseFieldDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "NoiseField"))
    .AddEffect<MorphingBlobsConfig, MorphingBlobsDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "MorphingBlobs"))
    .AddEffect<AuroraBorealisConfig, AuroraBorealisDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "AuroraBorealis"))
    .AddEffect<MatrixRainConfig, MatrixRainDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "MatrixRain"))
    .AddEffect<ParticleConstellationConfig, ParticleConstellationDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "ParticleConstellation"))
    .AddEffect<GradientWavesConfig, GradientWavesDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "GradientWaves"))
    .AddEffect<StarfieldConfig, StarfieldDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "Starfield"))
    .AddEffect<FireEmbersConfig, FireEmbersDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "FireEmbers"))
    .AddEffect<RippleConfig, RippleDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "Ripple"))
    .AddEffect<VortexTunnelConfig, VortexTunnelDescriptor>(
        c => BlazorMarkupGenerator.Generate(c, "VortexTunnel"))
);

await builder.Build().RunAsync();
