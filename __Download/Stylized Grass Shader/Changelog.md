1.4.6

Fixed:
- Patched critical shader errors when using APV in Unity 6 (not supported)

1.4.5

Added:
- Billboarding, parameter to control the amount of vertical rotation

Changed:
- Removed shader error workaround for MacOS added in v1.4.4. The related bug is now fixed in 2022.3.36f1+

Fixed:
- Hotfix for OUTPUT_SH4 and SAMPLE_GI shader macro errors in Unity 6000.0.9f1+

1.4.4

Added:
- Foliage Renderer integration

Changed:
- Implemented proper error handling for Unity 6.
- Improved handling of grass benders below the grass, which should not apply any force.
- Corrected LOD Group set up on GrassPatch_Realistic prefab.

Fixed:
- Shader compile error on MacOS when using 2022.3.15+ (workaround for known URP bug)

1.4.3

Fixed:
- Wind gusting not respecting the wind direction configured on a material.
- Shader error when building for PS5

Removed:
- GrassBendingFeature script file (obsolete since v1.3.0)

1.4.2
Minimum supported version is now Unity 2021.3.16f1.

Added:
- Parameters to choose which vertex color channel is used for shading, wind and bending features.

Changed:
- Optimization improvement for runtime usage of color map baking (~22-60% faster). The render result is now kept in memory and the GPU->CPU coping normally done for saving is skipped.
- Shader code no longer requires editing to support DOTS instancing.

Fixed:
- Entity Graphics rendering not having any effect in a build in Unity 2022.3+.
- Shader error when Probes Volumes was enabled.

1.4.1
This is the last update supporting Unity 2020 LTS

Added:
- LOD Crossfading support for cast shadows in Unity 2022.2+

Fixed:
- Script compile error in Unity 2022.2 (only) 
- Shader error in Unity 2023.1.7+ (URP v15.0.8) due to now backported code from Unity 2023.2
- Grass turning black when Entity Graphics was in use without MaterialProperty components to define a color

1.4.0
Verified support for Unity 2023.1.1f1

Added:
- New general Grass Render Feature, this now handles Bending, Perspective Correction and Dithering in one render feature.
- Support for runtime Color Map rendering (GrassColorMapRenderer.Render()).
- Hue Variation Height parameter, allows limiting the hue shift to the tip of grass geometry.
- Option to use the camera's forward direction for Perspective Correction (on the render feature).
- Additional wind gust texture variant (omni-directional).
- Support for Rendering Layer Mask (lights and decals).

Changed:
- Improved UI for the Colormap Renderer inspector to provide more concise instructions and notifications.
- When saving a Colormap asset to a file, the texture is now saved as a separate .PNG file. This will allow Unity to properly compress it.
- Grass bending can now be disabled on the render feature.
- Angle/Distance fading now uses a dithering texture (assigned on the render feature).
- (Unity 2022.2+) LOD crossfading now uses the same shader function now native to URP.
- Hue variation is now applied on a per-pixel basis.

Fixed:
- Colormap not taking effect when entering Play mode, if it was saved to an asset.
- Non-terrain objects always rendering on top of terrains in a Colormap render output.
- Colormap Renderer, non-terrain objects not being restored to their original Y-position after rendering.
- Colormap Renderer, fresnel reflections potentially still being visible when using a third-party lighting system.
- Ensured the flower prefabs have a centered position by default.
- Grass benders not rendering using the SRP Batcher, if enabled.
- Shader error 'redefinition of sampler_linearClamp' in URP 14.0.7+
- Material UI, not respecting individual texture assignments when selecting multiple materials.

