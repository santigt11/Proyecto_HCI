# 1. Project Overview

**Project Type:** VR Cognitive Training Game using XR Interaction Toolkit. It leverages Unity XR APIs to create immersive interaction and feedback systems aimed at evaluating and improving attention and decision-making.

**Target Platforms:** Standalone Windows (tested with PC‑based XR headset e.g. Meta Quest via OpenXR).

**Core Features / Pillars:**
- Fully immersive VR environment built on XR Interaction Toolkit.
- Dynamic stimulus spawning and real‑time attention metrics tracking.
- Haptic feedback integration for tactile reward/penalty cues.
- AI‑driven cognitive adaptation via decision trees (ArbolDecision.cs, GestorDificultad.cs).
- Real‑time UI feedback and performance metrics visualization through FeedbackCanvas.

# 2. Gameplay Flow / User Loop

**Major States:**
1. **Initialization:** GameManager boots, initializing VR environment and input.
2. **Stimulus Presentation:** EstimuloManager spawns sequence of visual stimuli in spawn area.
3. **Player Interaction:** User responds via VR controller; events routed through EstimuloInteractable and VRInteractionHandler.
4. **Feedback:** Immediate positive/negative feedback UI and haptics from HapticFeedbackManager + FeedbackCanvas.
5. **Metrics Update:** Metrica.cs logs precision, time, and error rates for current session.
6. **Adaptive Adjustment:** GestorDificultad and NivelAtencion adjust difficulty parameters for next session.

**Core Loop:** Player reacts to stimuli → response evaluated → feedback delivered → metrics update → next trial with adaptive variation.

**State Transitions:** Controlled by GameManager. Scene transitions minimal—primary loop occurs within SampleScene.

# 3. Architecture (Runtime + Editor)

**Runtime Systems:**
- **GameManager:** Central controller initializing managers and state transitions.
- **EstimuloManager:** Spawns visual stimuli (via Estimulo prefabs) within defined SpawnArea.
- **HapticFeedbackManager:** Manages patterned haptic signals for left/right controllers.
- **VRInteractionHandler:** Handles XR events, selection, trigger input, etc.
- **UI Layer:** FeedbackCanvas manages performance text and feedback visuals.
- **AI Subsystem:** ArbolDecision and NivelAtencion model cognitive decision paths and target difficulty curves.

**Editor Tooling:**
- Several editor scripts automate scene setup (AutoVRSetup, VRSceneSetup, SceneConfigurator). Useful for consistent XR rig initialization.

**Entry Points:**
- Entry scene: `Assets/Scenes/SampleScene.unity`.
- `GameManager` MonoBehaviour acts as bootstrap logic.

**Patterns & Communication:**
- Event‑based decoupling between interaction events and feedback systems.
- Managers communicate via UnityEvents and shared scriptable references.
- XR input handled by new Input System via InputActionAssets.

# 4. Scene Overview & Responsibilities

**Scenes:**
1. **XR DemoScene (Toolkit Sample):** Demonstrates all interaction types (gaze, poke, far grab, climb) used for testing or reference.
2. **SampleScene (Main):** Main project scene – integrates custom gameplay scripts and training logic.
3. **Scenes/SampleScene (Alternate build variant):** Minimal gameplay test scene.

**Loading Strategy:** Single‑scene runtime. Additive loads unnecessary. XR initialization via XR Interaction Management.

**Responsibilities:** Main scene owns all active managers (GameManager, EstimuloManager, UI, HapticFeedbackManager).

**Constraints:** Scene must always include XR Origin prefab and GameManager hierarchy. HapticFeedbackManager must exist or feedback will throw nulls.

# 5. UI System

**Framework:** Classic UGUI (Canvas + TextMeshPro)

**Navigation:** Static screen elements; displayed in world‑space canvas visible in headset.

**Binding Logic:** TextMeshPro fields (TextoPrecision, TextoErrores, etc.) updated by InterfazRetroalimentacion.cs through GameManager metrics events.

**UI Style:**
- Uses Liberation Sans font.
- Clear contrasting panels (white/black) from Blanco.mat, Negro.mat.
- Minimalist VR HUD aesthetic, centered readability.

# 6. Asset & Data Model

**Asset Style:**
Realistic minimalist URP lighting. Uses UniversalRenderPipeline with two base renderers (PC/Mobile). Scenes employ physically based materials with subtle lighting and bloom from VolumeProfiles.

**Data Formats:**
- Stimulus prefabs (`EstimuloBlanco`, `EstimuloNegro`) define color/state.
- Parameters stored in ScriptableObjects for render pipeline and volume settings.
- Runtime data captured by `SesionVR` and `Metrica` classes as transient memory objects.

**Asset Organization:**
- `/Scripts` subdivided by responsibility (AI, Core, UI, VR…).
- `/Prefabs` for stimuli, `/Materials` for visuals, `/Scenes` for levels.

**Naming Rules:** Classes and prefabs use Spanish nouns (matching cognitive‑training origin).

# 7. Project Structure (Repo & Folder Taxonomy)

**Key Folders:**
- `Assets/Scripts`: all C# sources grouped by domain.
- `Assets/Prefabs`: gameplay and environment prefabs.
- `Assets/Materials`: color/visual resources.
- `Assets/Scenes`: main build scene.
- `Assets/Samples/XR Interaction Toolkit`: reference and base assets for XR interaction.

**Conventions:**
- Gameplay logic kept separate from toolkit sample content.
- Prefab naming matches logical function (`EstimuloBlanco`, `EstimuloNegro`).

# 8. Technical Dependencies

**Unity & Pipeline:**
- Unity 6000.3.3f1 using **Universal Render Pipeline (URP)** asset PC_RPAsset.

**XR Runtime:**
- **XR Interaction Toolkit 3.3.1** with **OpenXR Plugin 1.16.1**.

**Key Third‑Party Packages:**
- `com.unity.inputsystem` 1.17.0  – Unified input for XR.
- `com.unity.xr.management` 4.5.4 – XR lifecycle.
- `com.unity.xr.core-utils` 2.5.3 – XR utilities.
- `com.unity.ai.*` 1.5.x  – AI support packages for reasoning and data generation.

**External Services:** None required; self‑contained standalone build.

# 9. Build & Deployment

**Build Steps:**
1. Open main scene `Assets/Scenes/SampleScene.unity`.
2. Configure build target: `StandaloneWindows64`.
3. Ensure OpenXR loader active and PC_RPAsset assigned.
4. Menu → File → Build and Run.

**Supported Platforms:** Windows with XR headset via OpenXR runtime.

**CI/CD:** None configured. Manual build only.

**Environment Requirements:** OpenXR installed; XR headset connected or simulator enabled.

# 10. Style, Quality & Testing

**Coding Style:**
- Spanish identifiers for user‑facing elements, PascalCase for scripts/classes.
- Managers are singular. One instance per scene.

**Performance Guidelines:**
- URP render scale = 1.0; consider lowering for mobile performance.
- Avoid dynamic allocation inside Update().

**Testing Strategy:**
- Primary testing with XR Device Simulator.
- Manual gameplay validation plus Unity Test Framework.

**Validation Rules:**
- Ensure GameObjects with interaction colliders include XR Interactable components.

# 11. Notes, Caveats & Gotchas

- **Known Issues:** Missing HapticFeedbackManager will disable tactile response.
- **Dependency Rules:** EstimuloManager requires properly configured prefabs; ensure material consistency.
- **Deprecated Systems:** None.
- **Platform Caveats:** Shader complexity can stress lower‑end GPUs under URP HDR. Disable ScreenSpaceAO for better performance.
