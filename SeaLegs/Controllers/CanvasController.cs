using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions;
using Microsoft.JSInterop;
using SeaLegs.Core;
using SeaLegs.Data;
using System.Numerics;


namespace SeaLegs.Controllers
{
    public class CanvasController : Controller
    {
        public static BECanvasComponent? canvas { get; private set; }
        public static Canvas2DContext? context { get; private set; }
        public static IJSObjectReference? JSModule { get; private set; }

        public static Func<float, Task>? UpdateAction { get; set; }

        public static int width { get; private set; }
        public static int height { get; private set; }
        public static Vector2 scale { get; private set; }


        public static void SetCoreComponents(BECanvasComponent canvas, Canvas2DContext context, IJSObjectReference JSModule)
        {
            CanvasController.canvas = canvas;
            CanvasController.context = context;
            CanvasController.JSModule = JSModule;
        }

        public static void SetDimensions(int width, int height)
        {
            CanvasController.width = width;
            CanvasController.height = height;

            //Scale ensures consistent look and feel regardless of canvas size.
            //Design width/height can be updated in GameSettings class
            //These dimensions should be set to the target dimensions the game was designed around.
            float horizontalScale = GameSettings.DesignWidth / width;
            float verticalScale = GameSettings.DesignHeight / height;
            scale = new Vector2(horizontalScale, verticalScale);
        }

        public static void SetCallback(Func<float, Task> callback)
        {
            UpdateAction = callback;
            Console.WriteLine("Setting up callback method");
        }

        public static async Task Update(float deltaTime)
        {
            if (CanvasController.context != null && UpdateAction != null && CanvasController.JSModule != null)
            {
                await RenderingController.DrawBlackBackground();
                await UpdateAction.Invoke(deltaTime);
            }
            
        }
    }
}
