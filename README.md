# SeaLegs V0.1
SeaLegs is a library and Entity Component System designed to streamline game development in Blazor Webassembly. As of now, May 6, 2025, SeaLegs is a folder that can be placed in the project solution of a stand-alone Blazor Web Application. The goal is to eventually release this as a NuGet package, however, at the time of writing this there is still a lot of work to do.

At the moment this library is built upon the Blazor.Extensions.Canvas NuGet package, or BECanvas. More information about this extension can be found [here](https://github.com/BlazorExtensions/Canvas) and must be set-up appropriately before using the SeaLegs library. In the far future I plan on writing my own API for rendering graphics to the HTML Canvas element, with both 2Dcontext and WebGL support, but as this project is still in the early stages of the development cycle I'm building upon BECanvas to save some time.

## Current Version Notes:
The current version is fairly simple to get started and unfortunately lacks the ECS structure. At the moment (5/6/25), the project handles a lot of boilerplate code set-up that was previously required with setting up BECanvas. This includes the following controllers:
- CanvasController(Handles data and logic connecting any game application to the HTML Canvas)
- RenderingController(Handles rendering images on the canvas (2DContext only)
- InputController(Retrieves keyboard and mouse inputs in the browser and stores the data for retrieval to be used within a game)
- AudioController(Enables playing audio files and volume control through JSInterop)

As I continue to build small game projects and work with SeaLegs I will gradually be building a reusable component system to streamline the game development process even further. This will include everything from a level manager, entities within the game's world, camera, and UI elements.

## How to use
This section will be built upon over time during the development process. The instructions here are for functionality currently available in the library.

[Set-up](https://github.com/NautiGameDev/SeaLegs/blob/main/Documentation/Settingup.md)



