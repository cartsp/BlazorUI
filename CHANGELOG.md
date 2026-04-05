# Changelog

All notable changes to BlazorEffects will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Interactive Playground UI with live preview, parameter editor, and code export (`BlazorEffects.Playground`)
- Effect descriptor abstractions for playground integration (`IEffectDescriptor<TConfig>`)
- EffectDemoLayout and FPS counter for the demo application

### Changed
- MatrixRain demo refactored to use shared EffectDemoLayout

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

[Unreleased]: https://github.com/cartsp/BlazorUI/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/cartsp/BlazorUI/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/cartsp/BlazorUI/releases/tag/v0.1.0
