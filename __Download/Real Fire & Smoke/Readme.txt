----------------------------------------
Real Fire & Smoke by Archanor VFX
----------------------------------------

1. Introduction
2. Scaling effects
3. URP Upgrade
4. Contact

----------------------------------------
1. INTRODUCTION
----------------------------------------

The asset is created in Built-in with included URP upgrades.

--

To get started using the effects, locate the particle effect prefabs in 'Real Fire & Smoke/Prefabs', then drag and drop one of the prefabs to your scene and start your scene to preview it. You can also test them in the Scene view and use the particle system controls to play, pause or stop the systems.

If you want to run the demo scenes in Unity, locate the scenes in 'Real Fire & Smoke/Demo/Scenes' and drag them to your "Scenes in Build" in the Build Settings window. This will let you use the in-game GUI to change scenes.

----------------------------------------
2. SCALING EFFECTS
----------------------------------------

To scale an effect in the scene, you can simply use the default Scaling tool (Hotkey 'R'). You can also select the effect and set the Scale in the Hierarchy.

Please remember that some parts of the effects such as Point Lights, Trail Renderers and Audio Sources may have to be manually adjusted afterwards.

----------------------------------------
3. URP UPGRADE
----------------------------------------

To upgrade the asset to URP, open 'RealFire URP Upgrade (2022.3.21f1)' and import it to your project.

-

To make sure the particles are rendering correct, to enable Depth Texture and Opaque Texture in your SRP settings. Otherwise they may end up not showing up in your project/build.

You can find the SRP settings in 'Edit/Project Settings/Graphics'. 

An alternative to enabling Depth Texture is to disable the 'Soft Particles' setting for the materials in the asset.

Keep in mind that 'Soft Particles' does not work for 2D projects that use orthographic cameras.

----------------------------------------
4. CONTACT
----------------------------------------

If you ran into a problem, please visit my support webiste.

https://archanor.com/support.html