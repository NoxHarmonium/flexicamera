flexicamera
===========

A flexible camera system for Unity3D

This camera plugin for Unity3D aims to be multipurpose camera that makes it easy to combine several different camera influences gracefully to enable things like tweening of the camera. At the moment it isn't actually very flexible as I'm developing it with a work project in mind, but I aim to expand the functionality in my free time.

Currently Implimented Controllers
---------------------------------
- Flyover (pan)
- Zoom
- Orbit
- Pan Into Bounds
- Zoom Into Bounds

Currently Implimented Input Adapters
------------------------------------
- FingerGestures v3

Usage
-----
If you're developing on OSX, I recommend checking out the repository into a seperate folder and adding a symlink in your Unity project's plugins folder to the relivant folder in the repository. Otherwise you can just copy the 'Assets/Plugins/FlexiCamera' folder into your plugins directory.

Architecture
------------
Input messages are generated every update which are sent to the camera controllers. The controllers process the input and then optionally return modifiers. The modifiers are applied to the camera transform one after then other in a pipeline. The controllers perform processing independant of each other and therefore can be run in parallel. If there dependencies that can't be avoided, multiple passes can be used. 