Removed:
(Deletions aren't tracked by Unity packages, these must be performed manually on your end)

- GrassBendingFeature class.

1.3.4

Added:
- Colormap Renderer, texture detail parameter. Allows to filter out high frequency details.
- Stylized Grass Renderer component, wind zone Main and Turbulence multipliers. Allows wind to be normalized to match potentially other wind-enabled shaders using the same Wind Zone.

Changed:
- Colormap Renderer, will now restore the terrain's original vegetation drawing setting after rendering (Nature Renderer use case).
- Colormap Renderer, will no longer enable Directional Lights that were intended to remain disabled (eg. lighting system with a disabled moon during daytime).
- Colormap Renderer, now creates a temporary empty renderer through script, to prevent any third-party post processing effects from interfering

Fixed:
- Console warnings about demo scene scripts not being part of an assembly
- Wind gusting accelerating over time when tied to a Wind Zone

Removed:
- Stylized Grass Shader/Runtime/Renderers folder, now obsolete

1.3.3

Added:
- Integration for Nature Renderer 2021

Changed:
- Vegetation Studio Pro integration is no longer default, as to not interfere with the Hybrid Renderer.

Fixed:
- Preemptive shader error fix for Unity 2023.1.0a26+
- Shader warning thrown in 2021.2+ related to using the Graphics Debugger
- Materials not receiving shadows, or showing banding artefacts, when a single shadow cascade is used. (In Unity 2021.3.15+, or versions released after December 15th 2022)

1.3.2

Added:
- Support for Unity 2022.2
- Support for the Forward+ rendering path
- Support for dynamic wind directions from a Wind Zone component
- Wind: Gusting speed material parameter
- "_Seed" parameter for use with the Hybrid Renderer in DOTS (0.5.0 and 1.0.0+)

Fixed:
- Min/max material parameters not displaying override options on material variants
- Grass bending pass no longer executes for overlay cameras.

1.3.1

Fixed:
- Terrain material being overriden when using the Colormap Renderer with specific settings.
- Preliminary fixes for Unity 2023.1.0

1.3.0
Minimum supported version is now Unity 2020.3 and URP 10.3.2

- Reworked the grass bending functionality for greatly improved performance.
- Restructured folders to follow the UPM standard. Scripts now in a separate Runtime/Editor folder.

Added:
- Prefabs specific to use with the terrain system (without LODs)
- Normal maps for all grass textures
- Masking Sphere component. Fades out the grass towards a single point, with a given radius.
- GI support for DOTS Instancing (Hybrid Renderer)
- Options for near/fade camera fading

Changed:
- Recreated most grass textures for improved shape and density.
- Scale map is no longer experimental
    * Transitions between different terrain layers are now seamless
    * Grass scale defaults to 100% for any layers not configured
- Translucency shading is now greatly improved for orthographic cameras.
- Color Map Renderer now respects individual materials assigned to terrain tiles.
- Reworked material UI to support multi-selection and Material Variants. This causes the following parameters to be reset:
    * Normal parameters
    * Distance fading parameters
    * Environment reflections toggle
    
Fixed:
- Alpha clipping value no longer influences the angle fading functionality.
- Color map rendering as completely red if the terrain had 0 height.
- Grass appearing shorter if the "Apply scale map" option was enabled, yet no scale map was present
- Sparkles appearing when the grass geometry occupies <1px on the screen and MSAA is enabled.
- Certain terrain layers not being configurable in the layer-based grass scale options

1.2.2

Fixed:
- Grass bending appearing flipped in Unity 2022.1
- Edge fading being inverted when using the OpenGLES graphics API

1.2.1
Minimum supported version is now 2019.4.15f1

Added:
- Emission color parameter (HDR)
- Vegetation Studio Pro LOD debug now also shows on grass

Fixed:
- Wind direction not matching up between ambient wind and gusting

1.2.0

Added:
- Translucency 
    * Offset parameter. Controls how much the effect wraps around the mesh
    * Falloff parameter. Controls the size of the effect (previously hardcoded to a value of 4, which is now default)
    * Indirect strength parameter. Shading can now also be performed for light hitting the front of the grass.
- Parameters to control normals are now available in VSP (requires the prefab to be refreshed)
- Option to invert the fading distance

Changed:
- When activating the Nature Renderer or GPU Instancer integration. Filepaths in the shader are now automatically rewritten based on their current install location
- Grass bending now fades out at a constant 2 units towards the edge of the render area, no matter its size.

Fixed:
- Translucency shading being wrongly affected by the grass color as it appears after lighting
- External material changes (eg. source control) not being reflected in the material UI

1.1.9

Fixed:
- Color map renderer not being able to find the necessary renderer when first set up
- Grass bending not taking affect at higher elevations

1.1.8

Fixed:
- Shader not compiling in WebGL builds in Unity 2019.3-2020.2

1.1.7
Minimum supported version is now Unity 2019.4.0 and URP 7.4.1

Added:
- Flatten normals parameter added. Lighting will treat the mesh as if its normals are pointing straight up.
- Build preprocessor script which strips shader variants for features only available in newer URP versions, improves build times
- Specular highlights toggle and smoothness parameter
- Support for Rendering Debugger and Decals in 2021.2.0b1

Changed:
- Bending renderer can now follow the current camera rendering, so is no longer limited to a single area
- Bending renderer, Mask Edges option is no longer needed, functionality now is incorporated into the shader, which is ultimately faster.
- Normals of grass meshes are now hard-shaded. This is required for the depth-normals pass in URP10+ and greatly improves the SSAO behaviour.
- Added option to disable jittering for trail benders (Force updating)

Fixed:
- Shader errors in Unity 2021.2.0b1 (alpha version is now obsolete, and will break instead).

Removed:
- Shadow bias correction option, unfortunately required to make room for newer URP features and reduce shader variants

1.1.6

Added:
- Preliminary support for Deferred rendering in URP 12+. Due to its nature, translucency will not work with this rendering path
- Configured _BaseColor as an exposed parameter for Hybrid Renderer v2 (more can be added following the same structure in the Input.hlsl file)
- Support for LOD crossfading in Vegetation Studio Pro beta 1.4.2.1

Changed:
- Improved LOD fading + workaround for URP bug where cast shadows flicker during the transition
- Color map rendering now supports terrains with varying height positions
- Trail grass benders now have their position slightly jittered, so their lifetime always updates even when stationary

Fixed:
- Material UI now showing the correct current wind strength, when a Wind Zone is used.

1.1.5
Revised demo content to showcase more grass and in larger varieties

Added:
- Flowers: Aster/Chamomile/Dandelions/Speedwell/Shrub
- Ground cover: Clovers/Daisies/Poppies
- Bending tint color, multiplies the base color by this value, where ever something bends the grass
- Spherify normals parameter, interprets the mesh as a sphere, resulting in fluffy shading

Changed;
- Refactored shader to more closely adhere to the standard URP structure. Now supports point/spot light shadows
- Trail benders now also support alpha blending (including particle trails)

Fixed:
- Colormap render not encapsulating terrain if larger than 4x4k and positioned extremely far from the world origin
- Scripting errors in URP 12/2021.2+ due to now obsolete code.

1.1.4

Added:
- Unlit shading mode, for projects using flat shading or targeting high performance 

Changed:
- SSAO in URP 10 now also properly takes into account point/spot lights

Fixed:
- Bright sparkles in some situations where HDR is used and bright skybox lighting
- SSAO appearing with dithered artifacts on grass in URP 11

1.1.3

Added:
- Support for Depth Normals pre-pass and SSAO in URP 10+

Changed:
- Minor UI improvements for clarity

1.1.2

Added:
- Billboarding mode, this requires the "GrassBillboardQuad" mesh. A new prefab configured for billboarding has been added.
- Line Renderers can now also be used for grass bending
- Angle fading feature, dithers the faces not directed at the camera, making the mesh's planes less noticeable  (works best without shadow casting)

Fixed:
- Skybox reflection and fog are temporarily disabled when rendering a color map, as their effects may end up being rendered into the texture

Changed:
- Improvements to grass bending, where any flickering is now entirely eliminated. 
- Color map is now rendered by rendering terrains with an unlit shader for increased color accuracy. Previous method still available through a checkbox, when using a third-party terrain shader.
- Minimized CG allocation in grass bending render loop
- Enabled LOD crossfading on all prefabs by default

Removed:
- Support for directional lightmaps, this is unlikely to ever be used with grass and halves the amount of shader variants

1.1.1

Added:
- Support for Curved World 2020. This has to be activated through the Curved World shader activator. Options will then appear under the "Vertices" section. Grass bending not supported.
- Support for GPU Instancer, this must be enabled at the bottom of the material UI

Fixed:
- Render features like third-party post processing effects no longer inadvertently apply to the color map render.
- Shader errors for grass bending when using Single Pass Instanced rendering

Changed:
- Polished material UI through new template. Sections now smoothly expand/collapse.
- Main color parameter is now HDR-enabled, allowing emissive grass

1.1.0

Added:
- Colormap Renderer can now also render a scale map for grass, based on terrain layers. Allows grass scale to be determined by painted terrain layer strength.
	Material now has a Vector3 field to control the scale influence for the XYZ axis. The "Apply scalemap" toggle must be enabled on a material.
- "Shadow banding correction" toggle. Offsets the shadows received by the grass a tiny bit. This avoids unwanted self-shadowing (aka shadow acne).
- Support for LOD dithered crossfading (requires to be enabled on LOD group)

Changed:
- Height offset for mesh grass benders is now also applied to the wireframe gizmo
- Active color map is now re-assigned whenever scene is saved, to avoid the link to the shader being broken
- "Bending" section in material UI was renamed to "Vertices".
- Bending render area edge masking now also works when not in play-mode.
- Added additional edge loop to LOD0 of meshes. Added a LOD2 mesh with fewer vertices.

Fixed:
- Color Map Renderer
	* Render area calculations when terrains have a negative position
	* Handling cases where terrains are entirely flat
- (Preemptive) Warning about obsolete method in URP 9.0.0+ (2020.2)
- Shader error in URP 9.0.0, due to changes in URP lighting functions.
- Environment reflections not correct when a normal map was used on curved meshes.

Removed:
- Specular highlight parameter, this was broken to begin with and translucency ends up offering the same visual result.

Note: Prefabs in Vegetation Studio Pro have to be refreshed to avoid shadow flickering issues

1.0.9
Verified compatibility with URP 7.4.1

Added:
- Support for Nature Renderer's "Procedural Instancing".
	* An option has been added at the bottom of the grass material to switch between Vegetation Studio and Nature Renderer integrations
- "GrassSingle" model/prefab. A version of the GrassPatch prefab but with a single tuft.

Changed:
- Revised demo scene
- When using the Simple Lighting mode, the color map is now also sampled per-pixel (texture would otherwise multiple over it)

Fixed:
- Bending area being flipped in URP 7.4.1
- Scenario where objects other than terrains could be rendered into the color map
- Sun light adding additional light when at a steep angle, causing color map to not match up with terrain
- Translucency now handles HDR light colors better

1.0.8

Changed:
- Bending renderer now moves to follow target transform intermittently, rather than constantly. This eliminates any possible movement flickering in the grass.
- Corrected settings for BendParticles prefab, it can now also be used as a child object.
- Wind gust tint no longer applies to emission, so will respond correctly to lower direct light intensity (eg. dawn/dusk/night)

Added:
- Wheat texture and prefab
- Thin grass texture and prefab
- StylizedGrassRenderer script now has a static SetPosition(Vector3 position) function

1.0.7
Minimum required URP version: 7.2.0. Tested in 7.2.1/7.3.1/8.0.1

Added:
- Alpha to coverage toggle to shader, reduces aliasing when using MSAA
- Thick grass texture and prefab

Changed:
- Wind gust tint is now applied after translucency, resulting in more natural lighting behaviour.
- Set all grass textures to "Clamp" to avoid thin lines at tip of grass at lower mipmaps
- Expanded state of material sections are now saved until Unity is closed

1.0.6
Minimum required URP version: 7.2.0. Tested in 7.2.1/7.3.1/8.0.1

Added:
- Perspective Correction parameter. Controls the amount by which the grass is gradually bent away from the camera as it looks down. Useful for better coverage in top-down perspectives

Changed:
- Improved visual quality of demo scenes through added color grading LUT and skybox

Fixed:
- Bug when using an override texture on a color map asset would cause it to be modified when re-rendering
- Bug where grass would still sample a color map when switching to a scene without one active
- Deleted hidden cameras from demo scenes, which caused a significant performance drop (development remnants)
- Bright pixels at tip of grass in situations where translucency is prominent and MSAA is used.
- Bending render area being flipped in URP 7.3.1

1.0.5
Minimum required URP version: 7.2.0. URP 7.3.1 compatibility verified. 

Note: Color maps have to be re-rendered in order to update their information to a new format!

Changed:
- Color map textures are now compressed, resulting in a substantial memory saving.

Fixed:
- Errors thrown on the ColorMapRenderer component when no ColorMap asset was assigned
- Bending render area being flipped in URP 7.3.1 (Make up your mind, Unity!)

1.0.4
URP version requirement: 7.2.0+

Added:
- Sorting layer to Grass Bender component. Higher layers are drawn over lower layers. Can be used to control which benders should overwrite other benders.
- Particle trail prefab, as an alternative to using a grass bender with a trail renderer.
- Color map height parameter, allows the color map to only take effect at the root of a grass mesh

Changed:
- Grass benders using a particle system in World simulation space are now supported
- Grass benders outside of the render area are no longer processed

Fixed:
- URP installation dialog not having any effect due to changes in the Package Manager
- Color map not rendering correctly if terrains were moved away from the world origin
- (Preemptive fix) Grass bending render area being flipped in URP 8.0.1+ (Apparently was a mistake in 7.2.0)

1.0.3
Changed:
- Translucency color is now mixed with grass color through overlay blending, instead of directly using the sun color. 
  A white directional light on green grass results in a yellow-ish tint, as expected.

Fixed:
- Renderer component incorrectly displaying an error regarding URP installation
- Added missing materials for Alien and Tall grass prefabs
- Grass bending render area being flipped (Occurs since URP 7.2.0)
- Error on MacOS when using OpenGL regarding unsupported render texture format

1.0.2
Package now requires Universal Render Pipeline 7.2.0+

If the package is imported without the URP installed, a dialog will now open with the option to install it.

Fixed:
- Dark edge showing at borders of shadow cascades (Occurs since URP 7.2.0)
- Warning in Lighting shader library

1.0.1
Added:
- Grass Renderer component now has a WindZone field. When enabled, ambient and gust strength are influenced by its "Main" value

Changed:
- Wind tint is now masked by shadows
- Translucency effect now fades out when Directional Light approaches to the horizon
- Shader UI will show warnings about reflection settings when incompatible scene lighting settings are used

Fixed:
- Tiling/Offset values for normal map had no effect
- Color map rendering from mesh terrains can now automatically calculate the correct render area size

1.0.0
Initial version