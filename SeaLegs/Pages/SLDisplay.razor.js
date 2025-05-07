
//Browser window sizing and resizing
export function getBrowserDimensions() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
}

let resizeEventHandler = null;
let dotNetReference = null;

export function setupResizeHandler(componentReference) {
    dotNetReference = componentReference;
    resizeEventHandler = () => {
        dotNetReference.invokeMethodAsync('OnBrowserResize');
    };
    window.addEventListener('resize', resizeEventHandler);
}

export function removeResizeHandler() {
    if (resizeEventHandler) {
        window.removeEventListener('resize', resizeEventHandler);
        resizeEventHandler = null;
        dotNetReference = null;
    }
}

//Audio handling

const loopingSounds = {};

export function playSound(path, volume, loop) {
    try {
        var audio = new Audio(path);
        audio.volume = volume;
        audio.loop = loop;
        audio.play();
        if (loop) {
            loopingSounds[path] = audio;
        }

    } catch (error) {
        console.error("Error playing sound:", error);
    }
}

export function stopSound(path) {
    if (loopingSounds[path]) {
        loopingSounds[path].pause();
        loopingSounds[path] = null;
    }
}
