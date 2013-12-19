BaseFramework
=============

Some common architectural code I can reuse for other Unity3D Projects.

AI
==
Feature Branch: feature/AI

The 'AI' feature is currently an in-progress implementation of a Behaviour-Tree framework. Plans for this feature include a Unity editor window, deffered actions, and build-time production of a Behaviour-Stream.


Audio
=====
Feature Branch: feature/DigitalSignalProcessing

Currently, the 'Audio' feature is an in-progress implementation of Onset Detection. Onset Detection is the means in which one can determine BPM and Beats of a given song. The implementation is nowhere near complete.


Core
====
Feature Branch: feature/Core

The 'Core' feature includes class extensions and various static helper/utility methods.


Editor Utilities
================
Feature Branch: feature/EditorUtils

The 'Editor Utilities' feature includes definitions of generic editor windows, and provides functionality for common tasks needed to be performed from Unity Editor Scripts.


TouchGestures
=====
Feature Branch: feature/Input

The 'TouchGestures' feature provides functionality for touch-screen gestures. Gestures can also be performed with the mouse in the Unity Editor.


Prefab Pool
===========
Feature Branch: feature/PrefabPool

Originally part of the 'Core' feature, the 'Prefab Pool' feature has become its own due to its implicit reusability. The framework was in development about a year ago and after being abandoned, most likely requires a partial rewrite.
