# Changelog

All notable changes to BlazorEffects will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.3.0] - 2026-04-05

### Added
- **Starfield** effect (`BlazorEffects.Starfield`) — warp-speed star field with 3D-to-2D projection (Hyperspace, Warp Drive, Golden Drift, Sparse, Blizzard, Retro Terminal presets)
- **Fire/Embers** effect (`BlazorEffects.FireEmbers`) — rising flame particles with glowing embers (Campfire, Inferno, Candles, Volcanic, Ethereal, EmberStorm presets)
- **Ripple** effect (`BlazorEffects.Ripple`) — concentric rings expanding from click/touch points (Classic, Neon, Zen, Raindrops, Sonic presets)
- **Vortex/Tunnel** effect (`BlazorEffects.VortexTunnel`) — spiraling tunnel with eye-drawing depth (Wormhole, Neon Spiral, Hypnotic, Deep Space, Retro presets)
- Shared `prefers-reduced-motion` utility in `BlazorEffects.Core` — all effects respect OS/browser accessibility setting
- `aria-hidden="true"` and `role="presentation"` on all canvas elements
- Interactive Playground UI with live preview, parameter editor, and code export (`BlazorEffects.Playground`)
- Effect descriptor abstractions for playground integration (`IEffectDescriptor<TConfig>`)
- EffectDemoLayout and FPS counter for the demo application
- GitHub Actions CI/CD pipeline (`build.yml`) — build, test, and format verification on every push/PR
- GitHub Actions NuGet packaging pipeline (`pack.yml`) — `dotnet pack` on version tags
- NuGet package metadata configured for all 10 effect libraries + Core + Playground
- Getting-started guide (`docs/getting-started.md`)
- API reference scaffold (`docs/api-reference.md`)
- Performance benchmarks for all 10 effects (`tests/QA/performance-benchmarks.md`)
- Accessibility audit (`docs/accessibility.md`)

### Changed
- MatrixRain demo refactored to use shared EffectDemoLayout
- All 10 effects now support `prefers-reduced-motion` with Pause/Minimal/Ignore behaviors

[0.3.0]: https://github.com/cartsp/BlazorUI/compare/v0.2.0...v0.3.0

## [0.2.0] - 2026-04-05

### Added
- **Morphing Blobs** effect (`BlazorEffects.Blobs`) — organic gradient blobs with smooth morphing animation
- **Aurora Borealis** effect (`BlazorEffects.Aurora`) — flowing aurora curtains with multi-color palettes (Classic, Arctic, Sunset, Emerald, Cosmic presets)
- **Noise Field** effect (`BlazorEffects.Noise`) — animated simplex noise textures with flowing motion
- **Particle Constellation** effect (`BlazorEffects.Particles`) — connected particle network (Default, Cyberpunk, DeepSpace, Amber, Minimal, Dense presets)
- Effect descriptors for all components enabling playground parameter discovery
- Comprehensive test suites for all effect components

## [0.1.0] - 2026-04-02

### Added
- Initial project scaffold with .NET 10 Blazor solution structure
- **BlazorEffects.Core** — shared abstractions (`EffectComponentBase`, `IEffectConfig`, `IJsEffectBridge`)
- **Matrix Rain** effect (`BlazorEffects.MatrixRain`) — cascading character rain with presets (Classic, Katakana, Binary, Hacker, Ghost)
- Canvas-based JS interop pattern with `init`/`update`/`dispose` lifecycle
- Multi-targeting support for .NET 8.0, 9.0, and 10.0
- Design token system and Tailwind CSS v4 integration
- DDD + CQRS architecture foundation (Domain, Application, Infrastructure, Presentation layers)
- Build script with format verification (`build.sh`)

[Unreleased]: https://github.com/cartsp/BlazorUI/compare/v0.3.0...HEAD
[0.2.0]: https://github.com/cartsp/BlazorUI/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/cartsp/BlazorUI/releases/tag/v0.1.0
