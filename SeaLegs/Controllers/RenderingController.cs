using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SeaLegs.Data;

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

        //Most common draw method. Draw sprite at position with a given dimensions (Scales with canvas size)
        public static async Task Draw(ElementReference sprite, Vector2 position, Vector2 dimensions)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.DrawImageAsync(sprite, position.X, position.Y, dimensions.X * CanvasController.scale.X, dimensions.Y * CanvasController.scale.Y);
            }
        }

        //Draw sprite to screen from a sprite sheet. Useful for animated sprites where spritesheet is split into x and y indexes.
        public static async Task Draw(ElementReference sprite, int animationIndexX, int animationIndexY, Vector2 dimensions, Vector2 position)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.DrawImageAsync(
                    sprite,
                    animationIndexX * dimensions.X,
                    animationIndexY * dimensions.Y,
                    dimensions.X,
                    dimensions.Y,
                    -((dimensions.X * CanvasController.scale.X) / 2),
                    -((dimensions.Y * CanvasController.scale.Y) / 2),
                    (dimensions.X * CanvasController.scale.X),
                    (dimensions.Y * CanvasController.scale.Y)
                    );
            }
        }

        //Draws sprite to screen with rotation factored in. Used primarily for twin-stick shooter like mechanics.
        public static async Task Draw(ElementReference sprite, int animationIndexX, int animationIndexY, Vector2 dimensions, Vector2 position, double rotation, Vector2 rotationOrigin)
        {
            if (CanvasController.context != null)
            {
                await CanvasController.context.SaveAsync();
                await CanvasController.context.TranslateAsync(rotationOrigin.X, rotationOrigin.Y);
                await CanvasController.context.RotateAsync((float)rotation);
                await CanvasController.context.DrawImageAsync(
                    sprite,
                    animationIndexX * dimensions.X,
                    animationIndexY * dimensions.Y,
                    dimensions.X,
                    dimensions.Y,
                    -((dimensions.X * CanvasController.scale.X)/ 2),
                    -((dimensions.Y * CanvasController.scale.Y)/ 2),
                    (dimensions.X * CanvasController.scale.X),
                    (dimensions.Y * CanvasController.scale.Y)
                    );
                await CanvasController.context.RestoreAsync();
            }
        }

        public static async Task DrawBlackBackground()
        {
            await CanvasController.context.SetFillStyleAsync("black");
            await CanvasController.context.FillRectAsync(0, 0, CanvasController.canvas.Width, CanvasController.canvas.Height);
        }
    }
}
