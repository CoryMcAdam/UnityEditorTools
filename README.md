# Unity Editor Tools

A collection of editor tools to help improve workflow in Unity.

### Features:
- Scene Selector - Select and load scenes from the scene view.
- Play from Scene - Enter play mode from a selected scene.
- Hierarchy Headers - Organise your scene hierarchy.

## Scene Selector

The scene selector overlay lets you save scenes and easily switch between them from a dropdown menu in the toolbar.

![](https://i.imgur.com/vTw1NpY.png)

- The dropdown menu will display a list of all the saved scenes.
- Scenes can be added/removed using the + and - buttons. Scenes with missing scene paths should automatically remove themselves such as when a scene is renamed or deleted.

### Things to Note:

- When entering play mode, the menu will be locked.
- The dropdown menu shows the currently selected scene. However, in older version of Unity the scene name isn't visible while docked.

![](https://i.imgur.com/fdTY6NR.png) ![](https://i.imgur.com/0BmPs2D.png) 

- The overlay can also be moved to the scene view where it should always display the current scene name.

![](https://i.imgur.com/fXDwG66.png)

## Play From Scene

The play from scene overlay lets you favourite a scene to enter play mode from. When the play button is pressed it loads the favourited scene and automatically enters play mode. When leaving play mode it will then reload the previously opened scene.

![](https://i.imgur.com/7ypgjgV.png)

- The play button will enter play mode from the favourited scene.
- The select scene button will display the currently saved play mode scene. Pressing it will set the current scene as the play mode scene.

### Things to Note:

- Unlike the normal play button, unsaved changes will be lost when entering from a favourited scene.
- Pressing the play button will prompt you to save the current scene if changes are detected.

## Hierarchy Headers

Hierarchy headers let you add empty GameObjects to the scene hierarchy that are formatted to help organise the scene.

![](https://i.imgur.com/OVyIa2t.png)

To create a header simply add "---" before the headers name and set the GameObjects tag to "EditorOnly".

![](https://i.imgur.com/QdUbFeT.png)

# Installation
To install the package, open the package manager, click the + icon and select "Add package from git URL...".
![](https://i.imgur.com/hWZdAaS.png)

Once installed the overlays can be enabled from the upper right corner of the scene view.
![](https://i.imgur.com/aYfyxkZ.png)
