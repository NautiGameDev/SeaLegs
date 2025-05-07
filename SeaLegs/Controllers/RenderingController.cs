using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SeaLegs.Controllers
{
    public class RenderingController
    {
        //Bare-bones draw method. Draw sprite at position. Not recommended as sprite doesn't scale with canvas size
        public static async Task Draw(ElementReference sprite, Vector2 position)
        {
            if(CanvasController.context != null)
            {
                await CanvasController.context.DrawImageAsync(sprite, position.X, position.Y);
            }
        }

        //Most common draw method. Draw sprite at position with a given scale
        public static async Task Draw(ElementReference sprite, Vector2 position, Vector2 scale)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.DrawImageAsync(sprite, position.X, position.Y, scale.X, scale.Y);
            }
        }

        //Draw sprite to screen from a sprite sheet. Useful for animated sprites where spritesheet is split into x and y indexes.
        public static async Task Draw(ElementReference sprite, int animationIndexX, int animationIndexY, Vector2 scale, Vector2 position, Vector2 baseSpriteDimensions)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.DrawImageAsync(
                    sprite,
                    animationIndexX * baseSpriteDimensions.X,
                    animationIndexY * baseSpriteDimensions.Y,
                    baseSpriteDimensions.X,
                    baseSpriteDimensions.Y,
                    position.X,
                    position.Y,
                    scale.X,
                    scale.Y
                    );
            }
        }

        //Draws sprite to screen with rotation factored in. Used primarily for twin-stick shooter like mechanics.
        public static async Task Draw(ElementReference sprite, int animationIndexX, int animationIndexY, Vector2 scale, Vector2 position, Vector2 baseSpriteDimensions, double rotation, Vector2 rotationOrigin)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.SaveAsync();
                await CanvasController.context.TranslateAsync(rotationOrigin.X, rotationOrigin.Y);
                await CanvasController.context.RotateAsync((float)rotation);
                await CanvasController.context.DrawImageAsync(
                    sprite,
                    animationIndexX * baseSpriteDimensions.X,
                    animationIndexY * baseSpriteDimensions.Y,
                    baseSpriteDimensions.X,
                    baseSpriteDimensions.Y,
                    -(scale.X/2),
                    -(scale.Y/2),
                    scale.X,
                    scale.Y
                    );
                await CanvasController.context.RestoreAsync();
            }
        }

        public static async Task<ElementReference> LoadImage(string path)
        {
            try
            {
                if (CanvasController.JSModule != null)
                {
                    Console.WriteLine($"Attempting to load image from path: {path}");
                    int imageId = await CanvasController.JSModule.InvokeAsync<int>("LoadImageAsElementReference", path, 1);
                    Console.WriteLine("Image loaded");
                    ElementReference newImage = await CanvasController.JSModule.InvokeAsync<ElementReference>("GetImageElementReference", imageId);
                    
                    return newImage;
                }
                else
                {
                    Console.WriteLine("JSModule not found");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return default;
            }      
        }

        public static async Task DrawBlackBackground()
        {
            await CanvasController.context.SetFillStyleAsync("black");
            await CanvasController.context.FillRectAsync(0, 0, CanvasController.canvas.Width, CanvasController.canvas.Height);
        }
    }
}
