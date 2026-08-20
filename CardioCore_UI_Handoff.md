# CardioCore MR — UI Module Handoff & Heart-Restore Steps

**Date:** 2026-08-20 · **Scene:** `Assets/Scenes/CardioCore.unity` (saved) · **Unity instance:** `CardioCore@d162a934` (port 6400)

---

## 1. What is DONE (built, wired, verified in Editor)

**Scripts** — matched to the *real* project namespaces (not the doc's `CardioCore`):

- `Assets/Scripts/XR/ManualControlPanel.cs` — namespace `XR`, `using Heart;`. Rewritten to the full spec: Manual Mode toggle, 4 channel sliders, **+ the three preset buttons** (Diastole / Systole / Resume Live) and their handlers, which the previous on-disk version was missing.
- `Assets/Scripts/XR/InjectPanel.cs` — namespace `XR`, `using Core;`. New file.
- `Assets/Scripts/Core/DataClient.cs` — patched in place (GUID preserved, so the Services reference stayed intact): added `mockRhythm` / `mockMurmur` fields, `SetMockRhythm(...)` / `SetMockMurmur(...)`, and `StepMock` now emits those instead of hard-coded `"normal"` / `"none"`.
- Compiles clean — **zero errors**.

**Panel A — `ManualPanel`** (existing Meta UISet backplate, left side, `UiPanelAnchor.sideOffset = -0.35`)
- Under `CanvasRoot/UIBackplate/PanelRoot`: Manual Mode toggle, sliders AV Close / SL Open / Ventricle / Atria (min 0, max 1), buttons Diastole / Systole / Resume Live.
- `ManualControlPanel` attached. All references wired **except `driver`** (blocked — see §3).

**Panel B — `InjectPanel`** (duplicated from the backplate, right side, `UiPanelAnchor.sideOffset = +0.35`)
- Buttons Force Afib (red) / Force Murmur (red) / Back to Normal (green).
- `InjectPanel` attached, `client` → `Services/DataClient`, `panelRoot` + 3 buttons wired.
- **Verified end-to-end in Play:** Force Afib → `mockRhythm = afib`; Force Murmur → `mockMurmur = systolic`; Back to Normal → both reset.

**ISDK / interaction plumbing**
- Each panel's `RayInteractable._surface` and `PokeInteractable._surfacePatch` point to **its own** child `Surface` (verified — no cross-linking). Interactor rig is healthy: 4 PokeInteractors + 4 RayInteractors (both hands + both controllers), all enabled.
- `EventSystem` has exactly one input module (`PointableCanvasModule`) → highest priority, nothing shadows it.
- `UiPanelAnchor` auto-resolves the head anchor (`ResolveHead()` finds `CenterEyeAnchor`), so the null `headAnchor` field is fine. At runtime the panels sit 0.70 m apart (left/right of the head).

**Summon (updated — was breaking interactions):**
- A new always-active `UISummoner` GameObject (script `Assets/Scripts/XR/PanelSummoner.cs`) toggles the **whole panel GameObject** on **B** (ManualPanel, `OVRInput.Button.Two`) / **Y** (InjectPanel, `OVRInput.Button.Four`). Both panels start disabled (`hideOnStart = true`).
- **Why:** the earlier approach toggled `CanvasRoot`, which disabled the Canvas + Surface while the interactables (on the panel root) stayed enabled — so ISDK held interactables pointing at a disabled surface and re-registered unreliably on summon. Toggling the whole panel makes every ISDK component enable/disable **as a unit**, which is the Meta-recommended pattern and fixes the flaky poke/ray. The panel scripts no longer self-hide (`panelRoot = null`, `startHidden = false`) — the summoner is the single source of truth.

> Physical poke/ray *presses* still need the headset over Link to confirm (can't press a virtual finger in the Editor). The click → logic path is proven; the ISDK surfaces and module priority are correct, so poke/ray should work on-device.

---

## 2. The ONE thing left on the UI side

`ManualPanel → ManualControlPanel → Driver` is currently **empty**, because there is no `HeartJointDriver` in the scene right now (the heart prefab is broken — §3). Everything else on the panel is wired. The panel runs safely with a null driver (all calls are null-guarded); it just can't move valves until the driver exists.

Once the heart is back (§3), do **§4** — a single reference assignment.

---

## 3. Restoring the heart (your part — GLB is intact, prefab is corrupt)

**Diagnosis:** `Assets/Prefabs/Beating heart.prefab` **fails to load entirely** (corrupt asset) — that's why the scene shows `Beating heart (Missing Prefab with guid: 01f9fc8e776385a4baed42e4c4cf9c1a)`, a stub with only a Transform. The source model `Assets/Models/beating-heart/source/Beating heart.glb` loads fine. So rebuild the instance from the GLB.

Target hierarchy (from the handoff truth-sheet): `Beating heart → skeletal.3 → Root.4 → heart_jnt.5 → (all *_jnt.N leaves)`.

**Step by step:**

1. **Instantiate the model.** Drag `Assets/Models/beating-heart/source/Beating heart.glb` into the Hierarchy, as a **child of `HeartAnchor`**.
2. **Transform of that new heart object** (local, under HeartAnchor): Position `(0, 0, 0)`, Rotation `(0, 0, 0)`, Scale `(0.01, 0.01, 0.01)`. (The GLB is authored huge; 0.01 is the established scale.)
3. **Rename** it `Beating heart` (so it reads cleanly; not functionally required).
4. **Animator:** if the GLB instantiated with an `Animator`, **uncheck/disable** it (keep it, don't delete) — the motion is procedural via the driver.
5. **Add `HeartJointDriver`** (Add Component → "Heart Joint Driver") to the `Beating heart` root, and set its serialized fields:
   - **Cycle** → the `CardiacCycleController` on the **`Services`** GameObject.
   - **Joint Root** → the **`heart_jnt.5`** transform inside the model (expand `skeletal.3 → Root.4 → heart_jnt.5`).
   - **Global Gain** = `1`, **Drive Enabled** = ✔ (checked). Leave the Manual Inspection block alone.
   - *Note:* peak poses are already baked into the script's `specs[]` (from `beat_pose_dump.txt`); the driver captures the **rest** pose automatically at Awake. You do **not** need the dump file again. If any joint leaf name in the GLB differs, the driver logs `joint not found: <name>` at play — check the Console and fix that leaf's name if so.
6. **Delete the broken stub:** remove the old `Beating heart (Missing Prefab ...)` object under HeartAnchor (the Transform-only one). Keep the new one.
7. **Re-check the grab pieces on `HeartAnchor`** (these were unaffected, just confirm):
   - `HeartRigController.grabbable` → the `Grabbable` on HeartAnchor (same GameObject).
   - `HeartAnchor`'s `BoxCollider` roughly encloses the heart (adjust size/center if the grab feels off). Rigidbody stays kinematic, no gravity.
8. **(Optional but recommended) Replace the corrupt prefab** so this can't recur: with the fixed `Beating heart` selected, drag it onto `Assets/Prefabs/Beating heart.prefab` to overwrite (Replace / Apply), or delete that corrupt `.prefab` and create a fresh one. Not required for the demo — an in-scene object works.

---

## 4. Wire the ManualPanel driver (after §3)

**Inspector way:** select `ManualPanel` → in **Manual Control Panel** → drag the `Beating heart` object (which now has `HeartJointDriver`) into the **Driver** slot. Save the scene.

**Or ping me** — once the heart's in the scene I can set it in one call and run the full verification for you.

---

## 5. On-headset verification checklist (Editor Play over Link, or APK)

1. Enter Play. Console: no new errors from `ManualControlPanel` / `InjectPanel` / `DataClient` / `PanelSummoner`. (The `ErrorFormFactorUnavailable` / Link lines only appear without an HMD; ignore in that case.)
2. Press **B** → ManualPanel appears on your left (whole panel enables). Press **Y** → InjectPanel appears on your right. Press again → hide. Nothing is shown by default.
3. **Poke** a button with your fingertip **and** point the **controller ray** + trigger — both should press (both interactors are wired to each panel's Surface).
4. Toggle **Manual Mode** on → the beat **freezes** (driver switches to manual values). Move **AV Close / SL Open / Ventricle / Atria** sliders → the corresponding valves/chambers move. **Diastole** = all open/relaxed, **Systole** = valves closed + ventricle contracted, **Resume Live** = beat resumes.
5. On InjectPanel: **Force Afib** / **Force Murmur** → HUD rhythm/murmur labels change (via the mock hooks); **Back to Normal** resets.
6. **A** resets/re-centers the heart in front of you.

---

## 6. Reference notes

- **Button map:** A = reset heart · B = ManualPanel · Y = InjectPanel · X = free.
- **Namespaces (actual):** `HeartJointDriver` → `Heart`; `DataClient` → `Core` (uses `CardioCore.TwinState`); `ManualControlPanel` / `InjectPanel` / `HeartRigController` / `UiPanelAnchor` → `XR`.
- **HeartJointDriver public API** used by the panel: `SetManualMode(bool)`, `SetManualAvClose/SlOpen/Ventricle/Atria(float)`, `ManualMode` — all present and matched.
- **Manual polarity** (already handled inside the driver): AV valves close on S1 (`1 − MitralOpen`), semilunars open on ejection (`AorticOpen`), ventricle scale, atria position.
- **Frozen contracts untouched:** no changes to `HeartJointDriver` drive logic, `CardiacCycleController`, the Grabbable/transformer setup, or the `/data` contract. The DataClient edit only added mock-injection hooks behind the existing `useMock` path.
- **UI controls** use built-in uGUI (Toggle/Slider/Button) with `LegacyRuntime.ttf` text, on `PanelRoot` (a `LayoutElement.ignoreLayout` container filling the backplate) — per the verified-architecture note that UISet controls are standard Unity UI and poke/ray come from the backplate's ISDK components.
