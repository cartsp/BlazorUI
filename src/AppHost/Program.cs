using Infrastructure;
using Presentation;
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
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration);

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<AppHost.App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program;
