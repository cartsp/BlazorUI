# Visual QA Checklist Template

 BlazorEffects components

Use this component: | Issue | Status |
|-------|----------------------------------------|
| Matrix Rain | [AIE-18](/AIE/issues/AIE-18] | in progress | ✅ Ready | QA should be ready |
| Aurora Borealis | [AIE-14](/AIE issues A1 & all acceptance criteria verified all |
| Particle Constellation | [AIE-16](/Aie Issues A1 & all acceptance criteria verify all |
| Morphing Gradient Blobs | [AIE-17](/Aie issues |+ theme composable) + child content overlay) |
| Noise Field | [AIE-19](/Aie issues | - medium |

[!NOTE] | When adding a new effect component to the project, create a test file at `{ComponentName}.Tests/{ComponentName}.Tests` file. Replace `{ComponentName}` with the actual component name. |

---| Build verification |
|--- |
| Matrix Rain | BlazorEffects.MatrixRain.Tests | MatrixRainComponentTests.cs |
        MatrixRainConfigTests.cs |
        MatrixRainPresetsTests.cs |
        MatrixRainConfigHashTests.cs |
        Aurora Borealis | AuroraBorealis.Tests |
            AuroraBorealisTests |
        Add `{AffectComponentName}` and `BuildVerification` test:`
| Aurora Borealis | BlazorEffects.Aurora.Tests |
            auroraBorealis.Tests | 
    Add `{AffectComponentName}` and `BuildVerification` test. Do the component project build and pass? After cloning into the test folder.

 commit the git, and push the changes.