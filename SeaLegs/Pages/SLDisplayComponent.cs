
using System.Numerics;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SeaLegs.Controllers;

namespace SeaLegs.Pages
{
    public class SLDisplayComponent : ComponentBase, IDisposable
    {
        //Core set-up
        public BECanvasComponent? canvas { get; set; }
        private Canvas2DContext? context { get; set; }
        private IJSObjectReference? JSModule {  get; set; }
        [Inject]
        public IJSRuntime? JsRuntime { get; set; }

        //Canvas dimensions
        [Parameter]
        public int Width { get; set; } = 0;
        [Parameter]
        public int Height { get; set; } = 0;
        public string? wrapperWidth { get; set; }
        public string? wrapperHeight { get; set; }

        [Parameter]
        public int AspectRatioWidth { get; set; } = 16;
        [Parameter]
        public int AspectRatioHeight { get; set; } = 9;

        private DotNetObjectReference<SLDisplayComponent>? dotNetReference { get; set; }

        //Gameplay Loop
        private System.Threading.Timer? gameTimer { get; set; }
        private DateTime lastFrameTime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && JsRuntime != null)
            {
                //Set-up key components ---------
                JSModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./SeaLegs/Pages/SLDisplay.razor.js");
                context = await canvas.CreateCanvas2DAsync();

                //Canvas resizing set-up ---------
                //If dimensions aren't defined in razor page, calculate dimensions according to aspect ratio
                if (Width == 0 && Height == 0)
                {
                    await UpdateCanvasDimensions();
                    StateHasChanged();

                    dotNetReference = DotNetObjectReference.Create(this);
                    await JSModule.InvokeVoidAsync("setupResizeHandler", dotNetReference);
                }
                

                //Transfer components to Lib system classes as needed
                SendComponentsToLib();
                SendDimensionsToLib();

                //Set-up gameplay loop
                lastFrameTime = DateTime.Now;
                gameTimer = new System.Threading.Timer(GameLoop, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(16));
            }
        }

        #region Canvas Sizing
        [JSInvokable]
        public async Task OnBrowserResize()
        {
            await UpdateCanvasDimensions();
            StateHasChanged();
            SendDimensionsToLib();
        }

        private async Task UpdateCanvasDimensions()
        {
            if(AspectRatioWidth != 0 && AspectRatioHeight != 0 && JSModule != null)
            {
                var dimensions = await JSModule.InvokeAsync<BrowserDimensions>("getBrowserDimensions");
                             

                if (AspectRatioWidth > AspectRatioHeight)
                {
                    double horizontalScale = (double)(dimensions.width / AspectRatioWidth);
                    Width = dimensions.width;
                    Height = (int)(horizontalScale * AspectRatioHeight);
                }
                else
                {
                    double verticleScale = (double)(dimensions.height / AspectRatioHeight);
                    Height = dimensions.height;
                    Width = (int)(verticleScale * AspectRatioWidth);
                }

                wrapperWidth = $"{Width}px";
                wrapperHeight = $"{Height}px";
            }
        }
        #endregion

        #region Data Transfer
        private void SendComponentsToLib()
        {
            if (canvas != null && context != null && JSModule != null)
            {
                CanvasController.SetCoreComponents(canvas, context, JSModule);
            }
        }

        private void SendDimensionsToLib()
        {
            CanvasController.SetDimensions(Width, Height);
        }

        public void HandleKeyDown(KeyboardEventArgs args)
        {
            InputController.SetKeyPressed(args.Key, true);
            InputController.AddKeyDown(args.Key);
        }

        public void HandleKeyUp(KeyboardEventArgs args)
        {
            InputController.AddKeyUp(args.Key);
        }

        public void HandleMousePosition(MouseEventArgs args)
        {
            Vector2 mousePos = new Vector2((float)args.ClientX, (float)args.ClientY);
            InputController.SetMousePosition(mousePos);
        }

        public void HandleMouseDown(MouseEventArgs args)
        {            
            InputController.SetMouseDown(true);
            InputController.SetMouseUp(false);
        }

        public void HandleMouseUp(MouseEventArgs args)
        {
            InputController.SetMouseUp(true);
            InputController.SetMouseDown(false);
        }

        #endregion

        #region Game Loop
        //To set-up game, call root game class Update method from CanvasController
        private async void GameLoop(object state)
        {
            var currentTime = DateTime.Now;
            var deltaTime = (float)(currentTime - lastFrameTime).TotalSeconds;
            lastFrameTime = currentTime;

            await RunGame(deltaTime);
        }

        private async Task RunGame(float deltaTime)
        {
            await CanvasController.Update(deltaTime);
        }

        public void Dispose()
        {
            if (dotNetReference != null && JSModule != null)
            {
                JSModule.InvokeVoidAsync("removeResizeHandler");
                dotNetReference.Dispose();
            }
        }
        #endregion

        //BrowserDimensions class is used to return size of browser from JSInterop
        class BrowserDimensions
        {
            public int width {  get; set; }
            public int height { get; set; }
        }
    }
}
