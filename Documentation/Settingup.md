### Basic Set-up
The SLDisplay razor page is already set-up within the library to be implemented in any other razor page. In order to access this component, we simply include the <SLDisplay> tag in the razor page to begin rendering the canvas. Below is an example of the Home.razor page I set-up while building out this project:

### Home.razor

```
@page "/"
@using SeaLegs
@using SeaLegs.Pages
@using SeaLegs.Data

<PageTitle>Home</PageTitle>

<SLDisplay AspectRatioWidth="16" AspectRatioHeight="9"/>

@code {
  Game.App myApp { get; set; } = new Game.App();

  protected override void OnInitialized()
  {
	//Set-up base settings
	GameSettings.UpdateDesignDimensions(1920, 1080);

	//Load graphics
	GraphicsData.AddGraphicToList("Assets/player.png");
  }
}
```

The <SLDisplay> element loads in the razor page from the library with all of the boilerplate code needed. There are two options for setting this up. The first, which I recommend, is to use the AspectRatioWidth and AspectRatioHeight parameters to define the aspect ratio of your game. In this case I'm using 16x9. This will cause the canvas to stretch across the entire width of the browser and adjust the height of the canvas accordingly, regardless of screen size. This also means that as the browser window is resized, the canvas will maintain the aspect ratio.

**Using Aspect Ratio**
```
<SLDisplay AspectRatioWidth="16" AspectRatioHeight="9"/>
```

The other way to set-up the canvas is by defining a specified Width and Height as parameters, for example Width=1080 and Height=720. This will give a definite canvas size regardless of the size of the browser window. It must be noted here that when these parameters are used, the library ignores the aspect ratio and any browser window resizing. This can be usefull if you specifically want a certain screen size for your game.

**Using Dimensions**
```
<SLDisplay Width="1080" Height="720"/>
```
**Game App Initialization**
Within the code block we have a few more things we need to set-up. First is instantiating our application. In your project you might have a separate folder that handles all of your game logic. In the @code block, we want to make sure the root class for the game is initialized.

```
Game.App myApp { get; set; } = new Game.App();
```

**Setting the base dimensions**
The next line of code here is to define the base dimensions of the game that you may have designed your game around. This line of code is important because it tells the Library how to scale the game so that it looks and feels the same regardless of the canvas size. It's viable here to use common dimensions that fit within your aspect ratio. For example, since my aspect ratio is 16x9, I opted for 1920x1080 for the base dimensions. If this isn't properly set-up it will stretch assets and cause unwanted behaviors when moving entities on the canvas.

```
GameSettings.UpdateDesignDimensions(1920, 1080);
```

**Loading in graphics**
In order to load in our graphics we need to tell the library which images from or wwwroot folder we will be using in the project. This line of code tells the library to create <img> elements on the <SLDisplay> razor page for each of the graphics listed, which are then stored as ElementReferences in the data structure of the library to be retrieved from the game logic. In my example here I'm using a folder called Assets and loading in player.png. This path "Assets/player.png" should be changed to suit your project.

```
GraphicsData.AddGraphicToList("Assets/player.png");
```

### App.cs
The next little bit of set-up we need is to create our root class for our project. This can be called whatever you wish, but I've opted for App. Just ensure to adjust the code appropriately in the example Home.razor page above.

**Example Class**
```
using System.Numerics;
using Microsoft.AspNetCore.Components;
using SeaLegs.Controllers;
using SeaLegs.Data;

namespace SeaLegsTestEnvironment.Game
{
    public class App
    {
        public App()
        {            
            CanvasController.SetCallback(this.Update);
        }

        public async Task Update(float deltaTime)
        {
            //Game logic here
        }
    }
```

We need to make sure this class has two things: An Update method(called every frame) and connecting that update method to the SeaLegs library. The update method needs to be asynchronous and return a Task, as well as take in a float as a parameter. This float is our deltaTime variable to ensure graphic movement and animations within the game are FPS independent. The next line we place in the constructor, which tells SeaLegs to call the new Update method each frame of the thread for our gameplay loop.

```
CanvasController.SetCallback(this.Update);
```

And that's it! Now you have a canvas on the screen ready to use for game development. All of the boiler plate code has been handled within the library.
