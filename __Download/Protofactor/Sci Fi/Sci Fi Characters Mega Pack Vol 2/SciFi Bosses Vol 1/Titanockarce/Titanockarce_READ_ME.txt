TITANOCKARCE RECOMMENDED USE

 First off, thanks for supporting us! We do hope you'll enjoy using this amazing asset in your project.

 This 3D animated model is meant to be used as a main boss enemy character in a SciFi genre project.

 Important: 
It's a very detailed character with a pretty heavy polycount, lots of bones in the rig and huge 8K PBR texture set. It is NOT meant to be used for targets such as mobile devices. Most likely polygon reduction tools etc... won't be enough to lower the polycount to a decent level as to keep the best visual quality up close. Also, there will be no way to reduce the amount of bones in the rig. Make sure to consider which platform you're targeting.

Recommended Use: 
We have designed this character so that developer can create an epic boss battle in their project.

 This boss battle should be created following these recommended phases: 

1. Destroy tentacles segments from tip to base. All tentacles are using split meshes attached to the rig. We have set up the model to have custom mesh colliders to cover his body and capsule colliders for each tentacles segments. Developer should use a particle emitter to make a segment explode and disable it, or un-parent the bone segment and add a rigid body to make it fall on the ground.
2.  Once all tentacles are destroyed, Titanockarce should use its stomp attacks as well as its stinger tail mega blast attack. This phase require the pulsating weak point on the stinger to be destroyed. The Stinger weak point bone already has a sphere collider set up 
3.  Once the stinger weak point is destroyed, Titanockarce can no longer use its tail mega bast. The only attacks it has left are the stomp attacks. At that point, some narrative should mention that some part of the shell on its back looks weaker and that players should concentrate their fire power on it. There is a part named “SM_Titanockarce_WeakPointBackShell”, this part uses a Game Object named “Collider_WeakPointBackShell”. Use this as a target. Once it's destroyed you can activate the split pieces:“SM_Titanockarce_WeakPointBackShell_SplitPart_x” and use gravity to make them fall on the ground. This will expose the final weak point. Player has to concentrate its fire on it to destroy it. 
4. Once all weak points are destroyed developer can trigger the “Death” animation. The boss is now finally defeated! 


This is the responsibility of developer to set up all game mechanics, tags, layers, VFX, SFX, animator controllers and AI. All these phases are just optional, this is how we envisioned this battle and that's why this model is set up the way it is. Our recommendation can be disregarded if you have already something specific in mind of course. 
